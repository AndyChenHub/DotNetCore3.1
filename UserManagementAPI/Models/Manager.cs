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

        public ICollection<Client> Clients { get; set; }
        
        public User User { get; set; }
    }

    public enum Position
    {
        Senior,
        Junior
    }
}
