using API.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base()
        {

        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Map Entity names to the Db Table Names
            modelBuilder.Entity<PresPoll>().ToTable("PresPolls");
            modelBuilder.Entity<Poll>().ToTable("Polls");

           
        }

        public DbSet<PresPoll> PresPolls { get; set; }
        public DbSet<Poll> Polls { get; set; }
    }
}
