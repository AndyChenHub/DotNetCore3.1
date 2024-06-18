using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [StringLength(200)]
        public string? UserName { get; set; }

        [EmailAddress]
        [StringLength(1000)]
        public string? Email { get; set; }

        [StringLength(1000)]
        public string? Alias { get; set; }

        [StringLength(1000)]
        public string? FirstName { get; set; }

        [StringLength(1000)]
        public string? LastName { get; set; }
        
        public UserType UserType { get; set; }

        public Manager Manager { get; set; }
        public Client Client { get; set; }
    }

    public enum UserType
    {
        Client,
        Manager
    }
}