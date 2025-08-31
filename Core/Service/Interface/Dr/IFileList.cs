using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Dr
{
    public interface IFileList
    {
        Task<bool> InsertFile(FileList fileList);
        Task<bool> deleteFile(string filename);
        Task<IEnumerable<FileList>> GetALlfileByUserDietId(int UserDietId, bool UserFile);
    }
}
