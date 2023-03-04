using System.Data;
using System.Reflection;

namespace DataAccess
{
    public static class DBATool
    {
        /// <summary> 將 DataTable 轉換成對應的 Entity 集合 </summary>
        public static IEnumerable<T> DataTableToEntities<T>(DataTable dt, bool IgnoreCase)
        {
            foreach (DataRow row in dt.Rows)
            {
                T result = Activator.CreateInstance<T>();
                foreach (DataColumn column in dt.Columns)
                {
                    try
                    {
                        PropertyInfo pinfo = null;

                        if (IgnoreCase)
                        { pinfo = typeof(T).GetProperty(column.ColumnName.Trim(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance); }
                        else
                        { pinfo = typeof(T).GetProperty(column.ColumnName.Trim()); }

                        if (pinfo == null)
                        { continue; }

                        Type thisType = Nullable.GetUnderlyingType(pinfo.PropertyType) ?? pinfo.PropertyType;
                        object Value = row[column.ColumnName].DbNullToNull();
                        object resultValue = (Value == null) ? null : (Value.GetType() == typeof(String) && string.IsNullOrEmpty(Value.ToString())) ? null : Convert.ChangeType(Value, thisType);
                        pinfo.SetValue(result, resultValue, null);

                    }
                    catch
                    {
                        continue;
                    }
                }
                yield return result;
            }
        }

        /// <summary> 將 特定一筆 DataTable Row 轉換成對應的 Entity </summary>
        public static T DataRowToEntity<T>(DataRow dr, bool IgnoreCase)
        {
            T result = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                try
                {
                    PropertyInfo pinfo = null;

                    if (IgnoreCase)
                    { pinfo = typeof(T).GetProperty(column.ColumnName.Trim(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance); }
                    else
                    { pinfo = typeof(T).GetProperty(column.ColumnName.Trim()); }

                    if (pinfo == null)
                    { continue; }

                    Type thisType = Nullable.GetUnderlyingType(pinfo.PropertyType) ?? pinfo.PropertyType;
                    object Value = dr[column.ColumnName].DbNullToNull();
                    object resultValue = (Value == null) ? null : Convert.ChangeType(Value, thisType);
                    pinfo.SetValue(result, resultValue, null);

                }
                catch
                {
                    continue;
                }
            }
            return result;
        }

        /// <summary> Object 轉換 </summary>
        public static T CastObject<T>(Object myobj, bool IgnoreCase)
        {
            Type source = myobj.GetType();
            Type target = typeof(T);
            T result = Activator.CreateInstance<T>();

            foreach (MemberInfo SourceInfo in source.GetMembers())
            {
                try
                {
                    PropertyInfo pinfo = null;

                    if (IgnoreCase)
                    { pinfo = target.GetProperty(SourceInfo.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance); }
                    else
                    { pinfo = target.GetProperty(SourceInfo.Name); }

                    if (pinfo == null)
                    { continue; }

                    Type thisType = Nullable.GetUnderlyingType(pinfo.PropertyType) ?? pinfo.PropertyType;
                    object thisValue = ((PropertyInfo)SourceInfo).GetValue(myobj);
                    object resultValue = (thisValue == null) ? null : Convert.ChangeType(thisValue, thisType);
                    pinfo.SetValue(result, resultValue, null);
                }
                catch
                {
                    continue;
                }
            }
            return (T)result;
        }

        /// <summary> 若值為DBNull.Value, 則轉為Null </summary>
        public static object DbNullToNull(this object original)
        {
            return original == DBNull.Value ? null : original;
        }
    }
}
