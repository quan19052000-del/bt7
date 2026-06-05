using BookManagement.Models;
namespace BookManagement.Data
{
    public static class BookRepository
    {
        private static List<Book> _books = new()
        {
            new Book { Id = 1, Name = "Clean Code", Price = 20 },
            new Book { Id = 2, Name = "ASP.NET MVC", Price = 15 },
            new Book { Id = 3, Name = "Design Pattern", Price = 25 },
        };

        public static IEnumerable<Book> GetAll() => _books;

        public static Book? GetById(int id) => _books.FirstOrDefault(b => b.Id == id);

        public static void Add(Book book)
        {
            var nextId = _books.Any() ? _books.Max(b => b.Id) + 1 : 1;
            book.Id = nextId;
            _books.Add(book);
        }
    }
}
