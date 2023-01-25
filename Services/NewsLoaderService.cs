using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using TestTask.Args;
using TestTask.Data;
using TestTask.Models;

namespace TestTask.Services;

public class NewsLoaderService
{
    private readonly AppDbContext _db;
    public event EventHandler<RssFeedAddedEventArgs> RssFeedAdded; 

    public NewsLoaderService(AppDbContext db)
    {
        _db = db;
    }
    
    public async void HandleRssFeedAdded(object? sender, RssFeedAddedEventArgs e)
    {
        var newsList = await ParseNewsItems(e.RssFeed);
        _db.News.AddRange(newsList);
        await _db.SaveChangesAsync();
    }

    private static Task<List<News>> ParseNewsItems(RssFeed rssFeed)
    {
        var document = XDocument.Load(rssFeed.Url);
        var items = from item in document.Descendants("item")
            select new News
            {
                Title = item.Element("title").Value,
                Description = item.Element("description").Value,
                Link = item.Element("link").Value,
                PublishedDate = DateTime.Parse(item.Element("pubDate").Value),
                IsRead = false,
                RssFeed = rssFeed,
                RssFeedId = rssFeed.Id
            };
        return Task.FromResult(items.ToList());
    }
    
    public async Task RefreshNews()
    {
        var activeRssFeeds = await _db.RssFeeds.Where(x => x.IsActive).ToListAsync();
        var newsList = new List<News>();
        foreach (var rssFeed in activeRssFeeds)
        {
            newsList.AddRange(await ParseNewsItems(rssFeed));
        }

        var newNews = newsList.Where(x => x.PublishedDate >= x.RssFeed.LatestUpdate).ToList();

        _db.News.AddRange(newNews);
        await _db.SaveChangesAsync();
    }

    
    public void RaiseRssFeedAdded(RssFeed rssFeed)
    {
        OnRssFeedAdded(new RssFeedAddedEventArgs { RssFeed = rssFeed });
    }

    
    protected virtual void OnRssFeedAdded(RssFeedAddedEventArgs e)
    {
        RssFeedAdded?.Invoke(this, e);
    }
}