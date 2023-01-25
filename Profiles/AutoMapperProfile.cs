using AutoMapper;
using TestTask.Dtos;
using TestTask.Models;

namespace TestTask.Profiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RssFeedDto, RssFeed>();
        CreateMap<RssFeed, RssFeedDto>();
        CreateMap<NewsDto, News>();
        CreateMap<News, NewsDto>();
    }
}