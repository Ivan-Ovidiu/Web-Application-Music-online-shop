using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TargDeMuzica.Models;

namespace TargDeMuzica.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
        public DbSet<Artist> Artists { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<IncomingRequest> IncomingRequests { get; set; }
		public DbSet<MusicSuport> MusicSuports { get; set; }
		public DbSet<Product> Products { get; set; }

        public DbSet<Clients> Clients { get; set; }
        public DbSet<Review> Reviews { get; set; }
    
    }
}
