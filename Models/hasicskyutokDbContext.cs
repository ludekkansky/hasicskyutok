using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace hasicskyutok.Models
{
    public partial class hasicskyutokDbContext : DbContext
    {
        public hasicskyutokDbContext(DbContextOptions<hasicskyutokDbContext> options) : base(options)
        {
            this.Database.Migrate();
        }


        public virtual DbSet<Kategorie> Kategorie { get; set; }
        public virtual DbSet<Druzstvo> Druzstva { get; set; }
    }
}
