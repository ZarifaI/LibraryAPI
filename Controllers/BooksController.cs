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

            // 📦 Get the connection string from appsettings.json
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT title, genre, year FROM books", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                books.Add(new
                {
                    Title = reader.GetString(0),
                    Genre = reader.GetString(1),
                    Year = reader.GetInt32(2)
                });
            }

            return Ok(books);
        }

        }
    }

//C# → [Npgsql] → PostgreSQL → SQL runs → Data comes back → C# turns it into JSON → You see it in browser