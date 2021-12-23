#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppDelBagno.Models;

namespace AppDelBagno.Data
{
    public class AppDelBagnoContext : DbContext
    {
        public AppDelBagnoContext (DbContextOptions<AppDelBagnoContext> options)
            : base(options)
        {
        }

        public DbSet<AppDelBagno.Models.Bagno> Bagno { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=192.168.100.2;user=bim;password=Quink$2100;database=test")
                .UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole().AddFilter(level => level >= LogLevel.Information)))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
    }
}
