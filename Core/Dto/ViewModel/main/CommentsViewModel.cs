using Domain;
using Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.main
{
    public class CommentsViewModel
    {
        public int EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
