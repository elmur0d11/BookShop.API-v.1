using AutoMapper;
using BookShop.API.Dtos;
using BookShop.API.Models;
using BookShop.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rememorize.Dtos;
using rememorize.Models;
using rememorize.Service;
using rememorize.Service.Caching;
using rememorize.Service.ELS;

namespace BookShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IElasticService _elasticService;
        private readonly IBuyHistoryRepository _historyRepository;
        public UserController(
            IBookRepository repository, IMapper mapper,
            ICacheService cacheService, IElasticService elasticService,
            IBuyHistoryRepository historyRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
            _elasticService = elasticService;
            _historyRepository = historyRepository;
        }

        [HttpGet("GetAllBooks")]
        public async Task<ActionResult> GetBooks()
        {
            try
            {
                var cacheNames = _cacheService.GetData<IEnumerable<Book>>("books");

                if (cacheNames != null && cacheNames.Count() > 0)
                {
                    return Ok(_mapper.Map<IEnumerable<BookReadDto>>(cacheNames));
                }
                cacheNames = await _repository.GetAllBooks();
                var expiryTime = DateTimeOffset.Now.AddMinutes(5);
                _cacheService.SetData<IEnumerable<Book>>("books", cacheNames, expiryTime);

                return Ok(_mapper.Map<IEnumerable<BookReadDto>>(cacheNames));

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("FastSearch")]
        public async Task<ActionResult> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("So‘rov matni bo‘sh bo‘lmasligi kerak.");
            try
            {
                var results = await _elasticService.SearchAsync(keyword);

                return Ok(results.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("BuyByCode/{code}")]
        public async Task<ActionResult> BuyBook(string code)
        {
            var book = await _repository.BuyBook(code);

            if (book == null) return NotFound("Kitob topilmadi!");
            if (book.Quantity <= 0) return BadRequest("Kitob Qolmagan!");

            book.Quantity--;

            await _repository.UpdateBook(book);
            await _repository.SaveChangesAsync();
            await _elasticService.UpdateBook(book);
            _cacheService.RemoveData("books");

            await _historyRepository.UpsertBuyHistory(book.BookCode, book.BookName);
            _cacheService.RemoveData("buyHistory");

            return Ok($"Kitob sotib olindi! Qolgan soni: {book.Quantity}");
        }

    }
}
