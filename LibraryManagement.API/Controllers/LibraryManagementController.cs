using LibraryManagement.Domain.DTO;
using LibraryManagement.Domain.Interfaces.BookBorrower;
using LibraryManagement.Domain.Interfaces.BookManager;
using LibraryManagement.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryManagementController : ControllerBase
    {
        private readonly IBookManagementService _bookManagementService;
        private readonly IBookBorrowerService _bookBorrowerService;
        private readonly IConfiguration _configuration;
        public LibraryManagementController(IBookManagementService bookManagementService, IBookBorrowerService bookBorrowerService, IConfiguration configuration)
        {
            _bookManagementService = bookManagementService;
            _bookBorrowerService = bookBorrowerService;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetBooks")]
        public IActionResult GetBooks()
        {
            List<BookDto> books = new List<BookDto>();
            try
            {
                var bookstore = _bookManagementService.GetBookStores();
                foreach(var bs in bookstore)
                {
                    var book = _bookManagementService.GetBook(bs.BookId);
                    var author = _bookManagementService.GetAuthor(book.AuthorId);
                    var publisher = _bookManagementService.GetPublisher(book.PublisherId);
                    BookDto bookDto = new BookDto { Id = book.Id, Name = book.Name, Quantity = bs.Quantity, Author=$"{author.FirstName} {author.LastName}" , Publisher = publisher.Name, PublishedDate = book.PublishDate};
                    books.Add(bookDto);
                }

                return Ok(new Response() { Code = "00", Description = "Success", Data = books });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new Response() { Code = "99", Description = "System error", Data=""});
            }
        }

        [HttpPost]
        [Route("BorrowBook")]
        public IActionResult BorrowBook([FromBody]BorrowBookRequestDto borrowBookRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                try
                {
                    var book = _bookBorrowerService.BorrowBook(borrowBookRequestDto.BookId, borrowBookRequestDto.BorrowerId, Int32.Parse(_configuration["quantity"]), Int32.Parse(_configuration["limit"]));
                    return Ok(new Response() { Code = "00", Description = "Success", Data = true });
                }
                catch(KeyNotFoundException ex)
                {
                    return StatusCode(422, new Response() { Code = "01", Description = ex.Message, Data = false });
                }
                catch(ArgumentOutOfRangeException ex)
                {
                    return StatusCode(422, new Response() { Code = "02", Description = ex.Message, Data = false });
                }
                catch (InvalidOperationException ex)
                {
                    return StatusCode(422, new Response() { Code = "03", Description = ex.Message, Data = false });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response() { Code = "99", Description = "System error", Data = "" });
            }

        }


        [HttpPost]
        [Route("ReturnBook")]
        public IActionResult ReturnBook([FromBody] BorrowBookRequestDto borrowBookRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                try
                {
                    var response = _bookBorrowerService.ReturnBook(borrowBookRequestDto.BookId, borrowBookRequestDto.BorrowerId, Int32.Parse(_configuration["quantity"]));
                    if(response) return Ok(new Response() { Code = "00", Description = "Success", Data = true });
                    return StatusCode(422, new Response() { Code = "01", Description = "Failure", Data = false });
                }
                catch (KeyNotFoundException ex)
                {
                    return StatusCode(422, new Response() { Code = "01", Description = ex.Message, Data = false });
                }          
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response() { Code = "99", Description = "System error", Data = "" });
            }
        }

        [HttpPost]
        [Route("CreateBorrower")]
        public IActionResult CreateBorrower([FromBody] BorrowerDto borrowerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                try
                {
                    var response = _bookBorrowerService.CreateBorrower(borrowerDto.FirstName, borrowerDto.LastName, borrowerDto.Email, borrowerDto.Password);
                    return Ok(new Response() { Code = "00", Description = "Success", Data = new { BorrowerId = response } });                   
                }
                catch (KeyNotFoundException ex)
                {
                    return StatusCode(422, new Response() { Code = "01", Description = ex.Message, Data = false });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response() { Code = "99", Description = "System error", Data = "" });
            }
        }

        [HttpGet]
        [Route("GetBorrowerById/{id}")]
        public IActionResult GetBorrowerById(int id)
        {
            if(id.GetType() != typeof(int))
            {
                return BadRequest("integer value for id expected");
            }

            try
            {
                var response = _bookBorrowerService.GetBorrower(id);
                if (response == null) return NotFound();
                return Ok(new Response() { Code = "00", Description = "Success", Data = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response() { Code = "99", Description = "System error", Data = "" });
            }
        }

        [HttpGet]
        [Route("GetBorrowedBooks/{id}")]
        public IActionResult GetBorrowedBooks(int id)
        {
            if (id.GetType() != typeof(int))
            {
                return BadRequest("integer value for id expected");
            }

            List<BookDto> books = new List<BookDto>();
            try
            {
                var borrorwedBooks = _bookBorrowerService.GetBooksBorrowedByBorrower(id);
                foreach (var bs in borrorwedBooks)
                {
                    var book = _bookManagementService.GetBook(bs.BookId);
                    var author = _bookManagementService.GetAuthor(book.AuthorId);
                    var publisher = _bookManagementService.GetPublisher(book.PublisherId);
                    BookDto bookDto = new BookDto { Id = book.Id, Name = book.Name, Quantity = 1, Author = $"{author.FirstName} {author.LastName}", Publisher = publisher.Name, PublishedDate = book.PublishDate };
                    books.Add(bookDto);
                }

                return Ok(new Response() { Code = "00", Description = "Success", Data = books });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response() { Code = "99", Description = "System error", Data = "" });
            }
        }

        [HttpPost]
        [Route("BorrowerLogIn")]
        public IActionResult BorrowerLogIn([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var response = _bookBorrowerService.Login(loginDto.Email, loginDto.Password);
                if (response == 0) return NotFound();
                return Ok(new Response() { Code = "00", Description = "Success", Data = new { BorrowerId = response } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response() { Code = "99", Description = "System error", Data = "" });
            }
        }
    }
}
