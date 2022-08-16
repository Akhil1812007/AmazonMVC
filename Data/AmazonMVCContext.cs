using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AmazonMVC.Models;

namespace AmazonMVC.Data
{
    public class AmazonMVCContext : DbContext
    {
        public AmazonMVCContext (DbContextOptions<AmazonMVCContext> options)
            : base(options)
        {
        }

        public DbSet<AmazonMVC.Models.Customer> Customer { get; set; } = default!;
    }
}
