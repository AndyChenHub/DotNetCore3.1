using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    public class Manager
    {
        [Key]
        public int ManagerId { get; set; }

        [Required]
        public Position Position { get; set; }

    // Navigation property for Clients associated with this Manager
        public ICollection<Client> Clients { get; set; }
        
        // Navigation property for the User associated with this Manager
        public User User { get; set; }
    }

    public enum Position
    {
        Senior,
        Junior
    }
}
