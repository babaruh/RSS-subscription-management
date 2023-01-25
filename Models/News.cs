using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Models;

public class News
{
    public News()
    {
        Id = Guid.NewGuid();
    }
    
    [Key]
    public Guid Id { get; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? Link { get; set; }
    public bool IsRead { get; set; }
    public DateTime PublishedDate { get; set; }
    
    //Relationships
    [ForeignKey("RssFeedId")]
    public Guid RssFeedId { get; set; }
    public RssFeed RssFeed { get; set; }
}