using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
      
        public PhotoService(IWebHostEnvironment env, IConfiguration config)
        {
            _config = config;
           _env = env;
        }

        public  Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
           var fileName= string.Empty;
           var newFileName= string.Empty;
           var uploadResult= new ImageUploadResult();
            if(file.Length>0)
            {
                fileName= ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string myUniqueFileName= Convert.ToString(Guid.NewGuid());
             var fileExtension=Path.GetExtension(fileName);
             newFileName= myUniqueFileName+fileExtension;

           bool exists = System.IO.Directory.Exists(Path.Combine(_env.ContentRootPath,"demoImages"));

            if(!exists)
             System.IO.Directory.CreateDirectory(Path.Combine(Path.Combine(_env.ContentRootPath,"demoImages")));

             fileName=Path.Combine(_env.ContentRootPath,"demoImages")+$@"\{newFileName}";
             
             using(FileStream fs=  System.IO.File.Create(fileName))
             {
                 file.CopyTo(fs);
                 fs.Flush();
             }
             uploadResult.fileName=_config["WebServer"]+ "/WeatherForecast/photo/"+newFileName;
             uploadResult.PublicId= newFileName;
            }
            return Task.FromResult(uploadResult);
           
        }

        public  Task<ImageUploadResult> DeletePhotoAsync(string publicId)
        {
            var uploadResult=new ImageUploadResult();
            string fileName=Path.Combine( _env.ContentRootPath,"demoImages") + $@"\{publicId}";
            if(System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
                uploadResult.PublicId=fileName;
            }
            return Task.FromResult(uploadResult);
        }
    }
}