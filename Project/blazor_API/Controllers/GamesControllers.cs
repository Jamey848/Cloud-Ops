
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Recommendation_engine;
using Recommendations_engine;
using System.Linq;

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
            .AsSplitQuery()
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

            var dto = await _context.Games
            .AsNoTracking()
            .Where(g => g.Appid == appId)
            .Select(game => new DetailedGameDto
            {
                AppId = game.Appid,
                Name = game.Name,
                ReleaseDate = game.Release_date,
                RequiredAge = game.Required_age,
                DlcCount = game.Dlc_count,
                ShortDescription = game.Description,
                HeaderImage = game.Header_image,

                Windows = game.Windows,
                Mac = game.Mac,
                Linux = game.Linux,

                Achievements = game.Achievements,
                Recommendations = game.Recommendations,
                Notes = game.Notes,
                Developers = game.Developers,
                Publishers = game.Publishers,
                CompositeScore = game.Score,

                Genres = game.GameGenres
                    .Where(x => x.Linked_genre != null)
                    .Select(x => x.Linked_genre.GenreName!)
                    .ToList(),

                Tags = game.GameTags
                    .Where(x => x.Linked_tag != null)
                    .Select(x => x.Linked_tag.TagName!)
                    .ToList(),

                Categories = game.GameCategories
                    .Where(x => x.Linked_category != null)
                    .Select(x => x.Linked_category.CategorieName!)
                    .ToList(),

                Screenshots = game.Screenshots
                    .Where(x => x.ScreenshotUrl != null)
                    .Select(x => x.ScreenshotUrl!)
                    .ToList(),

                PopDTO = game.Popularity == null ? null : new Game_popularity
                {
                    Url = game.Popularity.Url,
                    PlayerPeak = game.Popularity.PlayersAllTimePeak,
                    Reviews = game.Popularity.ReviewsTotal,
                    Pos_percent = game.Popularity.ReviewsPositivePercent,
                    Summary = game.Popularity.ReviewsSummary
                },

                PriceDTO = game.Price == null ? null : new Game_price
                {
                    InitialPrice = game.Price.InitialPrice,
                    FinalPrice = game.Price.FinalPrice,
                    Discount = game.Price.Discount,
                    UpdatedAt = game.Price.UpdatedAt
                }
            })
            .FirstOrDefaultAsync();

            return Ok(dto);
        }

        [HttpPost("Filter")]
        public async Task<IActionResult> GetMetadata(
        [FromBody] Expl_FilterRequest request)
        {
            var query = _context.Games
                .AsNoTracking()
                .AsQueryable();

            // Search by name
            if (!string.IsNullOrWhiteSpace(request.Searchname))
            {
                query = query.Where(g =>
                    g.Name.Contains(request.Searchname));
            }

            // Filter by AppID
            if (request.AppID.HasValue)
            {
                query = query.Where(g =>
                    g.Appid == request.AppID.Value);
            }

            // Filter by genres
            if (request.GenreIds.Count > 0)
            {
                query = query.Where(g =>
                    g.GameGenres.Any(gg =>
                        gg.GenreId != null &&
                        request.GenreIds.Contains(gg.GenreId)));
            }

            // Filter by categories
            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(g =>
                    g.GameCategories.Any(gc =>
                        gc.CategorieId != null &&
                        request.CategoryIds.Contains(gc.CategorieId)));
            }

            // Filter by tags
            if (request.TagIds.Count > 0)
            {
                query = query.Where(g =>
                    g.GameTags.Any(gt =>
                        gt.TagId.HasValue &&
                        request.TagIds.Contains(gt.TagId.Value)));
            }

            // Filter by minimum price
            if (request.MinPrice.HasValue)
            {
                query = query.Where(g =>
                    g.Price != null &&
                    g.Price.FinalPrice >= request.MinPrice.Value);
            }

            var games = await query
                .Include(g => g.GameGenres)
                .Include(g => g.GameTags)
                .Include(g => g.GameCategories)
                .Include(g => g.Popularity)
                .Include(g => g.Price)
                .AsSplitQuery()
                .ToListAsync();

            List<game_card> gc_list = games.Select(g => new game_card
            {
                Appid = g.Appid,
                Name = g.Name,
                Release_date = g.Release_date,
                Header_image = g.Header_image
            }).Take(1000).ToList();

            return Ok(gc_list);
        }
    }
}
