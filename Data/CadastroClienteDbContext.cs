using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CadastroClienteService.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroClienteService.Data
{
    public class CadastroClienteDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<Cliente> Clientes { get; set; }

        public CadastroClienteDbContext(IConfiguration configuration) => _configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.EnableSensitiveDataLogging();
            options.UseNpgsql(_configuration.GetConnectionString("Postgres"));
        }
    }
}