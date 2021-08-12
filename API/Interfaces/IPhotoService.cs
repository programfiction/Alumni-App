using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public class ImageUploadResult{
        public string PublicId { get; set; }
        public string fileName { get; set; }
        
        
        
    }
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<ImageUploadResult> DeletePhotoAsync(string publicId);
    }
}