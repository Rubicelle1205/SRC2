using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebPccuClub.Global
{
    public class ValidateCodeUtil
    {
        /// <summary>
        /// 產生驗證碼
        /// </summary>
        /// <param name="Length"> 長度 </param>
        /// <param name="Mode"> 1:數字、2:英文(含大小寫)、3:數字+英文(含大小寫) </param>
        /// <returns> 驗證碼文字 </returns>
        public static string GetValidCode(int Length, int Mode)
        {
            if (Length <= 0 || (Mode <= 0 || Mode > 3))
            { return string.Empty; }

            StringBuilder ResultStr = new StringBuilder();
            StringBuilder patterns = new StringBuilder();
            string PatternStr = string.Empty;
            Random Rnd = new Random();

            if (Mode == 1)
            { patterns.Append("0123456789"); }
            else if (Mode == 2)
            { patterns.Append("ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijklmnpqrstuvwxyz"); }
            else
            { patterns.Append("ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijklmnpqrstuvwxyz0123456789"); }

            PatternStr = patterns.ToString();

            for (int i = 0; i < Length; i++)
            {
                ResultStr.Append(PatternStr.Substring(Rnd.Next(0, PatternStr.Length - 1), 1));
            }

            return ResultStr.ToString();
        }

        /// <summary> 產生驗證碼圖片 </summary>
        public static byte[] GetImageByte(string ValidCode)
        {
            if (string.IsNullOrEmpty(ValidCode.Trim()))
            { return null; }

            int FontSize = 60;
            int FontSpacing = 50;
            int width = (int)Math.Ceiling((ValidCode.Length * (FontSize + 0.7)));
            int height = FontSize + 10;

            Bitmap thisMap = new Bitmap(width, height);
            Graphics graph = Graphics.FromImage(thisMap);
            Font font = new Font(FontFamily.GenericSerif, FontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            MemoryStream stream = new MemoryStream();
            Random Rnd = new Random();
            graph.Clear(Color.White);

            SolidBrush brush = new SolidBrush(Color.Black);
            for (int i = 0; i < ValidCode.Length; i++)
            {
                graph.DrawString(ValidCode.Substring(i, 1), font, brush, i * FontSpacing + 20, Rnd.Next(20));
            }

            Pen linePen = new Pen(new SolidBrush(Color.Silver), 2);
            for (int x = 0; x < 10; x++)
            {
                graph.DrawLine(linePen, new Point(Rnd.Next(0, 199), Rnd.Next(0, 59)), new Point(Rnd.Next(0, 199), Rnd.Next(0, 59)));
            }

            thisMap.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }

        //檢查身分證字號是否符合規則
        /// <summary>
        /// 檢查身分證字號是否符合規則
        /// </summary>
        /// <param name="iIDNO"></param>
        /// <returns>True:合規則，False:不合規則</returns>
        public bool isIDNo(string IDNo)
        {
#if DEBUG
            return true;
#endif

            if (string.IsNullOrWhiteSpace(IDNo) || IDNo.Length != 10)
            {
                return false;
            }

            char[] pidCharArray = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            IDNo = IDNo.ToUpper();
            char[] strArr = IDNo.ToCharArray();
            int verifyNum = 0;

            // 檢查身分證字號
            if (Regex.IsMatch(IDNo, @"[A-Z]{1}[1-2]{1}[0-9]{8}"))
            {
                // 原身分證英文字應轉換為10~33，這裡直接作個位數*9+10
                int[] pidIDInt = { 1, 10, 19, 28, 37, 46, 55, 64, 39, 73, 82, 2, 11, 20, 48, 29, 38, 47, 56, 65, 74, 83, 21, 3, 12, 30 };
                // 第一碼
                verifyNum = verifyNum + pidIDInt[Array.BinarySearch(pidCharArray, strArr[0])];
                // 第二~九碼
                for (int i = 1, j = 8; i < 9; i++, j--)
                {
                    verifyNum += Convert.ToInt32(strArr[i].ToString(), 10) * j;
                }
                // 檢查碼
                verifyNum = (10 - (verifyNum % 10)) % 10;
                bool isValid = verifyNum == Convert.ToInt32(strArr[9].ToString(), 10);
                if (isValid)
                {
                    return true;
                }

            }

            // 舊式居留證
            if (Regex.IsMatch(IDNo, @"[A-Z]{1}[A-D]{1}[0-9]{8}"))
            {
                // 原居留證第一碼英文字應轉換為10~33，十位數*1，個位數*9，這裡直接作[(十位數*1) mod 10] + [(個位數*9) mod 10]
                int[] pidResidentFirstInt = { 1, 10, 9, 8, 7, 6, 5, 4, 9, 3, 2, 2, 11, 10, 8, 9, 8, 7, 6, 5, 4, 3, 11, 3, 12, 10 };
                // 第一碼
                verifyNum += pidResidentFirstInt[Array.BinarySearch(pidCharArray, strArr[0])];
                // 原居留證第二碼英文字應轉換為10~33，並僅取個位數*6，這裡直接取[(個位數*6) mod 10]
                int[] pidResidentSecondInt = { 0, 8, 6, 4, 2, 0, 8, 6, 2, 4, 2, 0, 8, 6, 0, 4, 2, 0, 8, 6, 4, 2, 6, 0, 8, 4 };
                // 第二碼
                verifyNum += pidResidentSecondInt[Array.BinarySearch(pidCharArray, strArr[1])];
                // 第三~八碼
                for (int i = 2, j = 7; i < 9; i++, j--)
                {
                    verifyNum += Convert.ToInt32(strArr[i].ToString(), 10) * j;
                }
                // 檢查碼
                verifyNum = (10 - (verifyNum % 10)) % 10;
                bool isValid = verifyNum == Convert.ToInt32(strArr[9].ToString(), 10);
                if (isValid)
                {
                    return true;
                }

            }

            // 新式居留證
            if (Regex.IsMatch(IDNo, @"[A-Z]{1}[8-9]{1}[0-9]{8}"))
            {
                int[] pidResidentFirstInt = { 1, 10, 9, 8, 7, 6, 5, 4, 9, 3, 2, 2, 11, 10, 8, 9, 8, 7, 6, 5, 4, 3, 11, 3, 12, 10 };
                // 第一碼
                verifyNum += pidResidentFirstInt[Array.BinarySearch(pidCharArray, strArr[0])];
                // 第二~八碼
                for (int i = 1, j = 8; i < 9; i++, j--)
                {
                    verifyNum += Convert.ToInt32(strArr[i].ToString(), 10) * j;
                }
                // 檢查碼
                verifyNum = (10 - (verifyNum % 10)) % 10;

                bool isValid = verifyNum == Convert.ToInt32(strArr[9].ToString(), 10);
                if (isValid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 驗證 E-Mail格式
        /// </summary>
        /// <param name="email">E-Mail string</param>
        /// <returns>True:合規則，False:不合規則</returns>
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return Regex.IsMatch(email, @"(@)(.+)$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// 驗證 電話格是格式(ex. 06-2353535#5222，0965-123456)
        /// </summary>
        /// <param name="email">電話 string</param>
        /// <returns>True:合規則，False:不合規則</returns>
        public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            return Regex.IsMatch(phone, @"^\d{2}-\d{7}.+$|^\d{4}-\d{6}$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

        }
    }
}
