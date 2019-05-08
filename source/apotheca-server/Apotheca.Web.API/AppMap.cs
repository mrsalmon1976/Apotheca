using Apotheca.BLL.Models;
using Apotheca.Web.API.ViewModels.Account;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apotheca.Web.API
{
    public class AppMap
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<RegisterViewModel, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Salt, opt => opt.Ignore())
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.Stores, opt => opt.Ignore())
                .ForMember(dest => dest.Created, opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationCompleted, opt => opt.Ignore())
            );

            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}
