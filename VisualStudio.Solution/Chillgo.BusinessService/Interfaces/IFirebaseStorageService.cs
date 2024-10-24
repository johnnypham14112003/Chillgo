using Chillgo.BusinessService.BusinessModels;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, BM_Image imageData);
        Task<string> GetImageUrl(string imageName, byte typeReference);
        Task<bool> DeleteImageAsync(string fileName);
    }
}
