using AutoMapper;
using Core.Dto.ViewModel.Admin;
using Core.Dto.ViewModel.Dr.DietVm;
using Core.Dto.ViewModel.Dr.DietVM;
using Core.Dto.ViewModel.main;
using Core.Dto.ViewModel.Shop.Category;
using Core.Dto.ViewModel.Store.ProductDto;
using Domain.Dr;
using Domain.Main;
using Domain.PersonalData;
using Domain.Shop;


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

            CreateMap<Product, ProductAddVM>().ReverseMap();
            CreateMap<Product, ProductEditVm>().ReverseMap();

        }
    }
}
