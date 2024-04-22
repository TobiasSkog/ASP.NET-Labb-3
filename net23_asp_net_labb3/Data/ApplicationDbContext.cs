using Microsoft.EntityFrameworkCore;
using net23_asp_net_labb3.Models;

namespace net23_asp_net_labb3.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PeopleInterest>().HasKey(pi => new { pi.InterestId, pi.PeopleId });

        // This line is used if using identity and if having a overrided OnModelCreating
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<People> People { get; set; }
    public DbSet<Interest> Interests { get; set; }
    public DbSet<Link> Links { get; set; }
    public DbSet<PeopleInterest> PeopleInterests { get; set; }
}
