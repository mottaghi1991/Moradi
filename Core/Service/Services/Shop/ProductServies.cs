using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Dto.ViewModel.Store.ProductDto;
using Core.Interface.Store;

using Data.MasterInterface;
using Domain.Shop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dto.Shop.ProductDto;
using Core.Service.Interface.Shop;

namespace Core.Services.Store
{
    public class ProductServies : IProduct
    {
        private readonly IMaster<Product> _master;
        private readonly IMaster<ProductImage> _masterImage;
        private readonly IMaster<ProductBatch> _masterBach;
        private readonly IMapper _mapper;
        private readonly IOrderItem _orderItem;

        public ProductServies(IMaster<Product> master, IMaster<ProductImage> masterImage, IMapper mapper, IOrderItem orderItem, IMaster<ProductBatch> masterBach)
        {
            _master = master;
            _masterImage = masterImage;
            _mapper = mapper;
            _orderItem = orderItem;
            _masterBach = masterBach;
        }

        public async Task<bool> DeleteImage(int productImageId)
        {
            var obj = await GetProductImageById(productImageId);
            return await _masterImage.DeleteAsync(obj);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _master.GetAllEfAsync();
        }

        public async Task<IEnumerable<ProductImage>> GetAllImageOfProductById(int ProductId)
        {
            return await _masterImage.GetAllEfAsync(a => a.ProductId == ProductId);
        }

        public async Task<IEnumerable<Product>> getByFilter(int? categoryId, string sort)
        {

            IQueryable<Product> obj = _master.GetAllAsQueryable()
                .Include(p => p.ProductBatches);

            if (categoryId.HasValue)
                obj = obj.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort == "cheap")
                    obj = obj.OrderBy(p =>
                        p.ProductBatches
                            .Where(b => b.IsActive)
                            .Select(b => (decimal?)b.Price)   // از SalePrice به‌جای Price
                            .FirstOrDefault() ?? 0);

                else if (sort == "expensive")
                    obj = obj.OrderByDescending(p =>
                        p.ProductBatches
                            .Where(b => b.IsActive)
                            .Select(b => (decimal?)b.Price)
                            .FirstOrDefault() ?? 0);
            }
            return await obj.ToListAsync();
        }

        public Task<IEnumerable<Product>> GetProductBybcategory(int CategoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetProductById(int ProductId)
        {
            var product = await _master.GetAllAsQueryable()
                .Include(a => a.ProductBatches)
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == ProductId);

        

            return product;
        }

     



        public async Task<ProductImage> GetProductImageById(int productImageId)
        {
            var obj = await _masterImage.GetAllEfAsync(a => a.Id == productImageId);
            return obj.FirstOrDefault();
        }

        public async Task<ShowProductDetailVm> GetShowProductDetailVmByProductId(int ProductId)
        {
            var product = await _master.GetAllAsQueryable()
                .Include(a => a.ProductBatches)
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == ProductId);

            var batch = product?.ProductBatches
                .Where(b => b.IsActive)
                .OrderBy(b => b.CreateDate)
                .ThenBy(b => b.Id)
                .FirstOrDefault();

            if (batch == null)
                return new ShowProductDetailVm();
              
            // 🔹 محاسبه تعداد فروخته‌شده از همان Batch
            var soldCount = await _orderItem.GetSumOrderItembyBatchId(batch.Id);
             

            // ✅ موجودی واقعی
            var realStock = Math.Max(batch.Stock - soldCount, 0);

            var vm = new ShowProductDetailVm
            {
                Id = ProductId,
                ProductName = product?.ProductName ?? "",
                CategoryName = product?.Category?.CategoryName,
                Attrib = product?.Attrib,
                Price = batch.Price ,
                Stock = realStock
            };

            return vm;
        }

        public async Task<ProductBatchUsageDto> GetBatchUsageAsync(int productBatchId)
        {
            var batch = await _masterBach.GetAllAsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == productBatchId);

            if (batch == null)
                return null;

            // محاسبه تعداد فروش رفته از جدول OrderItems

            var soldCount = await _orderItem.GetSumOrderItembyBatchId(productBatchId);

            // باقی‌مانده واقعی (اختلاف بین کل و فروش‌رفته)
            var remaining = batch.Stock - soldCount;
            if (remaining < 0) remaining = 0;

            return new ProductBatchUsageDto
            {
                ProductBatchId = batch.Id,
                InitialStock = batch.Stock,
                SoldCount = soldCount,
                RemainingCount = remaining
            };
        }

        public async Task<ProductBatch> GetProductBatchById(int BatchId)
        {
            return  _masterBach.GetAllAsQueryable(a => a.Id == BatchId).FirstOrDefault();
        }

        public async Task<bool> UpdateBatchId(ProductBatch productBatch)
        {
            var obj= await _masterBach.UpdateAsync(productBatch);
            return obj != null;
        }

        public async Task<bool> InsertBatchId(ProductBatch productBatch)
        {
            var obj = await _masterBach.InsertAsync(productBatch);
            return obj != null;
        }

        public Task<ProductBatch> GetActiveBatchForProduct(int productId)
        {
return  _masterBach.GetAllAsQueryable(b => b.ProductID == productId && b.IsActive)
    .OrderBy(b => b.CreateDate)
    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductBatch>> GetAllBatchForProduct(int productId)
        {
            return _masterBach.GetAllAsQueryable(a => a.ProductID == productId)
                .Include(a => a.Product)
                .OrderBy(a=>a.CreateDate)
                .ToList();
        }

        public async Task<int> GetStockAsync(int productId)
        {
            var batch = await GetActiveBatchForProduct(productId);

            if (batch == null) return 0;

            var soldItems = await _orderItem.GetAllPaidItemByBatchId(batch.Id);
               

            var soldCount = soldItems.Sum(o => o.Quantity);
            var remaining = Math.Max(batch.Stock - soldCount, 0);
            return remaining;

        }

        public async Task<Product> Insert(Product product)
        {
            return await _master.InsertAsync(product);
        }

        public async Task<ProductImage> InsertImage(ProductImage productImage)
        {
            return await _masterImage.InsertAsync(productImage);
        }



        public async Task<Product> Update(Product product)
        {
            return await _master.UpdateAsync(product);
        }




    }
}
