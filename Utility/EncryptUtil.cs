using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    public class EncryptUtil
    {
        private string strIV = "Pccu";
        private string strKey = string.Empty;

        public EncryptUtil()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(@"appsettings.json");
            IConfiguration config = builder.Build();
            this.strKey = config.GetValue<string>("EncryptUtilKey");
        }

        public EncryptUtil(string strKey)
        {
            this.strKey = strKey;
        }

        public EncryptUtil(string IV, string Key)
        {
            this.strIV = IV;
            this.strKey = Key;
        }

        /// <summary> 加密 </summary>
        public string Encrypt(string EncryptStr)
        {
            try
            {

                if (string.IsNullOrEmpty(strIV) || string.IsNullOrEmpty(strKey) || string.IsNullOrEmpty(EncryptStr))
                { return string.Empty; }

                byte[] Key = Encoding.UTF8.GetBytes(strKey);
                byte[] IV = Encoding.UTF8.GetBytes(strIV);
                SHA256CryptoServiceProvider sha = new SHA256CryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                RijndaelManaged ProviedAes = new RijndaelManaged();
                ICryptoTransform CryptoAES = ProviedAes.CreateEncryptor(sha.ComputeHash(Key), md5.ComputeHash(IV));

                byte[] DataBuffer = Encoding.UTF8.GetBytes(EncryptStr);
                string encrypt = string.Empty;

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, CryptoAES, CryptoStreamMode.Write))
                {
                    cs.Write(DataBuffer, 0, DataBuffer.Length);
                    cs.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(ms.ToArray());

                }

                return encrypt;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary> 解密 </summary>
        public string Decrypt(string DecryptStr)
        {
            try
            {
                if (string.IsNullOrEmpty(strIV) || string.IsNullOrEmpty(strKey) || string.IsNullOrEmpty(DecryptStr))
                { return string.Empty; }

                byte[] Key = Encoding.UTF8.GetBytes(strKey);
                byte[] IV = Encoding.UTF8.GetBytes(strIV);
                SHA256CryptoServiceProvider sha = new SHA256CryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                RijndaelManaged ProviedAes = new RijndaelManaged();
                ICryptoTransform CryptoAES = ProviedAes.CreateDecryptor(sha.ComputeHash(Key), md5.ComputeHash(IV));

                byte[] DataBuffer = Convert.FromBase64String(DecryptStr);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, CryptoAES, CryptoStreamMode.Write))
                    {
                        cs.Write(DataBuffer, 0, DataBuffer.Length);
                        cs.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch
            {
                return string.Empty;
            }

        }

    }
}