using System.Text;

namespace ML_WebApp.Services
{
    public class FileHelper : IFile
    {
        private readonly IWebHostEnvironment _env;

        public FileHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string SaveImageFile(IFormFile file)
        {

            if (file == null)
                return string.Empty;

            string fileName = Path.GetFileName(file.FileName);
            string filePath = Path.Combine(_env.WebRootPath, "Media", fileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return "/Media/" + fileName;
        }

        public string SaveAudioFile(IFormFile file)
        {

            if (file == null)
                return string.Empty;

            string fileName = Path.GetFileName(file.FileName);
            string filePath = Path.Combine(_env.WebRootPath, "audio", fileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return "/audio/" + fileName;
        }


        public IFormFile ConvertStringToIFormFile(string content, string fileName, string contentType = "text/plain")
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);

            return new FormFile(stream, 0, bytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        public IFormFile? LoadFileAsFormFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return null;

            string fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (!File.Exists(fullPath)) return null;

            string fileName = Path.GetFileName(fullPath);
            string content = File.ReadAllText(fullPath); 
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);

            return new FormFile(stream, 0, bytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = GetMimeTypeFromExtension(fileName)
            };
        }

        public string GetMimeTypeFromExtension(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLowerInvariant();

            return ext switch
            {
                // Images
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".svg" => "image/svg+xml",

                // Audio
                ".mp3" => "audio/mpeg",
                ".wav" => "audio/wav",
                ".ogg" => "audio/ogg",
                ".aac" => "audio/aac",
                ".flac" => "audio/flac",
                ".m4a" => "audio/mp4",
                ".wma" => "audio/x-ms-wma",
                ".aiff" or ".aif" => "audio/aiff",

                // Default
                _ => "application/octet-stream"
            };
        }
    }
}
