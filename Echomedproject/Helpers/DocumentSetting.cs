namespace Echomedproject.PL.Helpers
{
    public class DocumentSetting
    {

        public static string UploadImage(IFormFile file, string FolderName)
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            string FilePath=Path.Combine(FolderPath, fileName);
   
            var fs=new FileStream(FilePath, FileMode.Create);
            file.CopyTo(fs);
            return fileName;
           
        }
    }
}
