using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HasicskyUtok.Models
{
    public partial class HasicskyUtokDbContext : IdentityDbContext
    {
        public HasicskyUtokDbContext(DbContextOptions<HasicskyUtokDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Druzstvo>().HasIndex(u => u.StartovniCislo).IsUnique();
            builder.Entity<VysledekUtok>().HasIndex(u => u.DruzstvoID).IsUnique();
        }

        public virtual DbSet<Kategorie> Kategorie { get; set; }
        public virtual DbSet<Stafeta> Stafeta { get; set; }
        public virtual DbSet<Druzstvo> Druzstva { get; set; }
        public virtual DbSet<VysledekUtok> VysledkyUtok { get; set; }
        public virtual DbSet<VysledekStafeta> VysledkyStafeta { get; set; }
    }
}
