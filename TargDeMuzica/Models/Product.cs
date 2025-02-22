using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TargDeMuzica.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        
        public float ProductPrice { get; set; }
        public int ProductStock { get; set; }
        public string? ProductImageLocation {  get; set; }
        public float? ProductScore { get; set; }

        public string? ProductGenresTemp { get; set; }
        public List<string>? ProductGenres { get; set; }


        //foreign key MusicSuport == categorie
        public int? MusicSuportID { get; set; }
        public virtual MusicSuport? MusicSuport { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? MusicSup { get; set; }
        //foreign key Artist
        public int? ArtistID { get; set; }
        public virtual Artist? Artist { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? ArtistList { get; set; }
        //many to many products - carts
        public virtual ICollection<Cart>? Carts { get; set; }
        //vector review
        public ICollection<Review>? Reviews { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
