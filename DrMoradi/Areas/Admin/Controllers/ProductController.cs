using AutoMapper;
using Core.Dto.Shop.Batch;
using Core.Dto.Shop.ProductDto;
using Core.Dto.ViewModel.Admin.Role;
using Core.Dto.ViewModel.Store.ProductDto;
using Core.Dto.ViewModel.Store.ProductImageDto;
using Core.Extention;
using Core.Interface.Store;
using Core.Service.Interface.Shop;
using Domain.Shop;
using Domain.User.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WebStore.Base;

namespace DrMoradi.Areas.Admin.Controllers
{
    [Area(areaName:AreaName.Admin)]
    
    public class ProductController : BaseController
    {

    
        private readonly IProduct _Product;
        private readonly ICategory _category;
        private readonly IMapper _mapper;

        public ProductController(IProduct product, ICategory category, IMapper mapper)
        {
            _Product = product;
            _mapper = mapper;
            _category = category;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _Product.GetAll());
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.subcategory = new SelectList(await _category.GetAllCategory(), "Id", "CategoryName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductAddVM addVM)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.subcategory = new SelectList(await _category.GetAllCategory(), "Id", "CategoryName",addVM.CategoryId);
                return View(addVM);
            }
            if (addVM.ImageFile == null || addVM.ImageFile.Length == 0)
            {
                ViewBag.subcategory = new SelectList(await _category.GetAllCategory(), "Id", "CategoryName", addVM.CategoryId);

                ModelState.AddModelError("ImageFile", "انتخاب تصویر الزامی است");
                return View(addVM);
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(addVM.ImageFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
            {
                ViewBag.subcategory = new SelectList(await _category.GetAllCategory(), "Id", "CategoryName", addVM.CategoryId);

                ModelState.AddModelError("ImageFile", "فرمت فایل تصویر مجاز نیست.");
                return View(addVM);
            }
            try
            {
                FileTools.ChechSize(addVM.ImageFile, 1);

            }
            catch
            {
                ViewBag.subcategory = new SelectList(await _category.GetAllCategory(), "Id", "CategoryName", addVM.CategoryId);
                ModelState.AddModelError("ImageFile", "حجم فایل نمیتواند بیشتر از 1M باشد");
                return View(addVM);
            }
            string FIleName;
            FIleName = FileTools.GetFileName(addVM.ImageFile);

            var FileResult = FileTools.UploadFile(addVM.ImageFile, FIleName, "Product");
            if (!FileResult.Success)
            {
                ViewBag.subcategory = new SelectList(await _category.GetAllCategory(), "Id", "CategoryName", addVM.CategoryId);

                ModelState.AddModelError("ImageFile", "بارگذازی فایل با مشکل مواجه گردید");
                return View(addVM);
            }
            addVM.ImageUrl = FileResult.FilePath;
            var result =await _Product.Insert(_mapper.Map<Product>(addVM));
            if (result != null)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData[Error] = ErrorMessage;
                FileTools.DeleteFile(FileResult.FilePath);
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int ProductId)
        {
            var product =await _Product.GetProductById(ProductId);
            if(product==null)
            {
                return NotFound();
            }
            ViewBag.subcategory = new SelectList(await _category. GetAllCategory(), "Id", "CategoryName");

            return View(_mapper.Map<ProductEditVm>(product));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditVm editVm)
        {
            Product result;
            if (!ModelState.IsValid)
            {
                ViewBag.subcategory = new SelectList(await _category.GetAllCategory(), "Id", "CategoryName", editVm.CategoryId);
                return View(editVm);
            }
            var oldproduct =await _Product.GetProductById(editVm.Id);
            if (editVm.ImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(editVm.ImageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                {
                    ViewBag.subcategory = new SelectList(await _category.GetAllCategory(), "Id", "CategoryName");
                    ModelState.AddModelError("ImageFile", "فرمت فایل تصویر مجاز نیست.");
                    return View(editVm);
                }
                try
                {
                    FileTools.ChechSize(editVm.ImageFile, 1);

                }
                catch
                {
                    ViewBag.subcategory = new SelectList(await _category.GetAllCategory(), "Id", "CategoryName");
                    ModelState.AddModelError("ImageFile", "حجم فایل نمیتواند بیشتر از 1M باشد");
                    return View(editVm);
                }

                string FIleName, FilePAth = null;
                FIleName = FileTools.GetFileName(editVm.ImageFile);
                var FileResult = FileTools.UploadFile(editVm.ImageFile, FIleName, "Product");
                if (!FileResult.Success)
                {
                    ModelState.AddModelError("ImageFile", "بارگذازی فایل با مشکل مواجه گردید");
                    return View(editVm);
                }
                editVm.ImageUrl = FileResult.FilePath;
                FileTools.DeleteFile(oldproduct.ImageUrl);

                result =await _Product.Update(_mapper.Map<Product>(editVm));
            }

          
            else
            {
                editVm.ImageUrl = oldproduct.ImageUrl;
                result =await _Product.Update(_mapper.Map<Product>(editVm));

            }
            if (result != null)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            else {
                TempData[Error] = ErrorMessage;
                ViewBag.subcategory = new SelectList(await _category.GetAllCategory(), "Id", "CategoryName", editVm.CategoryId);
                return View(editVm);
            }
        }
        public async Task<IActionResult> ProductImageList(int ProductId)
        {
            var product=await _Product.GetProductById(ProductId);
            if (product == null) { 
                return NotFound();
            }

            return View(new ProductImageListVM()
            {
                ProductId = ProductId,
                ProductName=product.ProductName,
                productImages=await _Product.GetAllImageOfProductById(ProductId)
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateImage(int ProductId,IFormFile ProductImage)
        {
            if(ProductImage==null)
            {
                TempData[Error] = "تصویر بارگذاری نگردیده است";
                return RedirectToAction("ProductImageList", new { ProductId = ProductId });
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(ProductImage.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
            {
                TempData[Error] = "فرمت فایل تصویر مجاز نیست.";
                return RedirectToAction("ProductImageList", new { ProductId = ProductId });
            }
            try
            {
                FileTools.ChechSize(ProductImage, 1);

            }
            catch
            {
                TempData[Error] = "حجم فایل نمیتواند بیشتر از 1M باشد.";
                return RedirectToAction("ProductImageList", new { ProductId = ProductId });
            }
            var product = _Product.GetProductById(ProductId);
            if(product==null)   
            {
                return NotFound();
            }
            string FIleName;
            FIleName = FileTools.GetFileName(ProductImage);
            var FileResult = FileTools.UploadFile(ProductImage, FIleName, "Product/ProductImage");
            if (!FileResult.Success)
            {
                TempData[Error] = "بارگذازی فایل با مشکل مواجه گردید";
                return RedirectToAction("ProductImageList", new { ProductId = ProductId });
            }
            
            var result =await _Product.InsertImage(new ProductImage()
            {
                ProductId = ProductId,
                ImageUrl = FileResult.FilePath
            });
            if (result != null)
            {
               
                TempData[Success] = SuccessMessage;
                return RedirectToAction("ProductImageList", new { ProductId = ProductId });
            }
            else
            {
                FileTools.DeleteFile(FileResult.FilePath);
                TempData[Error] = ErrorMessage;
                return RedirectToAction("ProductImageList", new { ProductId = ProductId });
            }

        }

        public async Task<IActionResult> DeleteImage(int ProductImageId)
        {
            var product =await _Product.GetProductImageById(ProductImageId);
            if (product == null)
            {
                return NotFound();
            }
            try
            {
                FileTools.DeleteFile(product.ImageUrl);
               await _Product.DeleteImage(ProductImageId);
                TempData[Success] = SuccessMessage;
                return RedirectToAction("ProductImageList", new { ProductId = product.ProductId });
            }
            catch
            {
                TempData[Error] = ErrorMessage;
                return RedirectToAction("ProductImageList", new { ProductId = product.ProductId });
            }
         
          
      
        }
        public async Task<IActionResult> ListBatch(int ProductId)
        {
            var obj = await _Product.GetProductById(ProductId);
            if (obj == null)
            {
                return NotFound();
            }

            return View( new BatchListVm()
            {
                ProductId = ProductId,
                ProductName = obj.ProductName,
                Batches = await _Product.GetAllBatchForProduct(ProductId)
            });
        }
        public async Task<IActionResult> AddBatch(int ProductId)
        {
            var obj = await _Product.GetProductById(ProductId);
            if (obj==null)
            {
                return NotFound();
            }
            return View(new BatchAddVM()
            {
                ProductID = ProductId
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddBatch(BatchAddVM productBatch)
        {
            if (!ModelState.IsValid)
            {
                return View(productBatch);

            }

            productBatch.CreateDate = productBatch.CreateDate.ToMiladi().ToString();
            var result = await _Product.InsertBatchId(_mapper.Map<ProductBatch>(productBatch));
            if (result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("ListBatch", new { ProductId = productBatch.ProductID });
            }
            else
            {
                TempData[Error] = ErrorMessage;
                return View(productBatch);
            }
        }
        public async Task<IActionResult> EditBatch(int BatchId)
        {
            var obj = await _Product.GetProductBatchById(BatchId);
            if (obj == null)
            {
                return NotFound();
            }

            var vm = _mapper.Map<BatchAddVM>(obj);
            vm.CreateDate =obj.CreateDate.ToPersian();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditBatch(BatchAddVM batch)
        {
            if (!ModelState.IsValid)
            {
                return View(batch);

            }

            batch.CreateDate = batch.CreateDate.ToMiladi().ToString();
            var result = await _Product.UpdateBatchId(_mapper.Map<ProductBatch>(batch));
            if (result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("ListBatch", new { ProductId = batch.ProductID });
            }
            else
            {
                TempData[Error] = ErrorMessage;
                return View(batch);
            }
        }
        [HttpGet]
        public async Task<IActionResult> ProductCategory(int ProductId)
        {

            var Product = await _Product.GetProductById(ProductId);
            if (Product == null)
            {
                return BadRequest();
            }
            ViewBag.Category = await _category.GetAllByActive(true);
            var obj = new EditProductCategoryVm()
            {
                ProductId = ProductId,
                ProductName = Product.ProductName,
                SelectCategory = await _category.GetCateoryOfProduct(ProductId)
            };
            return View(obj);


     
       
        }
        [HttpPost]
        public async Task<IActionResult> ProductCategory(List<int> CategoryList, int ProductId)
        {
            var deletedata = await _category.GetCateoryOfProduct(ProductId);
            if (deletedata.Any())
            {
                if (await _category.BulkDeletePC(deletedata.ToList()) != true)
                {
                    TempData["Error"] = "خطایی رخ داده است";
                    return RedirectToAction("ProductCategory", new { ProductId = ProductId });
                }

            }
            List<ProductCategory> list = new List<ProductCategory>();
            foreach (int i in CategoryList)
            {
                list.Add(new ProductCategory()
                {
                    ProductId = ProductId,
                    CategoryId = i
                });
            }
         

            var obj = await _category.BulkInsertPC(list);
            if (obj)
            {
                TempData["Success"] = "عملیات با موفقیت انجام گردید";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                TempData["Error"] = "خطایی رخ داده است";
                return RedirectToAction("ProductCategory", new { ProductId = ProductId });
            }
        }
    }
}
