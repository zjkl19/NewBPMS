﻿using AutoMapper;
using NewBPMS.Models;
using NewBPMS.ViewModels.ContractViewModels;

namespace NewBPMS.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {

            //CreateMap<source, destination>()
            CreateMap<Contract, ContractViewModel>()
            //    .ForMember(dest => dest.EngineeringStatus, src => src.MapFrom(x => x.EngineeringStatus))
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.ApplicationUser.StaffName));
            //p => p.EngineeringStatus, q => q.MapFrom(x => (EngineeringStatus)x.EngineeringStatus));

            CreateMap<UserContract, UserProductValueViewModel>()
                .ForMember(dest => dest.Labor, src => src.MapFrom(x => (Labor)x.Labor))
                .ForMember(dest => dest.StaffName, src => src.MapFrom(x => x.ApplicationUser.StaffName))
                .ForMember(dest => dest.Amount, src => src.MapFrom(x => (x.Ratio)*x.Contract.Amount));

            //CreateMap<CMProject, EditCMProjectViewModel>();

            //CreateMap<EditCMProjectViewModel, CMProject>();
            //.ForMember(dest => dest.Id, src =>src.Ignore());


        }
    }
}
