
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Dapper;
using Recommendations_engine;

namespace Recommendations_engine
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GamesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<game_card>>> GetGames()
        {
            var games = await _context.Games
                .AsNoTracking()
                .Take(100)
                .Select(g => new game_card
                {
                    Appid = g.Appid,
                    Name = g.Name,
                    Release_date = g.Release_date,
                    Header_image = g.Header_image // Ensure property names match your database entity
                })
                .ToListAsync();

            return Ok(games);
        }

        [HttpGet("{appid:int}")]
        public async Task<ActionResult<game>> GetGame(int appid)
        {
            var game = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Appid == appid);

            if (game is null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get_Specific_Game(int id)
        {
            // 1. Grab the connection directly from your existing EF context
            using var connection = _context.Database.GetDbConnection();

            // 2. Write your pure SQL
            string sql = "SELECT appid, name FROM games_table WHERE appid = @Id";

            // 3. Run it with Dapper (using dynamic so you don't need a class)
            var game = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }
    }


}
