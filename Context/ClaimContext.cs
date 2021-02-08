using Microsoft.EntityFrameworkCore;
//using System;
using IC3000.Models;
//using Microsoft.Extensions.Configuration;
//using System.IO;

namespace IC3000.Context
{
    public class ClaimContext : DbContext
    {
        public DbSet<Claim> Claims { get; set; }
        public ClaimContext(DbContextOptions<ClaimContext> options) : base(options)
        { }

    }
}