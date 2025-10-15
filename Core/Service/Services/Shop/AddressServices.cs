using Core.Service.Interface.Shop;
using Data.MasterInterface;
using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Shop
{
    public class AddressServices : IAddress
    { private readonly IMaster<ShippingAddres> _master;

        public AddressServices(IMaster<ShippingAddres> master)
        {
            _master = master;
        }

        public async Task<IEnumerable<ShippingAddres>> GetAloPeykAddressOfUser(int UserId)
        {
            return await _master.GetAllEfAsync(a => a.UserId == UserId&&a.Latitude != null);
        }

        public async Task<bool> Add(ShippingAddres shippingAddres)
        {
            var obj = await _master.InsertAsync(shippingAddres);
            return obj != null;
        }

        public async Task<ShippingAddres> GetAddresById(int addressId)
        {
            var obj = await _master.GetAllEfAsync(a => a.Id == addressId);
            return obj.FirstOrDefault();


        }





        public async Task<IEnumerable<ShippingAddres>> GetAddressOfUser(int UserId)
        {
            return await _master.GetAllEfAsync(a => a.UserId == UserId&&a.Latitude==null);
        }
    }
}
