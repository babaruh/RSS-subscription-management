using System.ServiceModel.Syndication;
using System.Xml;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Dtos;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.Controllers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RssFeedController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;
    private readonly NewsLoaderService _newsLoaderService;

    public RssFeedController(AppDbContext db, IMapper mapper, NewsLoaderService newsLoaderService)
    {
        _db = db;
        _mapper = mapper;
        _newsLoaderService = newsLoaderService;
        _newsLoaderService.RssFeedAdded += _newsLoaderService.HandleRssFeedAdded;
    }
    
    // 1. Add RSS feed (parameters: feed url)
    [HttpPost("rssFeed")]
    public async Task<IActionResult> AddRssFeed(RssFeedDto rssFeedDto)
    {
        var isActive = IsActiveRssFeed(rssFeedDto.Url);

        var rssFeed = _mapper.Map<RssFeed>(rssFeedDto);
        rssFeed.IsActive = isActive;
        rssFeed.LatestUpdate = DateTime.Now;
        
        _db.RssFeeds.Add(rssFeed);
        await _db.SaveChangesAsync();
        _newsLoaderService.RaiseRssFeedAdded(rssFeed);
        
        return Ok();
    }
    
    // 2. Get all active RSS feeds
    [HttpGet("activeRssFeeds")]
    public async Task<IActionResult> GetActiveRssFeeds()
    {
        var activeRssFeeds = await _db.RssFeeds.Where(x => x.IsActive).ToListAsync();

        var activeRssFeedsDto = _mapper.Map<List<RssFeedDto>>(activeRssFeeds);

        return Ok(activeRssFeedsDto);
    }

    // 3. Get all unread news from some date (parameters: date)
    [HttpGet("unreadNews")]
    public async Task<IActionResult> GetUnreadNewsFromDate(DateTime date)
    {
        var unreadNews = await _db.News.Where(x => x.PublishedDate >= date && !x.IsRead).ToListAsync();

        var unreadNewsDto = _mapper.Map<List<NewsDto>>(unreadNews);
        return Ok(unreadNewsDto);
    }

    // 4. Set news as read
    [HttpPut("{id:guid}/read")]
    public async Task<IActionResult> SetNewsAsRead(Guid id)
    {
        var newsItem = await _db.News.FirstOrDefaultAsync(x => x.Id == id);
        if (newsItem is null) return NotFound();
        newsItem.IsRead = true;
        
        await _db.SaveChangesAsync();
        return Ok();
    }
    
    private static bool IsActiveRssFeed(string url)
    {
        try {
            using var reader = XmlReader.Create(url);
            SyndicationFeed.Load(reader);
            return true;
        }
        catch (Exception) {
            return false;
        }
    }
}
