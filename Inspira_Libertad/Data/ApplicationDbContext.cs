using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Inspira_Libertad.Models;

namespace Inspira_Libertad.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Mensaje> Mensajes { get; set; }
        public DbSet<CarritoItem> CarritoItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Frase> Frases { get; set; }
        public DbSet<Articulo> Articulos { get; set; }

    }
}
