
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Recommendation_engine;
using Recommendations_engine;

namespace Recommendations_engine
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly AppDbContext _context; // Empty. But every class still needs to access this.

        public GamesController(AppDbContext context)
        {
            _context = context; // Fill the empty value with the one that is filled. Context is appdbcontext class that is now created with ctor.
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<game_card>>> GetGames()
        // IEnumerable = Like a list, only it doesn't care HOW your data was stored. Could be from a list, array, whatever.
        // ActionResult = Means returning either http response (400, 404, ...) or the actual data.
        // Task = Make it asynchronous
        {
            var games = await _context.Games
                .AsNoTracking() // Don't bother tracking any changes, because we are just retrieving their information.
                .Take(100)
                .Select(g => new game_card
                {
                    Appid = g.Appid,
                    Name = g.Name,
                    Release_date = g.Release_date,
                    Header_image = g.Header_image // Ensure property names match your database entity
                })
                .ToListAsync(); // = Execute the query and return to list.

            return Ok(games); // Return data if 200, otherwise return request response.
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
    
        [HttpGet("details/{appId:long}")]
        public async Task<ActionResult<DetailedGameDto>> GetGame(long appId)
        {
            var game = await _context.Games
            .AsNoTracking()
            .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Linked_genre)
            .Include(g => g.GameTags)
                .ThenInclude(gt => gt.Linked_tag)
            .Include(g => g.GameCategories)
                .ThenInclude(gc => gc.Linked_category)
            .Include(g => g.Popularity)
            .Include(g => g.Price)
            .Include(g => g.Screenshots)
            .FirstOrDefaultAsync(g => g.Appid == appId);

            if (game == null)
                return NotFound();

            var dto = new DetailedGameDto
            {
                // Basic information
                AppId = game.Appid,
                Name = game.Name,
                ReleaseDate = game.Release_date,
                RequiredAge = game.Required_age,
                DlcCount = game.Dlc_count,
                ShortDescription = game.Description,
                HeaderImage = game.Header_image,

                // Platform support
                Windows = game.Windows,
                Mac = game.Mac,
                Linux = game.Linux,

                // Additional information
                Achievements = game.Achievements,
                Recommendations = game.Recommendations,
                Notes = game.Notes,
                Developers = game.Developers,
                Publishers = game.Publishers,
                CompositeScore = game.Score,

                // Genres
                Genres = game.GameGenres
                .Where(x => x.Linked_genre != null)
                .Select(x => x.Linked_genre.GenreName!)
                .ToList(),

                // Tags
                Tags = game.GameTags
                .Where(x => x.Linked_tag != null)
                .Select(x => x.Linked_tag.TagName!)
                .ToList(),

                // Categories
                Categories = game.GameCategories
                .Where(x => x.Linked_category != null)
                .Select(x => x.Linked_category.CategorieName!)
                .ToList(),

                // Screenshots
                Screenshots = game.Screenshots
                .Where(x => x.ScreenshotUrl != null)
                .Select(x => x.ScreenshotUrl!)
                .ToList(),

                // Popularity
                /*Popularity = game.Popularity,

                // Price
                Prices = game.Price*/
            };

            return Ok(dto);
        }

    }
}
