using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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
        [EnumMember(Value = "Senior")]
        Senior,
        [EnumMember(Value = "Junior")]

        Junior
    }
}
