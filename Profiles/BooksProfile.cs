using AutoMapper;
using rememorize.Dtos;
using rememorize.Models;

namespace rememorize.Profiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<BookCreatedDto, Book>();
            CreateMap<Book, BookReadDto>();
            CreateMap<BookUpdateDto, Book>();
        }
    }
}
