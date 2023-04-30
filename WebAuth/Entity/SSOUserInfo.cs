using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAuth.Entity
{
    /// <summary>
    /// SSO資訊
    /// </summary>
    public class SSOUserInfo
    {
        /// <summary> 學號/人員代號 </summary>
        public string Account { get; set; }

        /// <summary> 身份 </summary>
        public string Role { get; set; }

        /// <summary> 姓名 </summary>
        public string Name { get; set; }

        /// <summary> 系級/單位 </summary>
        public string Department { get; set; }
    }
}
