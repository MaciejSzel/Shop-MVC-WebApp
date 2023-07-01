using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sklep.Data.Models.CMS;

namespace Sklep.Data.Models
{
    public class SklepDbContext : DbContext
    {
        public SklepDbContext(DbContextOptions<SklepDbContext> options) : base(options)
        {

        }

        public DbSet<Strona> Strona { get; set; }
        public DbSet<Aktualnosc> Aktualnosc { get; set; }
        public DbSet<Rodzaj> Rodzaj { get; set; }
        public DbSet<Podkategoria> Podkategoria { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ElementKoszyka> ElementKoszyka { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images) // Określenie relacji jeden do wielu między Product a Image
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);

            modelBuilder.Entity<Product>()
        .HasOne(p => p.Rodzaj)
        .WithMany(r => r.Produkty)
        .HasForeignKey(p => p.RodzajId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Podkategoria)
                .WithMany(pk => pk.Produkty)
                .HasForeignKey(p => p.PodkategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
            => services.AddDbContext<SklepDbContext>();

    }
}
