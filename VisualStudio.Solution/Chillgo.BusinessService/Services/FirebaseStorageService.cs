using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Extensions.Exceptions;
using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using Firebase.Storage;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Mapster;

namespace Chillgo.BusinessService.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _bucketName;

        public FirebaseStorageService(IUnitOfWork unitOfWork, string bucketName)
        {
            _unitOfWork = unitOfWork;
            _bucketName = bucketName;
        }

        private FirebaseStorage GetFirebaseStorage()
        {
            // Lấy instance của FirebaseApp đã được khởi tạo
            var app = FirebaseApp.DefaultInstance;
            if (app == null)
                throw new InvalidOperationException("Firebase App has not been initialized");

            // Lấy credentials từ app đã được khởi tạo
            var credential = (app.Options.Credential as GoogleCredential)
                ?.CreateScoped(new[]
                {
                    "https://www.googleapis.com/auth/firebase.storage",
                    "https://www.googleapis.com/auth/cloud-platform"
                });

            if (credential == null)
                throw new InvalidOperationException("Firebase credentials not found");

            return new FirebaseStorage(
                _bucketName,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => credential.UnderlyingCredential.GetAccessTokenForRequestAsync(),
                    ThrowOnCancel = true
                });
        }

        public async Task<string> UploadFileAsync(Stream fileStream, Guid fileName, BM_Image ImageData)
        {
            if (ImageData.AccountId is null && ImageData.LocationId is null)
                throw new BadRequestException("Unknown reference object of image! Must enter the Reference Object Id");

            Image? existImage = null;
            if (ImageData.Type == 1) existImage = await _unitOfWork.GetImageRepository().GetOneAsync(img => img.AccountId == fileName);
            else if (ImageData.Type == 3) existImage = await _unitOfWork.GetImageRepository().GetOneAsync(img => img.LocationId == fileName);

            try
            {
                var storage = GetFirebaseStorage();

                if (existImage is not null) await DeleteImageAsync(fileName);
                var fileUrl = await storage
                        .Child("images")
                        .Child(fileName.ToString())
                        .PutAsync(fileStream);

                //Save to DB
                var imageInDb = ImageData.Adapt<Image>();
                imageInDb.UrlPath = fileUrl;
                await _unitOfWork.GetImageRepository().AddAsync(imageInDb);

                await _unitOfWork.GetAccountRepository().SaveChangeAsync();

                return fileUrl;
            }
            catch (Exception ex)
            {
                throw new Exception($"File upload failed: {ex.Message}");
            }
        }

        public async Task<string> GetImageUrl(Guid imageName, byte typeReference)
        {
            try
            {
                var imageInDb = await _unitOfWork.GetImageRepository().GetImageAsync(imageName, typeReference);
                if (imageInDb != null && !string.IsNullOrEmpty(imageInDb.UrlPath))
                {
                    return imageInDb.UrlPath;
                }

                try
                {
                    //search on Firebase
                    var storage = GetFirebaseStorage();

                    string fileUrl = await storage
                        .Child("images")
                        .Child(imageName.ToString())
                        .GetDownloadUrlAsync();

                    // Cập nhật hoặc thêm mới vào database
                    if (imageInDb == null)
                    {
                        var newImage = new Image
                        {
                            UrlPath = fileUrl,
                            Type = typeReference
                        };

                        if (typeReference == 1) newImage.AccountId = imageName;
                        else if (typeReference == 3) newImage.LocationId = imageName;

                        await _unitOfWork.GetImageRepository().AddAsync(newImage);
                        var result = await _unitOfWork.GetAccountRepository().SaveChangeAsync();
                    }

                    return fileUrl;
                }
                catch (FirebaseStorageException ex)
                {
                    throw new BadRequestException($"Lỗi Firebase Storage trong get image: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Lỗi ở hàm get image: {ex.Message}");
            }
        }

        public async Task<bool> DeleteImageAsync(Guid imageName)
        {
            try
            {
                var imageInDb = await _unitOfWork.GetImageRepository().GetImageAsync(imageName, 0);
                if (imageInDb is not null)
                {
                    await _unitOfWork.GetImageRepository().DeleteAsync(imageInDb);
                }

                try
                {
                    var storage = GetFirebaseStorage();

                    await storage
                        .Child("images")
                        .Child(imageName.ToString())
                        .DeleteAsync();

                    await _unitOfWork.GetAccountRepository().SaveChangeAsync();
                    return true;
                }
                catch (FirebaseStorageException ex)
                {
                    throw new BadRequestException($"Lỗi Firebase Storage trong delete image: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Lỗi ở hàm delete image: {ex.Message}");
            }
        }
    }
}
