using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Shop
{
    public interface IAddress
    {
        Task<IEnumerable<ShippingAddres>> GetAddressOfUser(int UserId);
        Task<IEnumerable<ShippingAddres>> GetAloPeykAddressOfUser(int UserId);
        Task<bool> Add(ShippingAddres shippingAddres);
        Task<ShippingAddres> GetAddresById(int addressId);
    }
}
