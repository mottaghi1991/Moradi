using Core.Dto.ViewModel.Dr.DietVM;
using Core.Interface.Admin;
using Core.Interface.Sms;
using Core.Service.Interface.Admin;
using Core.Service.Interface.Dr;
using Core.Service.Interface.MainPage;
using Core.Service.Interface.Payment;
using Core.Service.Interface.Shop;
using Core.Service.Interface.Users;
using Core.Service.Services.Admin;
using Core.Service.Services.Dr;
using Core.Service.Services.MainPage;
using Core.Service.Services.Payment;
using Core.Service.Services.Shop;
using Core.Service.Services.Sms;
using Core.Service.Services.Users;
using Core.Services.Sms;
using Core.Services.Users;
using Data.MasterInterface;
using Data.MasterServices;
using Domain.Dr;
using Domain.DrShop;
using Domain.Main;
using Domain.PersonalData;
using Domain.SMS;
using Domain.User;
using Domain.User.Permission;
using Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IOC
{
    public class UserDependencyContainer
    {
        public static void RegisterServices(IServiceCollection Services)
        {


            #region Admin

            Services.AddScoped<IUser, UserServices>();

            #endregion
            #region Dr
            Services.AddScoped<IDiet, DietServices>();
            Services.AddScoped<IQuestion, QuestionServices>();
            Services.AddScoped<IUserDiet, UserDietServices>();
            Services.AddScoped<IUserAnswer, UserAnswerServices>();
            Services.AddScoped<IQuestionOption, QuestionOptionServices>();
            Services.AddScoped<ISendDiet, SendDietServices>();
            Services.AddScoped<IFileList, FileListServices>();
            Services.AddScoped<IPost, PostServices>();




            Services.AddScoped<IMaster<Diet>, MasterServices<Diet>>();
            Services.AddScoped<IMaster<Question>, MasterServices<Question>>();
            Services.AddScoped<IMaster<DietQuestion>, MasterServices<DietQuestion>>();
            Services.AddScoped<IMaster<UserDiet>, MasterServices<UserDiet>>();
            Services.AddScoped<IMaster<UserAnswer>, MasterServices<UserAnswer>>();
            Services.AddScoped<IMaster<QuestionOption>, MasterServices<QuestionOption>>();
            Services.AddScoped<IMaster<SendDiet>, MasterServices<SendDiet>>();
            Services.AddScoped<IMaster<FileList>, MasterServices<FileList>>();
            Services.AddScoped<IMaster<Post>, MasterServices<Post>>();



            //ViewModel
            Services.AddScoped<IMaster<ShowUserDietPanelVm>, MasterServices<ShowUserDietPanelVm>>();
        


            #endregion
            #region User

            Services.AddScoped<IPermisionList, PermissionListServices>();
            Services.AddScoped<IRole, RoleServices>();
            Services.AddScoped<IRolePermission, RolePermissionServices>();
            Services.AddScoped<IViewRender, RenderViewToStringServices>();
     
         
        

      


            Services.AddScoped<IMaster<PermissionList>, MasterServices<PermissionList>>();
            Services.AddScoped<IMaster<MyUser>, MasterServices<MyUser>>();
            Services.AddScoped<IMaster<Role>, MasterServices<Role>>();
            Services.AddScoped<IMaster<RolePermission>, MasterServices<RolePermission>>();
            Services.AddScoped<IMaster<UserRole>, MasterServices<UserRole>>();
            Services.AddScoped<IMaster<Setting>, MasterServices<Setting>>();


            #endregion
            #region Shop
            Services.AddScoped<IMaster<Category>, MasterServices<Category>>();

            Services.AddScoped<ICategory, CategoriesServices>();

            #endregion
            #region Sms
            //Services.AddScoped<ISms, SmsIRServices>();
            Services.AddScoped<ISms, KavehNegarSmsServices>();
            Services.AddScoped<IUserOtp, UserOtpServices>();


            Services.AddScoped<IMaster<UserOtp>, MasterServices<UserOtp>>();

            #endregion

            #region Main
            Services.AddScoped<IMaster<Slider>, MasterServices<Slider>>();
            Services.AddScoped<IMaster<Comment>, MasterServices<Comment>>();
            Services.AddScoped<IMaster<PopUp>, MasterServices<PopUp>>();

            Services.AddScoped<ISetting, SettingServices>();
            Services.AddScoped<ISlider, SliderServices>();
            Services.AddScoped<IComment, CommentServices>();
            Services.AddScoped<IPopUp, PopUpServices>();
            #endregion


            #region Payment
            Services.AddScoped<IPayment, PaymentServices>();

            #endregion
        }
    }
}
