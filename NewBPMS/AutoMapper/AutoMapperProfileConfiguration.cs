using AutoMapper;
using NewBPMS.Models;
using NewBPMS.ViewModels.ContractViewModels;
using NewBPMS.ViewModels.UserContractViewModels;
using System;

namespace NewBPMS.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<Contract, ContractWarningViewModel>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.ApplicationUser.StaffName))
                .ForMember(dest => dest.DelayDays, src => src.MapFrom(x => DateTime.Now.Date.AddDays(x.Deadline*(-1)).Subtract(x.SignedDate).TotalDays))
                .ForMember(dest => dest.DelayWarningDays, src => src.MapFrom(x => x.SignedDate.AddDays(x.Deadline).Subtract(DateTime.Now.Date).TotalDays));


            CreateMap<Contract, EditContractViewModel>();

            CreateMap<EditContractViewModel, Contract>();


            CreateMap<Contract, DeleteContractViewModel>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.ApplicationUser.StaffName));
            //CreateMap<source, destination>()
            CreateMap<Contract, ContractViewModel>()
            //    .ForMember(dest => dest.EngineeringStatus, src => src.MapFrom(x => x.EngineeringStatus))
                .ForMember(dest => dest.UserName, src => src.MapFrom(x => x.ApplicationUser.StaffName));
            //p => p.EngineeringStatus, q => q.MapFrom(x => (EngineeringStatus)x.EngineeringStatus));

            CreateMap<CreateContractViewModel, Contract>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => Guid.NewGuid()));

            CreateMap<UserContract, UserProductValueDetailsViewModel>()
                .ForMember(dest => dest.StaffNo, src => src.MapFrom(x => x.ApplicationUser.StaffNo))
                .ForMember(dest => dest.StaffName, src => src.MapFrom(x => x.ApplicationUser.StaffName))
                .ForMember(dest => dest.ContractNo, src => src.MapFrom(x => x.Contract.No))
                .ForMember(dest => dest.ContractName, src => src.MapFrom(x => x.Contract.Name))
                .ForMember(dest => dest.Amount, src => src.MapFrom(x => (x.Ratio) * x.Contract.Amount));

            CreateMap<UserContract, UserProductValueViewModel>()
                .ForMember(dest => dest.Labor, src => src.MapFrom(x => (Labor)x.Labor))
                .ForMember(dest => dest.StaffName, src => src.MapFrom(x => x.ApplicationUser.StaffName))
                .ForMember(dest => dest.Amount, src => src.MapFrom(x => (x.Ratio)*x.Contract.Amount));

            //CreateMap<UserContract, EditUserContractViewModel>()
            //    .ForMember(dest => dest.StaffName, src => src.MapFrom(x => x.ApplicationUser.StaffName));

            CreateMap<CreateUserContractViewModel, UserContract>()
                .ForMember(dest => dest.Id, src => src.MapFrom(x => Guid.NewGuid()));

            CreateMap<EditUserContractViewModel, UserContract>();

            //CreateMap<CMProject, EditCMProjectViewModel>();

            //CreateMap<EditCMProjectViewModel, CMProject>();
            //.ForMember(dest => dest.Id, src =>src.Ignore());


        }
    }
}
