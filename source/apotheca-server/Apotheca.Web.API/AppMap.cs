using Apotheca.Auth.Models;
using Apotheca.BLL.Models;
using Apotheca.Web.API.ViewModels.Account;
using Apotheca.Web.API.ViewModels.Common;
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
            Mapper.Initialize(cfg => {

                cfg.CreateMap<RegisterViewModel, User>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Salt, opt => opt.Ignore())
                    .ForMember(dest => dest.Token, opt => opt.Ignore())
                    .ForMember(dest => dest.Stores, opt => opt.Ignore())
                    .ForMember(dest => dest.Created, opt => opt.Ignore())
                    .ForMember(dest => dest.RegistrationCompleted, opt => opt.Ignore());

                cfg.CreateMap<Store, StoreViewModel>();

                cfg.CreateMap<UserResult, UserViewModel>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.Stores, opt => opt.Ignore())
                    .ForMember(dest => dest.Token, opt => opt.Ignore());

                cfg.CreateMap<User, UserViewModel>()
                    .ForMember(dest => dest.Stores, opt => opt.Ignore());
            });

            Mapper.Configuration.AssertConfigurationIsValid();
        }

        public static void Reset()
        {
            Mapper.Reset();
        }

        public static void ResetAndConfigure()
        {
            AppMap.Reset();
            AppMap.Configure();

        }
    }
}
