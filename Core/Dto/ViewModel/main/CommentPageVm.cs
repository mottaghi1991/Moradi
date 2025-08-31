using Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.main
{
    public class CommentPageVm
    {
        public IEnumerable<Comment> Comments{ get; set; }
        public int TotalPage { get; set; }
        public int Page { get; set; }
    }
}
