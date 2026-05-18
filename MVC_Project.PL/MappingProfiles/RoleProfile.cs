using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MVC_Project.PL.ViewModels;

namespace MVC_Project.PL.MappingProfiles
{
    public class RoleProfile: Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel, IdentityRole>().ForMember(I => I.Name, O => O.MapFrom(R => R.RoleName)).ReverseMap();
        }
    }
}
