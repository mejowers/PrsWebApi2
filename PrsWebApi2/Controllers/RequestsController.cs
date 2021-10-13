using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsWebApi2.Data;
using PrsWebApi2.Models;

namespace PrsWebApi2.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase {
        private readonly AppDbContext _context;

        public RequestsController(AppDbContext context) {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest() {
            return await _context.Requests.Include(r => r.user).ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id) {
            var request = await _context.Requests.Include(r => r.user)
                .SingleOrDefaultAsync(r => r.Id == id);

            if (request == null) {
                return NotFound();
            }

            return request;
        }

        // GET: api/Requests
        [HttpGet("request-lines/{id}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestByStatus(int id) {
            return await _context.Requests.Where(req => req.Status == "review" && req.UserId != id)
                .Include(r => r.user).ToListAsync();
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request) {
            if (id != request.Id) {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!RequestExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPut("submit-review")]
        public async Task<IActionResult> PutRequestReview(Request request) {
            return await PutRequestReview(request.Id, request);
        }

        // submit a change for the request approve if < 50.00 otherwise set to 'review'
        [HttpPut("submit-review/{id}")]
        public async Task<IActionResult> PutRequestReview(int id, Request request) {

            if (request.Total <= 50) {
                request.Status = "Approved";
            }
            else { request.Status = "Review"; }
            request.SubmittedDate = DateTime.Now;

            _context.Entry(request).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Requests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request) {
            request.user = null;

            request.Status = "new";
            request.SubmittedDate = DateTime.Now;

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Request>> DeleteRequest(int id) {
            var request = await _context.Requests.FindAsync(id);
            if (request == null) {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return request;
        }

        private bool RequestExists(int id) {
            return _context.Requests.Any(e => e.Id == id);
        }
        // put: change record without calling id number
        [HttpPut]
        public async Task<IActionResult> PutRequest(Request request) {
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("approve")]
        public async Task<ActionResult<Request>> PutRequestApprove(Request request) {

            request.Status = "Approved";

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return request;
        }
        [HttpPut("reject")]
        public async Task<ActionResult<Request>> PutRequestReject(Request request) {

            request.Status = "Rejected";

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return request;
        }
    }
}
