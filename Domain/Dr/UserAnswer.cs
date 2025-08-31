using Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dr
{
    public class UserAnswer:Base
    {
        public int UserId { get; set; }
        public MyUser user{ get; set; }
        public int QuestionId { get; set; }
        public Question question{ get; set; }
        public string Answer { get; set; }
        public int DietId { get; set; }
        public Diet Diet { get; set; }
        public int UserDietId { get; set; }
        public UserDiet userDiet { get; set; }


    }
}
