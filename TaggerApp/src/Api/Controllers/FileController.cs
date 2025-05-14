using HeyRed.Mime;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase {
        private const string _fileFolderPath = "D:\\GitRepos\\tagger\\TaggerApp\\Api\\Resources\\Example";

        [HttpGet]
        public IActionResult GetFilePaths(int startIndex = 0, int count = 20) {
            var images = Directory.GetFiles(_fileFolderPath)
                .Skip(startIndex)
                .Take(count)
                .Select(Path.GetFileName)
                .ToList();

            return Ok(images);
        }

        [HttpGet]
        [Route("absolute")]
        public IActionResult GetFilePathsAbsolute(int startIndex = 0, int count = 20) {
            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/file";
            var rng = new Random();
            var allFiles = Directory.EnumerateFiles(_fileFolderPath);
            if (startIndex >= allFiles.Count()) {
                return Ok(new List<string>());
            }

            var files = allFiles
                .OrderBy(_ => rng.Next())
                .Skip(startIndex)
                .Take(count)
                .Select(file => $"{baseUrl}/{Path.GetFileName(file)}")
                .ToList();

            /*var images = Directory.GetFiles(_fileFolderPath)
                .Skip(startIndex)
                .Take(count)
                .Select(file => $"{baseUrl}/{Path.GetFileName(file)}")
                .ToList();*/

            return Ok(files);
        }

        [HttpGet("{fileName}")]
        public IActionResult GetFile(string fileName) {
            var filePath = Path.Combine(_fileFolderPath, fileName);

            if (!System.IO.File.Exists(filePath)) {
                return NotFound();
            }

            var file = System.IO.File.OpenRead(filePath);
            var contentType = MimeTypesMap.GetMimeType(fileName);

            return File(file, contentType);
        }
    }
}
