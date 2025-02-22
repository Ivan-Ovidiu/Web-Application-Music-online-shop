using System.ComponentModel.DataAnnotations;

namespace TargDeMuzica.Models
{
    public class MusicSuport
    {
        [Key]
        public int MusicSuportID { get; set; }
        public string MusicSuportName { get; set;}
        public virtual ICollection<Product>? Products { get; set; }
    }
}
