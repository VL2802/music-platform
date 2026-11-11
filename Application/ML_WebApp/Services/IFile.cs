namespace ML_WebApp.Services
{
    public interface IFile
    {

        string SaveImageFile(IFormFile file);
        string SaveAudioFile(IFormFile file);

        IFormFile ConvertStringToIFormFile(string content, string fileName, string contentType = "text/plain");
        string GetMimeTypeFromExtension(string fileName);
        IFormFile? LoadFileAsFormFile(string relativePath);

    }
}
