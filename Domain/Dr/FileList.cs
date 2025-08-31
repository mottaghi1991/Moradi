using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dr
{
    public class FileList:Base
    {
        public int UserDietId { get; set; }
   
        public string File { get; set; }
        public bool UserFile { get; set; }

        public UserDiet UserDiet{ get; set; }
  
    }
}
