using AutoMapper;
using UserManagementAPI.Dtos;
using UserManagementAPI.Models;

namespace UserManagementAPI.Profiles
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Manager, ManagerDto>().ReverseMap();
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<Client, ClientWithManagerDto>();

        }
    }
}