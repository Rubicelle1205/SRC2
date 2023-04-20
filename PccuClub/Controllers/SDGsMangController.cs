using DataAccess;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.SDGs維護)]
    public class SDGsMangController : BaseController
    {
        SDGsMangDataAccess dbAccess = new SDGsMangDataAccess();

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            SDGsMangViewModel vm = new SDGsMangViewModel();
            vm.ConditionModel = new SDGsMangConditionModel();
            return View(vm);
        }


        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(SDGsMangViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [LogAttribute(LogActionChineseName.匯出)]
        public IActionResult ExportSearchResult(SDGsMangViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            //string oMsg = string.Empty;
            //DataSet ds = shr.TransExcel(vm.EditViewModel);

            //if (ds != null && ds.Tables[0].Rows.Count > 0)
            //{
            //    string FileName = "出庫單_" + vm.EditViewModel.OUTSTO_ID + "_" + DateTime.Now.ToString("yyyyMMdd");

            //    //從Model取得欄位名稱
            //    var DisplayName = String.Empty;

            //    var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(OutStockTransExcel));

            //    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            //        metadata.Properties.Where(p => p.PropertyName.Equals(ds.Tables[0].Columns[i].ColumnName))
            //            .ToList().ForEach(x => ds.Tables[0].Columns[i].ColumnName = x.DisplayName);

            //    return ExcelUtil.ExportExcelForMVC(ds, FileName);
            //}

            //AlertMessage = "匯出失敗!";

            return View("Index", vm);
            //return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"KEY_DomesticSearchAlert_國內疫情偵蒐預警關鍵字清單_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult EditOldData(SDGsMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                //var dbResult = dbAccess.UpdateConsent(vm.EditModel, LoginUser.UserName);

                //if (!dbResult.isSuccess)
                //    throw new Exception("Update/Insert Doc News failed");
                //dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

    }
}
