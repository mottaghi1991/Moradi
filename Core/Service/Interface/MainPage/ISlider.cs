using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.MainPage
{
    public interface ISlider
    {
        public Task<IEnumerable<Slider>> GetSliders();
    }
}
