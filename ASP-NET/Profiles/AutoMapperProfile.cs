using ASP_NET.Models;
using AutoMapper;
using DAL.Entities;
using System;
using System.Linq.Expressions;

namespace ASP_NET.Profiles
{

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, User>(MemberList.None).
                ForMember(x => x.Id, opt => opt.Ignore()).
                ForMember(x => x.LockoutEnabled, opt => opt.Ignore()).
                ForMember(x => x.AccessFailedCount, opt => opt.Ignore()).
                ForMember(x => x.LockoutEnd, opt => opt.Ignore()).
                ForMember(x => x.EmailConfirmed, opt => opt.Ignore()).
                ForMember(x => x.LockoutEnd, opt => opt.Ignore()).
                ForMember(x => x.NormalizedEmail, opt => opt.Ignore()).
                ForMember(x => x.NormalizedUserName, opt => opt.Ignore()).
                ForMember(x => x.PasswordHash, opt => opt.Ignore()).
                ForMember(x => x.PhoneNumberConfirmed, opt => opt.Ignore()).
                ForMember(x => x.ConcurrencyStamp, opt => opt.Ignore()).
                ForMember(x => x.TwoFactorEnabled, opt => opt.Ignore());
        }
    }
}
