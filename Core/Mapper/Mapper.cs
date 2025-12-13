using AutoMapper;
using Core.Dto.Shop.Batch;
using Core.Dto.ViewModel.Admin;
using Core.Dto.ViewModel.Admin.SettingDto;
using Core.Dto.ViewModel.Dr.DietVm;
using Core.Dto.ViewModel.Dr.DietVM;
using Core.Dto.ViewModel.main;
using Core.Dto.ViewModel.Shop.Category;
using Core.Dto.ViewModel.Store.ProductDto;
using Core.Dto.ViewModel.User;
using Domain.Dr;
using Domain.Main;
using Domain.Shop;
using Domain.User;


namespace Core.Mapper
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<MyUser, RegisterViewModel>().ReverseMap();

            CreateMap<Setting, EditSettingViewModel>().ReverseMap();
            CreateMap<Diet, AddDietVm>().ReverseMap();
            CreateMap<Diet, EditDietVm>().ReverseMap();

            CreateMap<Category,CategoryAddVM>().ReverseMap();
            CreateMap<Category,CategoryEditVM>().ReverseMap();


            CreateMap<PopUp, PopUpVm>().ReverseMap();
            CreateMap<PopUp, PopUpEditVm>().ReverseMap();

            CreateMap<Product, ProductAddVM>().ReverseMap();
            CreateMap<Product, ProductEditVm>().ReverseMap();

            CreateMap<ProductBatch, BatchAddVM>().ReverseMap();


            CreateMap<Setting, SettingVm>().ReverseMap();
            CreateMap<Slider, SliderEditVm>().ReverseMap();
        }
    }
}
