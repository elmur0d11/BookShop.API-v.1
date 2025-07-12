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
    public class AdminController : ControllerBase
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IElasticService _elasticService;
        private readonly IBuyHistoryRepository _historyRepository;
        public AdminController(IBookRepository repository, IMapper mapper, ICacheService cacheService, IElasticService elasticService, IBuyHistoryRepository historyRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
            _elasticService = elasticService;
            _historyRepository = historyRepository;
        }

        [HttpPost("PostBook")]
        public async Task<ActionResult> AddBook(BookCreatedDto bookCreatedDto)
        {
            var bookModel = _mapper.Map<Book>(bookCreatedDto);

            bookModel.BookCode = GenerateBookCode();

            await _repository.AddBook(bookModel);
            await _repository.SaveChangesAsync();
            //ELS
            try
            {
                await _elasticService.IndexBook(bookModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ELS Exeption: " + ex.Message);
            }
            //ELS
            var bookReadDto = _mapper.Map<BookReadDto>(bookModel);

            _cacheService.RemoveData("books");

            return Created("", bookModel);
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

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var bookModelFromRepo = await _repository.GetBook(id);

            if (bookModelFromRepo == null)
                return NotFound();

            _cacheService.RemoveData("books");

            _repository.DeleteBook(bookModelFromRepo);

            await _repository.SaveChangesAsync();

            try
            {
                await _elasticService.DeleteBook(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ELS Exeption: " + ex.Message);
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, BookUpdateDto bookUpdateDto)
        {
            var bookModelFromRepo = await _repository.GetBook(id);

            if (bookModelFromRepo == null)
                return NotFound();

            _mapper.Map(bookUpdateDto, bookModelFromRepo);

            await _repository.UpdateBook(bookModelFromRepo);
            await _repository.SaveChangesAsync();

            _cacheService.RemoveData("books");

            try
            {
                await _elasticService.UpdateBook(bookModelFromRepo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ELS update failed: " + ex.Message);
            }

            return NoContent();
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

        [HttpGet("GetBuyHistory")]
        public async Task<IActionResult> GetBuyHistory()
        {
            var historyCache = _cacheService.GetData<IEnumerable<BuyHistory>>("buyHistory");

            if (historyCache != null && historyCache.Count() > 0)
            {
                return Ok(_mapper.Map<IEnumerable<BuyHistoryReadDto>>(historyCache));
            }

            var history = await _historyRepository.GetAll();
            var expiryTime = DateTimeOffset.Now.AddMinutes(5);

            _cacheService.SetData("buyHistory", history, expiryTime);

            return Ok(_mapper.Map<IEnumerable<BuyHistoryReadDto>>(history));
        }

        private string GenerateBookCode()
        {
            return $"EL-{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}";
        }
    }
}
