using NPOI.HPSF;
using PccuClub.WebAuth;

namespace WebPccuClub.Global
{
    public class UploadUtil
    {
        AuthManager auth = new AuthManager();

        public async Task<string> UploadFileAsync(string PathName, IFormFile? file)
        {
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), string.Format("Upload\\{0}", PathName.Replace("Path", "")), dateFolder);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            // 產生加密的檔案名稱
            string encryptedFileName = String.Empty;
            if (!string.IsNullOrEmpty(file.FileName))
                encryptedFileName = auth.EncryptionText(Path.GetFileNameWithoutExtension(file.FileName));

            encryptedFileName = encryptedFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

            // 組合檔案儲存路徑
             string savePath = Path.Combine(uploadPath, encryptedFileName + fileExtension);

            // 儲存檔案
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 取得相對路徑
            string relativePath = $"../Upload/{PathName.Replace("Path", "")}/{dateFolder}/{encryptedFileName}{fileExtension}";

            // 回傳上傳的目錄字串
            return relativePath;
        }
    }
}
