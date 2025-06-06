﻿using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebAuth.Entity;
using WebPccuClub.Global.Extension;

namespace WebPccuClub.Models
{
    public class ConsultationRoleMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public ConsultationRoleMangConditionModel ConditionModel { get; set; }

        public List<ConsultationRoleMangResultModel> ResultModel { get; set; }

        public ConsultationRoleMangCreateModel CreateModel { get; set; }

        public ConsultationRoleMangEditModel EditModel { get; set; }
    }

    public class ConsultationRoleMangConditionModel
    {
        public ConsultationRoleMangConditionModel()
        {
            this.Page = 0;
            this.PageSize = 10;
            this.TotalCount = 0;
        }

        /// <summary> 目前頁數 </summary>
        public int Page { get; set; }

        /// <summary> 預設每頁顯示筆數 - 依需求更改 </summary>
        public int PageSize { get; set; }

        /// <summary> 總筆數 </summary>
        public int TotalCount { get; set; }

        /// <summary>角色ID</summary>
        [DisplayName("角色ID")]
        public string? RoleId { get; set; }

        /// <summary>角色</summary>
        [DisplayName("角色")]
        public string? RoleName { get; set; }

        /// <summary>權限	</summary>
        [DisplayName("權限")]
        public string? MenuNode { get; set; }

        [DisplayName("起始日期")]
        public DateTime? From_ReleaseDate { get; set; }

        [DisplayName("結束日期")]
        public DateTime? To_ReleaseDate { get; set; }
    }

    public class ConsultationRoleMangResultModel
    {
        /// <summary>角色ID</summary>
        [DisplayName("角色ID")]
        public string? RoleId { get; set; }

        /// <summary>角色名稱</summary>
        [DisplayName("角色名稱")]
        public string? RoleName { get; set; }
        
        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// 權限
        /// </summary>
        public List<FunInfo> LstFunInfo = new List<FunInfo>();
    }

    public class ConsultationRoleMangCreateModel
    {
        /// <summary>
        /// 已有權限
        /// </summary>
        public List<SelectListItem> LstFunItem = new List<SelectListItem>();

        /// <summary>
        /// 新權限
        /// </summary>
        public string? strFunInfo { get; set; }

        /// <summary>角色ID</summary>
        [DisplayName("角色ID")]
        public string? RoleId { get; set; }

        /// <summary>角色名稱</summary>
        [DisplayName("角色名稱")]
        public string? RoleName { get; set; }

        /// <summary>角色說明</summary>
        [DisplayName("角色說明")]
        public string? Comment { get; set; }

        /// <summary>角色種類</summary>
        [DisplayName("角色種類")]
        public string? SystemRoleCode { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }
    }

    public class ConsultationRoleMangEditModel
    {
        /// <summary>
        /// 已有權限
        /// </summary>
        public List<SelectListItem> LstFunItem = new List<SelectListItem>();

        /// <summary>
        /// 新權限
        /// </summary>
        public string? strFunInfo { get; set; }

        /// <summary>角色ID</summary>
        [DisplayName("角色ID")]
        public string? RoleId { get; set; }

        /// <summary>角色</summary>
        [DisplayName("角色")]
        public string? RoleName { get; set; }

        /// <summary>角色種類</summary>
        [DisplayName("角色種類")]
        public string? SystemRoleCode { get; set; }

        /// <summary>角色說明</summary>
        [DisplayName("角色說明")]
        public string? Comment { get; set; }

        [DisplayName("建立時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? Created { get; set; }

        /// <summary>更新者</summary>
        [DisplayName("更新者")]
        public string? LastModifier { get; set; }

        [DisplayName("更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastModified { get; set; }
    }

}
