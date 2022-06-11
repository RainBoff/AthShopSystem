using asp_mvc_std_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace asp_mvc_std_v2.Profiles
{
    public class RoleProfile : Profile
    {

        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>();
        }

    }

}