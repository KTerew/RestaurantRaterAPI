using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterAPI.Models;

namespace RestaurantRaterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private RestaurantDbContext _context;
        public RestaurantController(RestaurantDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostRestaurant([FromForm] RestaurantEdit model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Restaurants.Add(new Restaurant()
            {
                Name = model.Name,
                Location = model.Location
            });

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetRestaurants()
        {
            var restaurants = await _context.Restaurants.Include(r=> r.Ratings).ToListAsync();
            
            List<RestaurantListItem> restaurantList = restaurants.Select(r=> new RestaurantListItem() {
                Id = r.Id,
                Name = r.Name,
                Location = r.Location,
                AverageRating = r.AverageRating
            }).ToList();
            
            return Ok(restaurantList);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            var restaurant = await _context.Restaurants.Include(r=> r.Ratings).FirstOrDefaultAsync(r=>r.Id==id);

            if(restaurant == null)
                return NotFound();
            
            return Ok(restaurant);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateRestaurant([FromForm] RestaurantEdit model, [FromForm] int id)
        {
            var oldRestaurant = await _context.Restaurants.FindAsync(id);

            if(oldRestaurant == null)
                return NotFound();
            
            if(!ModelState.IsValid)
                return BadRequest();

            if(!string.IsNullOrEmpty(model.Name))
            {
                oldRestaurant.Name = model.Name;
            }

            if(!string.IsNullOrEmpty(model.Location))
            {
                oldRestaurant.Location = model.Location;
            }

            await _context.SaveChangesAsync();
            return Ok("Successfully Updated!");
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if(restaurant == null)
            {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return Ok("Successfully Deleted!");
        }
    }
}