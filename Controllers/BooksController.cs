using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BooksController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = new List<object>();

            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT book_id, title, genre, year FROM books", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                books.Add(new
                {
                    BookId = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Genre = reader.GetString(2),
                    Year = reader.GetInt32(3)
                });
            }

            return Ok(books);
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] BookDto book)
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new NpgsqlConnection(connStr);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO books (title, genre, year) VALUES (@title, @genre, @year)", conn);
            cmd.Parameters.AddWithValue("title", book.Title);
            cmd.Parameters.AddWithValue("genre", book.Genre);
            cmd.Parameters.AddWithValue("year", book.Year);

            cmd.ExecuteNonQuery();
            return Ok("Book added successfully.");
        }

        public class BookDto
        {
            public string Title { get; set; }
            public string Genre { get; set; }
            public int Year { get; set; }
        }
    }
}

//C# → [Npgsql] → PostgreSQL → SQL runs → Data comes back → C# turns it into JSON → You see it in browser