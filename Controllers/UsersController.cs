using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET /users
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = new List<object>();
            var connStr = _configuration.GetConnectionString("DefaultConnection");

            using var conn = new NpgsqlConnection(connStr);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT user_id, first_name, last_name, email FROM users", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3)
                });
            }

            return Ok(users);
        }

        // POST /users
        [HttpPost]
        public IActionResult AddUser([FromBody] UserDto user)
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");

            using var conn = new NpgsqlConnection(connStr);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO users (first_name, last_name, email) VALUES (@first, @last, @email)", conn);
            cmd.Parameters.AddWithValue("first", user.FirstName);
            cmd.Parameters.AddWithValue("last", user.LastName);
            cmd.Parameters.AddWithValue("email", user.Email);

            cmd.ExecuteNonQuery();

            return Ok("User added successfully.");
        }

        public class UserDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }
    }
}