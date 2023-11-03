using FreeCourse.IdentityServerJWT.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreeCourse.IdentityServerJWT.Data
{
    public class AppDbContext: IdentityDbContext<UserApp, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
            optionsBuilder.UseSqlServer(connectionString: @"Server=localhost,1433;Database=IdentityDb2;User=sa;Password=klmn1234;TrustServerCertificate=True");
        }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            builder.Entity<UserRefreshToken>()
            .HasKey(x => x.UserId);
           


            base.OnModelCreating(builder);
        }
    }
}
