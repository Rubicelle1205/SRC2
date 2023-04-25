//using Microsoft.International.Formatters;
using System.Globalization;
using System.Text.RegularExpressions;

namespace WebPccuClub.Global
{
    /// <summary> 共用Function </summary>
    public static class PublicFunction
    {
        #region 日期轉換

        #region 不含時間
        /// <summary>
        /// 西元年轉民國年 - 不含時間(回傳String) - EditorTemplates 專用
        /// </summary>
        /// <param name="oP_GRE">西元年字串(ex:2007/9/23)或DateTime物件</param>
        /// <returns>傳回 yyy/MM/dd 的格式</returns>
        public static string GRE2CNS(object oP_GRE)
        {
            try
            {
                // 雖然參考為0，但 EditorTemplates 有使用到
                if (oP_GRE == null)
                { return string.Empty; }

                DateTime dt = Convert.ToDateTime(oP_GRE);
                TaiwanCalendar twCalendar = new TaiwanCalendar();
                int tmpYear = twCalendar.GetYear(dt);
                string Year = (tmpYear > 1911 ? tmpYear - 1911 : tmpYear).ToString();
                string Month = twCalendar.GetMonth(dt).ToString().PadLeft(2, '0');
                string Day = dt.Day.ToString().PadLeft(2, '0');
                return $@"{Year}/{Month}/{Day}";
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 民國年轉西元年 - 不含時間
        /// </summary>
        /// <param name="sP_CNS">民國年字串 ex:96/8/8</param>
        /// <returns>傳回 yyyy/MM/dd 的格式</returns>
        public static string CNS2GRE(object sP_CNS)
        {
            try
            {
                return GetGREDateFromCNS(sP_CNS).Value.ToString("yyyy/MM/dd");
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 從民國年字串取得西元年DateTime物件 - 不含時間
        /// </summary>
        /// <param name="sP_CNS">民國年字串 ex:96/8/8</param>
        /// <returns>DateTime 物件</returns>
        public static DateTime? GetGREDateFromCNS(object sP_CNS)
        {
            try
            {
                if (sP_CNS == null)
                { return null; ; }

                CultureInfo tw = new CultureInfo("zh-TW");
                tw.DateTimeFormat.Calendar = new TaiwanCalendar();
                return DateTime.Parse(sP_CNS.ToString(), tw);

            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 從西元字串取得DateTime物件 - 不含時間
        /// </summary>
        /// <param name="sP_CNS">西元字串 ex:20220101</param>
        /// <returns>DateTime 物件</returns>
        public static DateTime? GetStringToDate(string sDate, string DateFormate = "yyyyMMdd")
        {
            try
            {
                if (sDate == null)
                { return null; }

                if (DateTime.TryParseExact(sDate, DateFormate, null, DateTimeStyles.None, out DateTime dt1))
                { return dt1; }

                return null;
            }
            catch
            {
                return null;
            }
        }

        #endregion 不含時間

        #region 含時間
        /// <summary>
        /// 從西元年字串取得西元年DateTime物件
        /// </summary>
        /// <param name="sP_CNS">西元年字串 ex:20100601</param>
        /// <returns>DateTime 物件</returns>
        public static DateTime? GetGREDateTimeFromGREString(string s_GRE)
        {
            try
            {
                if (DateTime.TryParse(s_GRE, out DateTime dt))
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 西元年 DateTime 轉民國年字串(含時間) - DateTime.Value.Year=西元年時用
        /// </summary>
        public static string DateTimeTransformCNSD(DateTime? thisDateTime)
        {
            try
            {
                if (thisDateTime == null)
                { return null; }

                TaiwanCalendar twCalendar = new TaiwanCalendar();
                string Year = twCalendar.GetYear(thisDateTime.Value).ToString();
                string Month = twCalendar.GetMonth(thisDateTime.Value).ToString().PadLeft(2, '0');
                string Day = thisDateTime.Value.Day.ToString().PadLeft(2, '0');
                string Hour = thisDateTime.Value.Hour.ToString().PadLeft(2, '0');
                string Minute = thisDateTime.Value.Minute.ToString().PadLeft(2, '0');

                return $@"{Year}/{Month}/{Day} {Hour}:{Minute}";
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 民國年轉西元年DateTime物件(含時間) - DateTime.Value.Year=民國年時用
        /// </summary>
        public static DateTime? DateTimeTransformGRED(object sP_CNS)
        {
            return GetGREDateFromCNS(sP_CNS);
        }

        #endregion 含時間

        /// <summary> 取得該日期的最大時間(時:分:秒) </summary>
        public static DateTime? GetDateTimeMaxTime(DateTime? dt)
        {
            try
            { return new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day, 23, 59, 59); }
            catch
            { return null; }
        }

        /// <summary> 取得該日期的最小時間(時:分:秒) </summary>
        public static DateTime? GetDateTimeMinTime(DateTime? dt)
        {
            try
            { return new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day, 0, 0, 0); }
            catch
            { return null; }
        }

        /// <summary> 取得該日期的最大時間(秒) </summary>
        public static DateTime? GetDateTimeMaxSecond(DateTime? dt)
        {
            try
            { return new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day, dt.Value.Hour, dt.Value.Minute, 59); }
            catch
            { return null; }
        }

        /// <summary> 取得該日期的最小時間(秒) </summary>
        public static DateTime? GetDateTimeMinSecond(DateTime? dt)
        {
            try
            { return new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day, dt.Value.Hour, dt.Value.Minute, 0); }
            catch
            { return null; }
        }

        #endregion 日期轉換

        #region 文字驗證

        public static bool HasNumber(this string? str)
        {
            int c = 0;

            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    char[] ch = new char[str.Length];
                    ch = str.ToCharArray();
                    for (int i = 0; i < ch.Length; i++)
                    {
                        if (58 > ch[i] && ch[i] > 47)   //48 - 57
                            c = c + 1;
                    }

                    if (c > 0)
                        return true;
                    else
                        return false;
                }
                catch
                { return false; }
            }
            else { 
                return false; 
            }
        }

        public static bool HasUpperText(this string? str)
        {
            int c = 0;

            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    char[] ch = new char[str.Length];
                    ch = str.ToCharArray();
                    for (int i = 0; i < ch.Length; i++)
                    {
                        if (91 > ch[i] && ch[i] > 64)   //65 - 90
                            c = c + 1;
                    }

                    if (c > 0)
                        return true;
                    else
                        return false;
                }
                catch
                { return false; }
            }
            else
            {
                return false;
            }
        }

        public static bool HasLowerText(this string? str)
        {
            int c = 0;

            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    char[] ch = new char[str.Length];
                    ch = str.ToCharArray();
                    for (int i = 0; i < ch.Length; i++)
                    {
                        if (123 > ch[i] && ch[i] > 96)   //97 - 122
                            c = c + 1;
                    }

                    if (c > 0)
                        return true;
                    else
                        return false;
                }
                catch
                { return false; }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 字串處理

        public static string TrimStartAndEnd(this string? str)
        {
            string Restr = "";

            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    Restr = str.TrimStart().TrimEnd();
                    return Restr;
                }
                catch
                { return Restr; }
            }
            else
            {
                return Restr;
            }
        }

        

        #endregion
    }
}
