namespace Simple.KYC.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Simple.KYC.Services;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly FaceDetectionClient _faceDetectionClient;
        public UploadController(FaceDetectionClient client)
        {
            _faceDetectionClient = client;
        }
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            if (Request.Form.Files != null && Request.Form.Files.Count >= 1)
            {
                var photo = Request.Form.Files[0];
                var result = await _faceDetectionClient.DetectFace(photo.OpenReadStream());

                return Ok(result);
            }
            return Ok();
        }
    }
}
