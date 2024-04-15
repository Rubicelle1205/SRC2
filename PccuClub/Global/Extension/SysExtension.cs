using DataAccess;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using PccuClub.WebAuth;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace WebPccuClub.Global.Extension
{
    public static class SysExtension
    {

        /// <summary> 使用者是否為管理者角色 </summary>
        public static bool isSupervisor(this UserInfo user)
            => user.UserType == "03";

        /// <summary> DataSet 是否有資料 (無資料:true，有:False) </summary>
        public static bool IsNullOrEmpty(this DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            { return true; }

            return false;
        }

        /// <summary> DataTable 是否有資料  (無資料:true，有:False) </summary>
        public static bool IsNullOrEmpty(this DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            { return true; }

            return false;
        }

        /// <summary> 將DataTable轉換成對應的Entity集合 (不分大小寫) </summary>
        public static IEnumerable<T> DataTableToEntities<T>(this DataTable dt)
        {
            return DBATool.DataTableToEntities<T>(dt, true);
        }

        /// <summary> 將DataTable轉換成對應的Entity集合 (設定是否分大小寫) </summary>
        public static IEnumerable<T> DataTableToEntities<T>(this DataTable dt, bool IgnoreCase)
        {
            return DBATool.DataTableToEntities<T>(dt, IgnoreCase);
        }

        /// <summary> 將DataTable Row 轉換成對應的Entity (設定是否分大小寫) </summary>
        public static T DataRowToEntity<T>(this DataRow dr, bool IgnoreCase = true)
        {
            return DBATool.DataRowToEntity<T>(dr, IgnoreCase);
        }

        /// <summary> 將Entity集合轉成DataTable (設定是否分大小寫) </summary>
        public static DataTable EntityToDataTable<T>(this IEnumerable<T> Entity)
        {
            DataTable dt = new DataTable();
            var prop = typeof(T).GetProperties();
            dt.Columns.AddRange(prop.Select(p => new DataColumn(p.Name, typeof(string))).ToArray());
            Entity.ToList().ForEach(t =>
            {
                ArrayList temp = new ArrayList();
                prop.ToList().ForEach(p =>
                {
                    var obj = p.GetValue(t, null);
                    temp.Add(obj);
                });
                dt.LoadDataRow(temp.ToArray(), true);
            });
            return dt;
        }

        /// <summary> 將一個Entity轉成DataTable (設定是否分大小寫) </summary>
        public static DataTable OneEntityToDataTable<T>(this T Entity) where T : class
        {
            List<T> entityList = new List<T> { Entity };
            return entityList.EntityToDataTable();
        }

        /// <summary> Object 轉換 (不分大小寫) </summary>
        public static T ConvertObject<T>(this object thisObject)
        {
            return DBATool.CastObject<T>(thisObject, true);
        }

        /// <summary> Object 轉換 (設定是否分大小寫，True:不分大小寫，False:分大小寫) </summary>
        public static T ConvertObject<T>(this object thisObject, bool IgnoreCase)
        {
            return DBATool.CastObject<T>(thisObject, IgnoreCase);
        }

        /// <summary> List Object 轉換 (不分大小寫) </summary>
        public static IEnumerable<T> ConvertObjectList<T>(this IEnumerable<object> thisList)
        {
            List<T> resultList = new List<T>();
            foreach (object thisObj in thisList)
            {
                T resultObj = DBATool.CastObject<T>(thisObj, true);
                resultList.Add(resultObj);
            }
            return resultList;
        }

        /// <summary> List Object 轉換 (設定是否分大小寫，True:不分大小寫，False:分大小寫) </summary>
        public static IEnumerable<T> ConvertObjectList<T>(this IEnumerable<object> thisList, bool IgnoreCase)
        {
            List<T> resultList = new List<T>();
            foreach (object thisObj in thisList)
            {
                T resultObj = DBATool.CastObject<T>(thisObj, IgnoreCase);
                resultList.Add(resultObj);
            }
            return resultList;
        }


        /// <summary> Object 轉換 </summary>
        public static Object TrimObjectValue(Object myobj)
        {
            Type source = myobj.GetType();
            foreach (PropertyInfo pinfo in source.GetProperties())
            {
                Type thisType = Nullable.GetUnderlyingType(pinfo.PropertyType) ?? pinfo.PropertyType;
                if (thisType == typeof(string))
                {
                    object thisValue = pinfo.GetValue(myobj);
                    if (thisValue != null)
                    {
                        string trimValue = thisValue.ToString().Trim();
                        pinfo.SetValue(myobj, trimValue, null);
                    }
                }
            }

            return myobj;
        }

        /// <summary>
        /// ObjectValue Mapping - 不分大小寫
        /// </summary>
        /// <param name="tragetObj">目的 Object</param>
        /// <param name="sourceObj">來源 Object</param>
        /// <param name="isReplace">是否取代目的 Object值(True:取代，False:不取代)</param>
        /// <returns></returns>
        public static Object ValueMapping(Object tragetObj, Object sourceObj, bool isReplace = false)
        {
            return ValueMapping(tragetObj, sourceObj, isReplace, isReplace);
        }

        /// <summary>
        /// ObjectValue Mapping
        /// </summary>
        /// <param name="tragetObj">目的 Object</param>
        /// <param name="sourceObj">來源 Object</param>
        /// <param name="isReplace">是否取代目的 Object值(True:取代，False:不取代)</param>
        /// <param name="IgnoreCase">是否不分大小寫(True:不分大小寫，False:分大小寫)</param>
        /// <returns></returns>
        public static Object ValueMapping(Object tragetObj, Object sourceObj, bool isReplace, bool IgnoreCase)
        {
            Type source = sourceObj.GetType();
            Type target = tragetObj.GetType();

            foreach (PropertyInfo TargetInfo in target.GetProperties())
            {
                try
                {

                    PropertyInfo TargetPinfo = null;
                    PropertyInfo SourcePinfo = null;
                    string TarName = TargetInfo.Name;
                    if (IgnoreCase)
                    {
                        TargetPinfo = target.GetProperty(TarName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        SourcePinfo = sourceObj.GetType().GetProperty(TarName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    }
                    else
                    {
                        TargetPinfo = target.GetProperty(TarName);
                        SourcePinfo = sourceObj.GetType().GetProperty(TarName);
                    }

                    if (TargetPinfo == null || SourcePinfo == null)
                    { continue; }


                    Type TargetType = Nullable.GetUnderlyingType(TargetPinfo.PropertyType) ?? TargetPinfo.PropertyType;
                    object SourceValue = SourcePinfo.GetValue(sourceObj);
                    object TargetValue = TargetPinfo.GetValue(tragetObj);

                    if (TargetValue == null || isReplace || (typeof(IEnumerable).IsAssignableFrom(TargetType) && ((IList)TargetValue).Count == 0))
                    {
                        object resultValue = (SourceValue == null) ? null : Convert.ChangeType(SourceValue, TargetType);
                        TargetPinfo.SetValue(tragetObj, resultValue, null);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return tragetObj;
        }

        /// <summary> 根據 ViewModel DisplayName 重新命名 DataTable Column Name - 不分大小寫 </summary>
        public static void ColumnReName<T>(this DataTable thisDT)
        {
            foreach (MemberInfo thisProp in typeof(T).GetProperties())
            {
                var thisAttr = thisProp.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();

                if (thisAttr == null)
                { continue; }

                string thisPropName = thisProp.Name.ToUpper();
                string thisDisplayName = ((DisplayNameAttribute)thisAttr).DisplayName.ToUpper();

                if (thisDT.Columns.Contains(thisDisplayName))
                {
                    thisDT.Columns[thisDisplayName].ColumnName = thisPropName;
                }
            }
        }

        /// <summary> 根據 ViewModel DisplayName 重新命名 DataTable Column Name - 英文轉中文 </summary>
        public static void ColumnReNameChinese<T>(this DataTable thisDT)
        {
            foreach (MemberInfo thisProp in typeof(T).GetProperties())
            {
                var thisAttr = thisProp.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();

                if (thisAttr == null)
                { continue; }

                string thisPropName = thisProp.Name.ToUpper();
                string thisDisplayName = ((DisplayNameAttribute)thisAttr).DisplayName.ToUpper();

                if (thisDT.Columns.Contains(thisPropName))
                {
                    thisDT.Columns[thisPropName].ColumnName = thisDisplayName;
                }
            }
        }

        /// <summary>
        /// Model轉換RouteValueDictionary
        /// </summary>
        /// <param name="PrefixStr">RouteValueDictionary Key 前置字串</param>
        /// <param name="Model">ViewModel</param>
        /// <returns>RouteValueDictionary</returns>
        public static RouteValueDictionary ConvertModelToRouteValueDictionary(string PrefixStr, Object Model)
        {
            RouteValueDictionary ResultDic = new RouteValueDictionary();
            Type source = Model.GetType();

            foreach (System.Reflection.MemberInfo SourceInfo in source.GetProperties())
            {
                try
                {
                    System.Reflection.PropertyInfo pinfo = null;
                    string PropName = string.Empty;

                    pinfo = source.GetProperty(SourceInfo.Name, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                    object PropValue = ((System.Reflection.PropertyInfo)SourceInfo).GetValue(Model);

                    if (pinfo == null || PropValue == null)
                    { continue; }

                    PropName = pinfo.Name;
                    Type PropType = Nullable.GetUnderlyingType(pinfo.PropertyType) ?? pinfo.PropertyType;

                    if (Type.GetTypeCode(PropType).Equals(TypeCode.Object) && !Type.GetTypeCode(PropValue.GetType()).Equals(TypeCode.String))
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(PropType))
                        {
                            string EnumName = string.Empty;
                            int Index = 0;
                            foreach (var element in (IEnumerable)PropValue)
                            {
                                TypeCode EnumType = Type.GetTypeCode(element.GetType());

                                if (!string.IsNullOrEmpty(PrefixStr))
                                { EnumName = string.Format(@"{0}.{1}[{2}]", PrefixStr, PropName, Index); }
                                else
                                { EnumName = string.Format(@"{0}[{1}]", PropName, Index); }

                                if (EnumType.Equals(TypeCode.Object) && !EnumType.Equals(TypeCode.String))
                                {
                                    RouteValueDictionary PropertyDic = ConvertModelToRouteValueDictionary(EnumName, element);
                                    PropertyDic.ToList().ForEach(x => ResultDic.Add(x.Key, x.Value));
                                }
                                else
                                {
                                    ResultDic.Add(EnumName, element);
                                }
                                Index++;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(PrefixStr))
                            { PropName = string.Format(@"{0}.{1}", PrefixStr, PropName); }

                            RouteValueDictionary PropertyDic = ConvertModelToRouteValueDictionary(PropName, PropValue);
                            PropertyDic.ToList().ForEach(x => ResultDic.Add(x.Key, x.Value));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(PrefixStr))
                        { PropName = string.Format(@"{0}.{1}", PrefixStr, PropName); }

                        ResultDic.Add(PropName, PropValue);
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return ResultDic;
        }

        /// <summary>
        /// 建立錯誤訊息
        /// </summary>
        /// <typeparam name="TModel">ViewModel</typeparam>
        /// <param name="expression">Lambda 運算式</param>
        /// <param name="ErrorMsg">錯誤訊息</param>
        /// <returns>KeyValuePair<ModelStageKey, Error Message></returns>
        public static KeyValuePair<string, string> GetModelError<TModel>(Expression<Func<TModel, object>> expression, string ErrorMsg)
        {
            string expBody = ((LambdaExpression)expression).Body.ToString();
            string paramName = expression.Parameters[0].Name;
            string paramTypeName = expression.Parameters[0].Type.Name;

            MatchCollection mArray = Regex.Matches(expBody, @"\.get_Item\(\d+\)");
            foreach (Match match in mArray)
            {
                string ReStr = match.Value.Replace(".get_Item(", "[").Replace(")", "]");
                expBody = expBody.Replace(match.Value, ReStr);
            }

            MatchCollection mElemAt = Regex.Matches(expBody, @"\.ElementAt\(\d+\)");
            foreach (Match match in mElemAt)
            {
                string ReStr = match.Value.Replace(".ElementAt(", "[").Replace(")", "]");
                expBody = expBody.Replace(match.Value, ReStr);
            }

            MatchCollection mConvert = Regex.Matches(expBody, @"Convert\(.+\)");
            foreach (Match match in mConvert)
            {
                string ReStr = match.Value.Replace(@"Convert(", string.Empty).Replace(")", string.Empty);
                expBody = expBody.Replace(match.Value, ReStr);
            }

            string ErrorKry = expBody.Replace(paramName + ".", paramTypeName + ".");
            return new KeyValuePair<string, string>(ErrorKry, ErrorMsg);
        }

        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary> HtmlContent to String  </summary>
        /// <param name="content">HtmlContent</param>
        /// <returns>String</returns>
        public static HtmlString ToHtmlString(this IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return new HtmlString(writer.ToString());
            }
        }

        public static string GetExpressionText<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
        {
            var expressionProvider = htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(ModelExpressionProvider)) as ModelExpressionProvider;

            return expressionProvider.GetExpressionText(expression);
        }

        public static string[] DataSetToStringArray(this DataSet ds, string ColumnName)
        {
            List<string> results = new List<string>();

            foreach (DataTable table in ds.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (table.Columns.Contains(ColumnName) && row[ColumnName] != DBNull.Value)
                    {
                        results.Add(row[ColumnName].ToString());
                    }
                }
            }

            return results.ToArray();
        }
    }
}
