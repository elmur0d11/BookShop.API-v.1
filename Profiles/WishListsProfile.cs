using AutoMapper;
using BookShop.API.Dtos;
using BookShop.API.Models;

namespace BookShop.API.Profiles
{
    public class WishListsProfile : Profile
    {
        public WishListsProfile()
        {
            CreateMap<WishListCreatedDto, WishList>();
            CreateMap<WishList, WishListReadDto>();
        }
    }
}
