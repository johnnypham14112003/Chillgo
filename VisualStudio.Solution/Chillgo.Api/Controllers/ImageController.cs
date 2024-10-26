using Chillgo.Api.Models.Request;
using Chillgo.BusinessService.BusinessModels;
using Chillgo.BusinessService.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chillgo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        //=================================[ Declares ]================================
        private readonly IServiceFactory _serviceFactory;
        public ImageController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        //=================================[ Endpoints ]================================
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] RQ_ImageInfo imageData)
        {
            if (imageData.File == null || imageData.File.Length == 0)
                return BadRequest("No file provided.");

            //Convert File to Stream
            using (var stream = new MemoryStream())
            {
                await imageData.File.CopyToAsync(stream);
                stream.Position = 0;

                //Transfer stream into service
                string imageUrl = await _serviceFactory.GetFirebaseStorageService().UploadFileAsync(stream, imageData.FileName, imageData.Adapt<BM_Image>());
                return Ok(new
                {
                    Message = "Image uploaded and saved successfully",
                    ImageUrl = imageUrl
                });
            }
        }

        [Authorize]
        [HttpGet("{name}_{typeReference}")]
        public async Task<IActionResult> GetImageByAccountId([FromRoute]string name, byte typeReference)
        {
            // Gọi FirebaseStorageService để lấy URL của ảnh
            var imageUrl = await _serviceFactory.GetFirebaseStorageService().GetImageUrl(name, typeReference);
            return Ok(new { ImageUrl = imageUrl });
        }

        [Authorize]
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteImageByAccountId([FromRoute]string name)
        {
            var result = await _serviceFactory.GetFirebaseStorageService().DeleteImageAsync(name);
            return result ? Ok("Xóa thành công!") : BadRequest("Xóa thất bại!");
        }
    }
}
