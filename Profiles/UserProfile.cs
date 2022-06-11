using asp_mvc_std_v2.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_mvc_std_v2.Profiles
{
    public class UserProfile : Profile
    {
            public UserProfile()
            {
                CreateMap<IdentityUser, User>();
            }
    }
}
