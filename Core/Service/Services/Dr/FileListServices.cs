using Core.Service.Interface.Dr;
using Data.MasterInterface;
using Domain.Dr;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Dr
{
    public class FileListServices : IFileList
    {
        private readonly IMaster<FileList> _master;

        public FileListServices(IMaster<FileList> master)
        {
            _master = master;
        }

        public async Task<bool> deleteFile(string filename)
        {
          var obj=await _master.GetAllEfAsync(a=>a.File==filename);
            if (obj != null)
            {
                var result = await _master.DeleteAsync(obj.FirstOrDefault());
                if (result)
                    return true;
            }
          return false;
                          
        }

        public async Task<IEnumerable<FileList>> GetALlfileByUserDietId(int UserDietId, bool UserFile)
        {
            var obj = await _master.GetAllEfAsync(a => a.UserDietId == UserDietId&&a.UserFile==UserFile);
            return obj;
        }

        public async Task<bool> InsertFile(FileList fileList)
        {
         var obj=await _master.InsertAsync(fileList);   
            return obj!=null;
        }
    }
}
