using DataAccess;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming.Values;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Utility;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.行程管理)]
    public class RundownMangController : BaseController
    {
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        RundownMangDataAccess dbAccess = new RundownMangDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public RundownMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify();
            ViewBag.ddlAllActType = dbAccess.GetAllActType();
            ViewBag.ddlAllSDGs = dbAccess.GetAllSDGs();
            ViewBag.ddlAllPlaceSource = dbAccess.GetAllPlaceSource();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();
            ViewBag.ddlRundownStatus = dbAccess.GetAllRundownStatus();


            RundownMangViewModel vm = new RundownMangViewModel();
            vm.ConditionModel = new RundownMangConditionModel();
            return View(vm);
        }


        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(RundownMangViewModel vm)
        {
            ViewBag.ddlRundownStatus = dbAccess.GetAllRundownStatus();

            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> EditOldData(RundownMangViewModel vm)
        {
            try
            {

                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdateRundownData(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "修改失敗";
                    return Json(vmRtn);
                }

                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportSearchResult(RundownMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.幹部名冊, DateTime.Now.ToString("yyyyMMdd"));
            //vm.ExcelModel = dbAccess.GetExportResult(vm.ConditionModel);

            if (vm.ExcelModel != null && vm.ExcelModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 50, 20, 20, 20, 20, 20, 30,30,30 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(RundownMangExcelResultModel).GetProperties();

                //設定欄位
                IRow headerRow = sheet.CreateRow(0);

                XSSFCellStyle headStyle = ExcelUtil.GetDefaultHeaderStyle(workbook);

                for (int i = 0; i <= properties.Length - 1; i++)
                {
                    var displayAttribute = (DisplayNameAttribute)properties[i].GetCustomAttribute(typeof(DisplayNameAttribute));
                    var displayName = displayAttribute?.DisplayName ?? properties[i].Name;

                    headerRow.CreateCell(i).SetCellValue(displayName);

                    foreach (var cell in headerRow.Cells)
                        cell.CellStyle = headStyle;

                }

                XSSFCellStyle contentStyle = ExcelUtil.GetDefaultContentStyle(workbook);

                //設定資料
                for (int i = 0; i <= vm.ExcelModel.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    //dataRow.CreateCell(0).SetCellValue(vm.ExcelModel[i].SchoolYear);
                    //dataRow.CreateCell(1).SetCellValue(vm.ExcelModel[i].ClubID);
                    //dataRow.CreateCell(2).SetCellValue(vm.ExcelModel[i].ClubCName);
                    //dataRow.CreateCell(3).SetCellValue(vm.ExcelModel[i].Organizer);
                    //dataRow.CreateCell(4).SetCellValue(vm.ExcelModel[i].AwdActName);
                    //dataRow.CreateCell(5).SetCellValue(vm.ExcelModel[i].AwdType);
                    //dataRow.CreateCell(6).SetCellValue(vm.ExcelModel[i].AwdName);
                    //dataRow.CreateCell(7).SetCellValue(vm.ExcelModel[i].AwdDate.Value.ToString("yyyy/MM/dd"));
                    //dataRow.CreateCell(8).SetCellValue(vm.ExcelModel[i].ActVerifyText);
                    //dataRow.CreateCell(9).SetCellValue(vm.ExcelModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm:ss"));

                    foreach (var cell in dataRow.Cells)
                        cell.CellStyle = contentStyle;
                }

                MemoryStream ms = new MemoryStream();
                workbook.Write(ms, true);
                ms.Flush();
                ms.Position = 0;

                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
            }

            return View("Index", vm);

        }

    }
}
