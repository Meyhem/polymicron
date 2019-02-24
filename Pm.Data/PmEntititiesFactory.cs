using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Pm.Data
{
    public class PmEntititiesFactory : IDesignTimeDbContextFactory<PmEntities>
    {
        public PmEntities CreateDbContext(string[] args)
        {
            var cfg = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            Console.WriteLine(cfg.GetConnectionString("PmEntities"));
            var opts = new DbContextOptionsBuilder<PmEntities>();
            opts.UseNpgsql(cfg.GetConnectionString("PmEntities")/*, 
                opt => opt.MigrationsAssembly(typeof(PmEntities).GetTypeInfo().Assembly.GetName().Name)*/);

            return new PmEntities(opts.Options);
        }
    }
}
