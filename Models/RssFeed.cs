using System.ComponentModel.DataAnnotations;

namespace TestTask.Models;

public class RssFeed
{
    public RssFeed()
    {
        Id = Guid.NewGuid();
    }

    [Key]
    public Guid Id { get; }
    public string Url { get; set; }
    public string Feed { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LatestUpdate { get; set; }

    
    //Relationships
    public ICollection<News> News { get; set; }
}