using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementAPI.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        public int Level { get; set; }

        [ForeignKey("ClientId")]
        public User User { get; set; }

        public int? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public Manager Manager { get; set; }
    }
}
