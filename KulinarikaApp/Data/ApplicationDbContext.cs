using KulinarikaApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KulinarikaApp.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Title> Titles { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Recipe>().ToTable("Recipe");
        modelBuilder.Entity<Title>().ToTable("Title");
        modelBuilder.Entity<Bookmark>().ToTable("Bookmark");
        modelBuilder.Entity<Comment>().ToTable("Comment");

    }

}