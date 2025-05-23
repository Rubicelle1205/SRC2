﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPccuClub.Models
{
    public class EventCaseStatusMangViewModel
    {
        [DisplayName("檔案上傳")]
        [UIHint("_UploadFile")]
        public IFormFile? File { get; set; }

        public EventCaseStatusMangConditionModel ConditionModel { get; set; }

        public List<EventCaseStatusMangResultModel> ResultModel { get; set; }

        public EventCaseStatusMangCreateModel CreateModel { get; set; }

        public EventCaseStatusMangEditModel EditModel { get; set; }

    }

    public class EventCaseStatusMangConditionModel
    {
        public EventCaseStatusMangConditionModel()
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

        /// <summary>啟用</summary>
        [DisplayName("啟用")]
        public string? Enable { get; set; }

        /// <summary>狀態名稱</summary>
        [DisplayName("狀態名稱")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class EventCaseStatusMangResultModel
    {
        public int? ID { get; set; }

        /// <summary>啟用狀態</summary>
        [DisplayName("啟用狀態")]
        public string? Enable { get; set; }

        /// <summary>啟用狀態</summary>
        [DisplayName("啟用狀態")]
        public string? EnableText { get; set; }

        /// <summary>狀態名稱</summary>
        [DisplayName("狀態名稱")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class EventCaseStatusMangCreateModel
    {
        /// <summary>啟用狀態</summary>
        [DisplayName("啟用狀態")]
        public string? Enable { get; set; }

        /// <summary>狀態名稱</summary>
        [DisplayName("狀態名稱")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }
    }

    public class EventCaseStatusMangEditModel
    {
        public int? ID { get; set; }

        /// <summary>啟用狀態</summary>
        [DisplayName("啟用狀態")]
        public string? Enable { get; set; }

        /// <summary>狀態名稱</summary>
        [DisplayName("狀態名稱")]
        public string? Text { get; set; }

        /// <summary>備註</summary>
        [DisplayName("備註")]
        public string? Memo { get; set; }

        /// <summary>建立時間</summary>
        [DisplayName("建立時間")]
        public string? Created { get; set; }

        /// <summary>更新時間</summary>
        [DisplayName("更新時間")]
        public string? LastModified { get; set; }
    }
}
