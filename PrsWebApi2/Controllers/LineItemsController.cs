using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsWebApi2.Data;
using PrsWebApi2.Models;

namespace PrsWebApi2.Controllers
{
    [Route("api/line-items")]
    [ApiController]
    public class LineItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LineItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LineItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LineItem>>> GetLineItems()
        {
            return await _context.LineItems.Include(ri => ri.product).ToListAsync();
        }

        // GET: api/LineItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LineItem>> GetLineItem(int id)
        {
            var lineItem = await _context.LineItems.Include(ri => ri.product).ThenInclude(r => r.vendor)
                 .SingleOrDefaultAsync(r => r.Id == id); ;

            if (lineItem == null)
            {
                return NotFound();
            }

            return lineItem;
        }

        // GET: api/LineItems/Lines-for-pr/{Id}
        [HttpGet("lines-for-pr/{id}")]
        public async Task<ActionResult<IEnumerable<LineItem>>> GetAllByRequest(int id)
        {
            return await _context.LineItems.Where(li => li.RequestId == id).Include(li => li.product.vendor).ToListAsync();
        }

        // PUT: api/LineItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLineItem(int id, LineItem lineItem)
        {
            if (id != lineItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(lineItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await RecalculateTotal(lineItem.RequestId);

            return NoContent();
        }

        // POST: api/LineItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<LineItem>> PostLineItem(LineItem lineItem)
        {
            _context.LineItems.Add(lineItem);
            await _context.SaveChangesAsync();
            await RecalculateTotal(lineItem.RequestId);

            return CreatedAtAction("GetLineItem", new { id = lineItem.Id }, lineItem);
        }

        // DELETE: api/LineItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LineItem>> DeleteLineItem(int id)
        {
            var lineItem = await _context.LineItems.FindAsync(id);
            if (lineItem == null)
            {
                return NotFound();
            }

            _context.LineItems.Remove(lineItem);
            await _context.SaveChangesAsync();
            await RecalculateTotal(lineItem.RequestId);

            return lineItem;
        }

        private bool LineItemExists(int id)
        {
            return _context.LineItems.Any(e => e.Id == id);
        }

        public async Task RecalculateTotal(int requestId)
        {
            var request = await _context.Requests.FindAsync(requestId);
            request.Total = (from l in _context.LineItems
                             join p in _context.Products on l.ProductId equals p.Id
                             where l.RequestId == requestId
                             select new { Total = l.Quantity * p.Price })
                             .Sum(x => x.Total);
            var rc = await _context.SaveChangesAsync();
            if (rc != 1) throw new Exception("Fatal Error: Did not calculate.");
        }
    }
}
