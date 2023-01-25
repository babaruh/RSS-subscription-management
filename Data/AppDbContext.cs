using Microsoft.EntityFrameworkCore;
using TestTask.Models;

namespace TestTask.Data;

public class AppDbContext : DbContext
{
    public DbSet<RssFeed> RssFeeds { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("DataSource=Data/TestTask.db");
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasKey(n => n.Id);

        builder.Entity<RssFeed>().HasKey(n => n.Id);
        builder.Entity<RssFeed>()
            .HasMany(x => x.News)
            .WithOne(x => x.RssFeed)
            .HasForeignKey(x => x.RssFeedId);
        
        builder.Entity<News>().HasKey(n => n.Id);
        builder.Entity<News>()
            .HasOne(x => x.RssFeed)
            .WithMany(x => x.News)
            .HasForeignKey(x => x.RssFeedId);
        
        base.OnModelCreating(builder);
    }
}