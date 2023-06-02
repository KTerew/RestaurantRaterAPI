using Microsoft.AspNetCore.Mvc;
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

            return Ok();
        }
    }
}