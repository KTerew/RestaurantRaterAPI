using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterAPI.Models;

namespace RestaurantRaterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly ILogger<RatingController> _logger;
        private RestaurantDbContext _context;

        public RatingController(ILogger<RatingController> logger, RestaurantDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RateRestaurant([FromForm] RatingEdit model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Ratings.Add(new Rating() {
                Score = model.Score,
                RestaurantId = model.RestaurantId
            });

            await _context.SaveChangesAsync();

            return Ok("Successfully Added!");
        }

        [HttpGet]
        public async Task<IActionResult> GetRatings()
        {
            var ratings = await _context.Ratings.Select(r=> new RatingListItem
            {
                Id = r.Id,
                RestaurantId = r.RestaurantId
            }).ToListAsync();

            return Ok(ratings);
        }
    }
}