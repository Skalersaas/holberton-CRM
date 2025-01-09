using AutoMapper;
using HolbertonCRM.Application.DTOs.Auth;
using HolbertonCRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HolbertonCRM.Application.Mapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<UserDto, AppUser>().ReverseMap();
            CreateMap<RegisterDto, AppUser>().ReverseMap();
            CreateMap<LoginRequestDto, AppUser>().ReverseMap();
            CreateMap<LoginResponseDto, AppUser>().ReverseMap();

        }
    }
}
