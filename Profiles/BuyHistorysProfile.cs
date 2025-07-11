using AutoMapper;
using BookShop.API.Dtos;
using BookShop.API.Models;

namespace BookShop.API.Profiles
{
    public class BuyHistorysProfile : Profile
    {
        public BuyHistorysProfile()
        {
            CreateMap<BuyHistoryCreatedDto, BuyHistory>();
            CreateMap<BuyHistory, BuyHistoryReadDto>();
        }
    }
}
