using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAuth.Entity
{
    public class FunInfo
    {
        /// <summary> 選單節點 </summary>
        public string MenuNode { get; set; }

        /// <summary> 選單名稱 </summary>
        public string MenuName { get; set; }

        /// <summary> 上層選單節點 </summary>
        public string MenuUpNode { get; set; }

        /// <summary> 程式流水號 </summary>
        public string FunId { get; set; }

        /// <summary> 功能名稱 </summary>
        public string FunName { get; set; }

        /// <summary> 功能網址 </summary>
        public string Url { get; set; }

        /// <summary> 網頁圖示 </summary>
        public string IconTag { get; set; }

        /// <summary> 是否啟用(False:不啟用, True:啟用)  </summary>
        public bool IsEnable { get; set; }

        /// <summary> 是否顯示(False:不啟用, True:啟用)  </summary>
        public bool IsVisIble { get; set; }

        /// <summary> 排序 </summary>
        public int SortOrder { get; set; }
    }
}
