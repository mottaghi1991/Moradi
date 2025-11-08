using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Dto.ViewModel.main;
using Domain;
using Microsoft.AspNetCore.Http;


namespace Core.Extention
{
   public static class FileTools
   {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };

        // MIME های مجاز
        private static readonly string[] AllowedMimeTypes = { "image/jpeg", "image/png", "application/pdf" };

        // Magic Numbers برای فرمت‌های مجاز
        private static readonly Dictionary<string, byte[][]> FileSignatures = new()
    {
        { ".jpg", new[] { new byte[] { 0xFF, 0xD8, 0xFF } } },
        { ".jpeg", new[] { new byte[] { 0xFF, 0xD8, 0xFF } } },
        { ".png", new[] { new byte[] { 0x89, 0x50, 0x4E, 0x47 } } },
        { ".pdf", new[] { new byte[] { 0x25, 0x50, 0x44, 0x46 } } } // %PDF
    };
        public static string GetFileName(IFormFile FileAttach)
        {
            var extension = Path.GetExtension(FileAttach.FileName); // پسوند با نقطه
            var shortGuid = Guid.NewGuid().ToString("N").Substring(0, 8); // فقط ۸ کاراکتر
            var nameOnly = Path.GetFileNameWithoutExtension(FileAttach.FileName);

            if (nameOnly.Length > 8)
                nameOnly = nameOnly.Substring(0, 8); // حداکثر ۸ کاراکتر از نام فایل

            var fileName = $"{nameOnly}_{shortGuid}{extension}";
            return fileName;
        }

       public static bool CheckFormat(IFormFile FileAttach,List<string> ValidFormat = null)
       {
           var fileformat = Path.GetExtension(FileAttach.FileName);
           if (ValidFormat.Any(a=>a== fileformat))
           {
               return true;
           }

           return false;
       }
    
        public static FileUploadResult UploadFile(IFormFile FileAttach, string FileName,string FolderName)
        {
            var result = new FileUploadResult();

            try
            {
                // استفاده از مسیر ایمن‌تر در ویندوز
                var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FileUpload", FolderName);
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                var fullPath = Path.Combine(basePath, FileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    FileAttach.CopyTo(stream);
                }

                result.Success = true;
                result.FilePath = $"/FileUpload/{FolderName}/{FileName}";
            }
            catch (UnauthorizedAccessException ex)
            {
                // دسترسی نوشتن وجود ندارد
                result.Success = false;
                result.ErrorMessage = "عدم دسترسی برای ذخیره‌سازی فایل در مسیر سرور.";
                result.ErrorMessage = ex.Message;
            }
            catch (IOException ex)
            {
                // خطای فیزیکی IO
                result.Success = false;
                result.ErrorMessage = "خطا در عملیات ورودی/خروجی هنگام ذخیره فایل.";
                result.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                // سایر خطاهای کلی
                result.Success = false;
                result.ErrorMessage = "خطای غیرمنتظره در بارگذاری فایل رخ داد.";
                result.ErrorMessage = ex.Message;
            }

            return result;

        }
        public static FileUploadResult DeleteFile(string Path)
        {
            var result = new FileUploadResult();

            try
            {
                var p = Directory.GetCurrentDirectory() + "/wwwroot/" + Path;
                if (File.Exists(p))
                {
                    File.Delete(p);
                    result.Success = true;
                    result.ErrorMessage = "فایل با موفقیت حذف شد";
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "فایل یافت نشد";
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                result.Success = false;
                result.ErrorMessage = "عدم دسترسی به فایل";
            }
            catch (IOException ex)
            {
                result.Success = false;
                result.ErrorMessage = "خطای فایل در زمان حذف";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = "خطای غیرمنتظره در حذف فایل";
            }

            return result;
        }

        public static void ChechSize(this IFormFile file,int maxsizeMb)
        {
            if(file==null)
                throw new ArgumentNullException(nameof(file), "هیچ فایلی ارسال نشده است.");
            long maxbyte = maxsizeMb *1024 * 1024;
            if(file.Length>maxbyte)
                throw new InvalidOperationException($"حجم فایل نباید بیشتر از {maxsizeMb} مگابایت باشد.");
        }
        public static long GetAllSize( List<IFormFile> files)
        {
            long size = 0;
            foreach (IFormFile file in files)
            {
                size += file.Length;
            }
            return size;
        }
        public static bool IsValidUploadedFile(List<IFormFile> files)
        {
            foreach(var file in files)
            {
                if (file == null || file.Length == 0)
                    return false;

                // 1️⃣ چک پسوند
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(ext))
                    return false;

                // 2️⃣ چک MIME Type
                if (!AllowedMimeTypes.Contains(file.ContentType))
                    return false;

                // 3️⃣ چک Magic Number
                using (var reader = new BinaryReader(file.OpenReadStream()))
                {
                    var signatures = FileSignatures[ext];
                    var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                    if (!signatures.Any(sig => headerBytes.Take(sig.Length).SequenceEqual(sig)))
                        return false;
                }
           
            }
            return true;

        }
    }
}