using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using rememorize.Dtos;
using rememorize.Models;
using rememorize.Service;
using rememorize.Service.Caching;
using rememorize.Service.ELS;

namespace rememorize.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IElasticService _elasticService;
        public BookController(IBookRepository repository, IMapper mapper, ICacheService cacheService, IElasticService elasticService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
            _elasticService = elasticService;
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
                    Console.WriteLine("CACHE WORKED");
                    return Ok(_mapper.Map<IEnumerable<BookReadDto>>(cacheNames));
                }
                Console.WriteLine("CACHED");
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
            }catch (Exception ex)
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

            return Ok($"Kitob sotib olindi! Qolgan soni: {book.Quantity}");
        }

        private string GenerateBookCode()
        {
            return $"EL-{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}";
        }

    }
}
