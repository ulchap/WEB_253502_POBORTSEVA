using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB_253502_POBORTSEVA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _imagePath;

        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "Images");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file is null)
            {
                return BadRequest();
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(_imagePath, uniqueFileName);
            var fileInfo = new FileInfo(filePath);

            using (var fileStream = fileInfo.Create())
            {
                await file.CopyToAsync(fileStream);
            }

            var host = HttpContext.Request.Host;
            var fileUrl = $"https://{host}/Images/{uniqueFileName}";

            return Ok(fileUrl);
        }

        [HttpDelete]
        public IActionResult DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return Ok();
            }

            return NotFound();
        }
    }

}
