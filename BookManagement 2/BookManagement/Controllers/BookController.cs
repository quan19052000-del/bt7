using Microsoft.AspNetCore.Mvc;
using BookManagement.Data;
using BookManagement.Models;

namespace BookManagement.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            var books = BookRepository.GetAll();
            return View(books);
        }

        public IActionResult Detail(int id)
        {
            var book = BookRepository.GetById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            BookRepository.Add(model);
            TempData["Message"] = "Thêm sách thành công";
            return RedirectToAction(nameof(Index));
        }
    }
}
