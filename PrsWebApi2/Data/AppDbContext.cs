using Microsoft.EntityFrameworkCore;
using PrsWebApi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrsWebApi2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<PrsWebApi2.Models.Vendor> Vendors { get; set; }

        public DbSet<PrsWebApi2.Models.Product> Products { get; set; }

        public DbSet<PrsWebApi2.Models.Request> Requests { get; set; }

        public DbSet<PrsWebApi2.Models.LineItem> LineItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(e => {
                e.HasIndex(p => p.Username).IsUnique();
            });
            {
                builder.Entity<Vendor>(e => {
                    e.HasIndex(p => p.Code).IsUnique();
                });
                {
                    builder.Entity<Product>(e => {
                        e.HasIndex(p => new { p.PartNumber, p.VendorId }).IsUnique();
                    });
                    {
                        builder.Entity<LineItem>(e => {
                            e.HasIndex(p => new { p.RequestId, p.ProductId }).IsUnique();
                        });
                    }
                }

            }


        }


    }
}
