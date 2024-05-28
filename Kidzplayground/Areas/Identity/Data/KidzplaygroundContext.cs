using Kidzplayground.Areas.Identity.Data;
using Kidzplayground.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kidzplayground.Data;

public class KidzplaygroundContext : IdentityDbContext<KidzplaygroundUser>
{
    public KidzplaygroundContext(DbContextOptions<KidzplaygroundContext> options)
        : base(options)
    {
    }


    public DbSet<Kidzplayground.Models.Comment> Comment { get; set; } = default!;
    public DbSet<Kidzplayground.Models.Post> Post { get; set; } = default!;
    public DbSet<Kidzplayground.Models.Category> Categories { get; set; }
    public DbSet<Message> Messages { get; set; }
    



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
