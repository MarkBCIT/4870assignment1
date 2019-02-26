using assignment.ModelViews;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext:IdentityDbContext<ApplicationUser> {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    {    }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        #region "Seed Data"

        builder.Entity<IdentityRole>().HasData(
            new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new { Id = "2", Name = "Member", NormalizedName = "MEMBER" }
        );





        #endregion
    }
    public DbSet<Boat> Boats { get; set; }
}
