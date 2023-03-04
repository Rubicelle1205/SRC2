using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataAccess
{
    public class DBAParameter
    {
        private DynamicParameters _parameters;

        public DBAParameter()
        {
            this._parameters = new DynamicParameters();
        }

        /// <summary>
        /// 新增一個資料庫參數
        /// </summary>
        /// <param name="key">參數名稱</param>
        /// <param name="content">參數的內容</param>
        public void Add(string key, object content)
        {
            _parameters.Add(key, content);
        }

        /// <summary> 取得所有內容(Dictionary) </summary>
        public DynamicParameters GetInstance()
        {
            return _parameters;
        }
    }
}
