using System.ComponentModel.DataAnnotations;


namespace TargDeMuzica.Models
{
    public class Clients
    {
        [Key]
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CurrentRole { get; set; }
        public List<string> AllAvailableRoles { get; set; }
    }
}