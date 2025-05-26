using Microsoft.AspNetCore.Mvc;
using Npgsql;


[ApiController]
[Route("[controller]")] // This tells .NET to use the UsersController class
public class LoansController : ControllerBase
{

    private readonly IConfiguration _configuration;

    public LoansController(IConfiguration configuration)
    {
        _configuration = configuration;
    } // 🔑 This lets the controller read values from appsettings.json — like your PostgreSQL connection string.
    [HttpPost]
    public IActionResult BorrowBook([FromBody] LoanDto loan)
    {
        var connStr = _configuration.GetConnectionString("DefaultConnection");
        using var conn = new NpgsqlConnection(connStr);
        conn.Open();
        using var cmd = new NpgsqlCommand(
            "INSERT INTO loans (user_id, book_id, borrow_date, return_date) VALUES (@user, @book, @borrow, @return)", conn);

        cmd.Parameters.AddWithValue("user", loan.UserId);
        cmd.Parameters.AddWithValue("book", loan.BookId);
        cmd.Parameters.AddWithValue("borrow", loan.BorrowDate);

        // optional: return date might be null
        cmd.Parameters.AddWithValue("return", loan.ReturnDate ?? (object)DBNull.Value);
        return Ok("Book borrowed successfully!");

    }
        public class LoanDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; } // Optional
    }
}