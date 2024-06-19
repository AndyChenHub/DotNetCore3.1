using System.ComponentModel.DataAnnotations;
using UserManagementAPI.Models;

namespace UserManagementAPI.Dtos
{
    public class UserDto
    {
        [Required]
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
    }
}