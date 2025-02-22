using System.ComponentModel.DataAnnotations;
namespace TargDeMuzica.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }
        
        public string? ReviewContent { get; set; }
        [Required(ErrorMessage = "Numele de utilizator este obligatoriu!")]

        public string ReviewerName { get; set; }
        public int? ProductId { get; set; }
        public DateTime ReviewDate { get; set; }

        [Required(ErrorMessage = "Rating-ul este obligatoriu!")]
        [Range(1, 5, ErrorMessage = "Rating-ul este intre 1-5!")]
        public int StarRating { get; set; }

        public virtual Product? Product { get; set; }

        //foreign key user
        public virtual ApplicationUser? User { get; set; }

    }
}
