using System.ComponentModel.DataAnnotations;

namespace TargDeMuzica.Models
{
    public class IncomingRequest
    {
        [Key]
        public int RequestID { get; set; }

        public DateTime RequestDate { get; set; }
        public RequestStatus Status { get; set; }
        public string? AdminComment { get; set; }

        public virtual Product ProposedProduct { get; set; }
        public virtual ApplicationUser User { get; set; }


        public enum RequestStatus
        {
            Approved,
            Pending,
            Rejected
        }
    }
}
