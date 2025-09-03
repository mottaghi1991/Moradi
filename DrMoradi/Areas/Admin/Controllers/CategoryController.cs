using AutoMapper;
using Core.Dto.ViewModel.Dr.DietVm;
using Core.Dto.ViewModel.Shop.Category;
using Core.Extention;
using Core.Service.Interface.Shop;
using Domain.Dr;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WebStore.Base;

namespace Personal.Areas.Admin.Controllers
{
    [Area(AreaName.Admin)]
    public class CategoryController : BaseController
    {
        private readonly ICategory _Category;
        private readonly IMapper _mapper;

        public CategoryController(ICategory Category, IMapper mapper)
        {
            _Category = Category;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _Category.GetAllCategory());
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryAddVM Category)
        {


            if (!ModelState.IsValid)
            {
                return View(Category);
            }
            if (Category.ImageFile == null || Category.ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "انتخاب تصویر الزامی است");
                return View(Category);
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(Category.ImageFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
            {
                ModelState.AddModelError("ImageFile", "فرمت فایل تصویر مجاز نیست.");
                return View(Category);
            }
            try
            {
                FileTools.ChechSize(Category.ImageFile, 1);

            }
            catch
            {
                ModelState.AddModelError("ImageFile", "حجم فایل نمیتواند بیشتر از 1M باشد");
                return View(Category);
            }

           

                string FIleName, FilePAth = null;
                FIleName = FileTools.GetFileName(Category.ImageFile);

                var FileResult = FileTools.UploadFile(Category.ImageFile, FIleName, "Category");
                if (!FileResult.Success)
                {
                    ModelState.AddModelError("ImageFile", "بارگذازی فایل با مشکل مواجه گردید");
                    return View(Category);
                }
                FilePAth = FileResult.FilePath;
                Category.Image = FilePAth;
                var result =await _Category.Insert(new Domain.DrShop.Category()
                {
                    CategoryName = Category.CategoryName,
                    Image = FilePAth,

                });
                if (result != null)
                {
                    TempData[Success] = SuccessMessage;
                    return RedirectToAction("Index");
                }
                else
                {
                    FileTools.DeleteFile(FilePAth);
                    TempData[Error] = ErrorMessage;
                    return RedirectToAction("Index");
                }
           
         





        }

        // GET: RoleController/Edit/5
        public async Task<ActionResult> Edit(int CategoryId)
        {

            var result =await _Category.GetCategoryById(CategoryId);
            if (result == null)
            {
                return NotFound();
            }
            return View( _mapper.Map<CategoryEditVM>(result));
        }

        // POST: RoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryEditVM CategoryVm)
        {
            if (!ModelState.IsValid)
            {
                return View(CategoryVm);
            }
           
            var old =await _Category.GetCategoryById(CategoryVm.Id);
            old.CategoryName = CategoryVm.CategoryName;
            var oldimage = CategoryVm.Image;
            if (CategoryVm.ImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(CategoryVm.ImageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("ImageFile", "فرمت فایل تصویر مجاز نیست.");
                    return View(CategoryVm);
                }
                try
                {
                    FileTools.ChechSize(CategoryVm.ImageFile, 1);

                }
                catch
                {
                    ModelState.AddModelError("ImageFile", "حجم فایل نمیتواند بیشتر از 1M باشد");
                    return View(CategoryVm);
                }

                string FIleName, FilePAth = null;
                FIleName = FileTools.GetFileName(CategoryVm.ImageFile);
                var FileResult = FileTools.UploadFile(CategoryVm.ImageFile, FIleName, "Category");
                if (!FileResult.Success)
                {
                    ModelState.AddModelError("ImageFile", "بارگذازی فایل با مشکل مواجه گردید");
                    return View(CategoryVm);
                }
                CategoryVm.Image = FileResult.FilePath;
            }



            var result =await _Category.Update(old);
            if (result != null)
            {
                if (CategoryVm.ImageFile != null)
                    FileTools.DeleteFile(oldimage);
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            if (oldimage != CategoryVm.Image)
                FileTools.DeleteFile(CategoryVm.Image);
           
                TempData[Error] = ErrorMessage;
                return RedirectToAction("Index");
            

        }

        // GET: RoleController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {



            var obj =await _Category.GetCategoryById(id);
            if (obj == null)
            {
                return NotFound();
            }

            if (_Category.GetCategoryById(id) != null)
            {
                TempData["Error"] = "منو دارای مجموعه می باشد لطفا ابتدا آنها را پاک کنید";
                return RedirectToAction("Index");
            }

            var result =await _Category.Delete(id);
            if (result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            TempData[Error] = ErrorMessage;
            return RedirectToAction("Index");


        }
    }
}
