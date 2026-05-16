using AutoMapper;
using MVC_Project.DAL.Models;
using MVC_Project.PL.ViewModels;

namespace MVC_Project.PL.MappingProfiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>();
        }
    }
}
