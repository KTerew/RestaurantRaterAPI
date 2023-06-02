using Microsoft.EntityFrameworkCore;

namespace RestaurantRaterAPI
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options) {}
    }
}