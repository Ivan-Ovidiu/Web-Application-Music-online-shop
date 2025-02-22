using System.ComponentModel.DataAnnotations;
namespace TargDeMuzica.Models
{
    public class Artist
    {
        [Key]
        public int ArtistID { get; set; }
        public String ArtistName { get; set; }
        public int ArtistAge { get; set; }
        //Relatie one to many intre Artist(1) -- (M)Product
        public virtual ICollection<Product> Products { get; set; }
    }
}
