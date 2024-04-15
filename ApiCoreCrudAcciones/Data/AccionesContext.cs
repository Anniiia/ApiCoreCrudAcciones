using ApiCoreCrudAcciones.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCoreCrudAcciones.Data
{
    public class AccionesContext : DbContext
    {
        public AccionesContext(DbContextOptions<AccionesContext> options) : base(options)
        { }

        public DbSet<Accion> Acciones { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
