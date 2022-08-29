using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hasicskyutok.Models
{
    public partial class hasicskyutokDbContext : IdentityDbContext
    {
        public hasicskyutokDbContext(DbContextOptions<hasicskyutokDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Druzstvo>()
                .HasIndex(u => u.StartovniCislo)
                .IsUnique();

            builder.Entity<Vysledek>()
            .HasIndex(u => u.DruzstvoID)
            .IsUnique();
        }

        public virtual DbSet<Kategorie> Kategorie { get; set; }
        public virtual DbSet<Druzstvo> Druzstva { get; set; }
        public virtual DbSet<Vysledek> Vysledky { get; set; }
    }
}
