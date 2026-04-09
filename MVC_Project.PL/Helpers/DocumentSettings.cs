namespace MVC_Project.PL.Helpers
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile? file, string folderName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName);
            string fileName = $"{Guid.NewGuid()}-{file?.FileName}";
            string filePath = Path.Combine(folderPath, fileName);

            using(var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file?.CopyTo(fileStream);
            }
            return fileName;
        }

        public static void DeleteFile(string fileName, string folderName)
        {
            if(fileName is not null && folderName is not null)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName, fileName);
                if (File.Exists(folderPath))
                    File.Delete(folderPath);
            }
        }
    }
}
