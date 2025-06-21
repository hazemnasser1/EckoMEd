using Microsoft.AspNetCore.Mvc;

namespace Echomedproject.PL.Helpers
{
    public class DocumentSetting
    {

        public static string UploadImage(IFormFile file, string FolderName)
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            string FilePath = Path.Combine(FolderPath, fileName);

            var fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(fs);
            return fileName;

        }
        public static string GetBase64Image(string fileName, string folderName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName);
            string filePath = Path.Combine(folderPath, fileName);

            if (!System.IO.File.Exists(filePath))
                return null; // Or return a placeholder base64

            byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
            return Convert.ToBase64String(imageBytes);
        }
    }
}
