using Amongus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amongus.Domain
{
    public class AmongusContext : DbContext 
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=MELLISSSS\MSSQLSERVER01;Database=amongus;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
