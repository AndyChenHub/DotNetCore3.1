namespace UserManagementAPI.Dtos
{
    public class ClientWithManagerDto
    {
        public int ClientId { get; set; }

        public string UserName { get; set; }

        public string ManagerUserName { get; set; }

        public int ManagerId { get; set; }
    }
}
