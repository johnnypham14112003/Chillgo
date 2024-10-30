using Chillgo.BusinessService.BusinessModels;

namespace Chillgo.BusinessService.Interfaces
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, Guid fileName, BM_Image imageData);
        Task<string> GetImageUrl(Guid imageName, byte typeReference);
        Task<bool> DeleteImageAsync(Guid fileName);
    }
}
