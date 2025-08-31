using Core.Service.Interface.MainPage;
using Data.MasterInterface;
using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.MainPage
{
    public class SliderServices : ISlider
    {
        private readonly IMaster<Slider> _master;

        public SliderServices(IMaster<Slider> master)
        {
            _master = master;
        }

        public async Task<IEnumerable<Slider>> GetSliders()
        {
            return await _master.GetAllEfAsync();
        }
    }
}
