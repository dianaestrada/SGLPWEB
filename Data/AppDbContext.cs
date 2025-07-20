using Microsoft.EntityFrameworkCore;
using SGLPWEB.Models;

namespace SGLPWEB.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Caso> Casos { get; set; }
        public DbSet<Plazo> Plazos { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Evidencia> Evidencias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✔️ Clave primaria explícita
            modelBuilder.Entity<Caso>()
                .HasKey(c => c.CasoId);

            // ✔️ Relaciones con hijos
            modelBuilder.Entity<Tarea>()
                .HasOne(t => t.Caso)
                .WithMany(c => c.Tareas)
                .HasForeignKey(t => t.CasoId);

            modelBuilder.Entity<Plazo>()
                .HasOne(p => p.Caso)
                .WithMany(c => c.Plazos)
                .HasForeignKey(p => p.CasoId);

            modelBuilder.Entity<Evidencia>()
                .HasOne(e => e.Caso)
                .WithMany(c => c.Evidencias)
                .HasForeignKey(e => e.CasoId);

            // ✔️ Relación Cliente ↔ Caso
            modelBuilder.Entity<Caso>()
                .HasOne(c => c.Cliente)
                .WithMany(cl => cl.Casos)
                .HasForeignKey(c => c.ClienteId);
        }
    }
}