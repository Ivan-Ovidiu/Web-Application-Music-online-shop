using System.ComponentModel.DataAnnotations;

namespace TargDeMuzica.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        public int TotalPrice { get; set; }
        //vectorul de produse din cos ( produsele din tabelul Products) Many to many intre Cart si Product
        public virtual ICollection<Product> Products { get; set; }

       public virtual ApplicationUser User { get; set; }
    }
}
