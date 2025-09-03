using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto.ViewModel;
using Core.Dto.ViewModel.Admin;
using Core.Dto.ViewModel.Dr.DietVm;
using Core.Dto.ViewModel.Dr.DietVM;
using Core.Dto.ViewModel.main;
using Core.Dto.ViewModel.Shop.Category;
using Data.Migrations;
using Domain.Dr;
using Domain.DrShop;
using Domain.Main;
using Domain.PersonalData;


namespace Core.Mapper
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Setting, EditSettingViewModel>().ReverseMap();
            CreateMap<Diet, AddDietVm>().ReverseMap();
            CreateMap<Diet, EditDietVm>().ReverseMap();

            CreateMap<Category,CategoryAddVM>().ReverseMap();
            CreateMap<Category,CategoryEditVM>().ReverseMap();


            CreateMap<PopUp, PopUpVm>().ReverseMap();
            CreateMap<PopUp, PopUpEditVm>().ReverseMap();
       
        }
    }
}
