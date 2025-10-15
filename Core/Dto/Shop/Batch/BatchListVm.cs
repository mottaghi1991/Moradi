using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Shop;

namespace Core.Dto.Shop.Batch
{
    public class BatchListVm
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<ProductBatch> Batches { get; set; }
    }
}
