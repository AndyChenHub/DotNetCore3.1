using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UserManagementAPI.Dtos;

namespace UserManagementAPI.Models
{
    public class ManagerDto
    {
        [Key]
        public int ManagerId { get; set; }

        public string UserName { get; set; }

        public ICollection<ClientDto> Clients { get; set; }
    }
}
