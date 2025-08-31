using Domain;
using Domain.Dr;
using Domain.DrShop;
using Domain.Main;
using Domain.PersonalData;
using Domain.SMS;
using Domain.User;
using Domain.User.Permission;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class MyContext:DbContext
    {
        public MyContext(DbContextOptions<MyContext> options):base(options)
        {
            
        }
        public virtual DbSet<MyUser> MyUser { get; set; }
        public virtual DbSet<PermissionList> PermissionList { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RolePermission> RolePermission { get; set; }
        public virtual DbSet<PopUp> PopUps{ get; set; }

        #region Dr
        public virtual DbSet<Diet> Diets{ get; set; }
        public virtual DbSet<Question> Questions{ get; set; }
        public DbSet<DietQuestion> DietQuestions { get; set; }
        public DbSet<UserDiet> UserDiet{ get; set; }
        public DbSet<UserAnswer> userAnswers{ get; set; }
        public DbSet<QuestionOption> questionOptions{ get; set; }
        public DbSet<Post>  posts{ get; set; }


        #endregion


        #region Shop
        public virtual DbSet<Category> Categories{ get; set; }

        #endregion
        public virtual DbSet<Setting> Settings{ get; set; }
        public virtual DbSet<Slider> Sliders{ get; set; }
        public virtual DbSet<Comment> Comments{ get; set; }

        #region SMS
        public virtual DbSet<UserOtp> UserOtps { get; set; }
        #endregion



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(Base).IsAssignableFrom(entityType.ClrType))
                {
                    var prop = entityType.FindProperty(nameof(Base.IsDeleted));
                    if (prop != null)
                    {
                        prop.SetDefaultValue(false);
                    }
                }
            }
            modelBuilder.Entity<Question>().Property(a => a.FieldType).HasConversion<string>();
            modelBuilder.Entity<UserDiet>().Property(a => a.Status).HasConversion<string>();
            modelBuilder.Entity<Comment>().Property(a => a.EntityType).HasConversion<string>();
            modelBuilder.Entity<DietQuestion>().HasKey(a => new { a.DietId, a.QuestionId });
            modelBuilder.Entity<DietQuestion>().ToTable("DietQuestions");
            modelBuilder.Entity<Comment>()
                .HasOne(a => a.myUser)
                .WithMany(u => u.Comments)
                .HasForeignKey(a => a.UserId)
                .HasPrincipalKey(u => u.ItUserId);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserDiet>()
       .HasOne(ud => ud.Parent)          // یک UserDiet می‌تونه یک پدر داشته باشه
       .WithMany(ud => ud.Children)      // یک پدر می‌تونه چند فرزند داشته باشه
       .HasForeignKey(ud => ud.ParentId) // کلید خارجی برای Parent
       .OnDelete(DeleteBehavior.Restrict); // حذف پدر، بچه‌ها رو حذف نکن
        }
    }
}
