using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming.Values;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using Org.BouncyCastle.Utilities;
using PccuClub.WebAuth;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Utility;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.前台活動報備)]
    public class ClubActFinishController : FBaseController
	{
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubActFinishDataAccess dbAccess = new ClubActFinishDataAccess();
		ActListMangDataAccess dbAccess3 = new ActListMangDataAccess();
		UploadUtil upload = new UploadUtil();
        MailUtil mail = new MailUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public ClubActFinishController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlActVerify = dbAccess.GetActVerify();

            ClubActFinishViewModel vm = new ClubActFinishViewModel();
            vm.ConditionModel = new ClubActFinishConditionModel();
            vm.ConditionModel.SchoolYear = PublicFun.GetNowSchoolYear();
            
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Detail(string id, string type, ClubActFinishViewModel vm)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            if(type != "P" && type != "D")
                return RedirectToAction("Index");

            vm.DetailModel = dbAccess.GetDetailData(id);
            vm.DetailModel.PersonModel = dbAccess.GetPersonData(id);
            vm.DetailModel.type = type;

            return View(vm);
        }

        [Log(LogActionChineseName.匯出列印活動證明_個人)]
        public IActionResult SelectedPersonal(string id)
        {
            ViewBag.ddlName = dbAccess.GetActFinishNames(id);

            ClubActFinishViewModel vm = new ClubActFinishViewModel();
            vm.DetailModel = dbAccess.GetDetailData(id);

            return View(vm);
        }
        
        [Log(LogActionChineseName.匯出列印活動證明_個人)]
        public IActionResult PrintPersonal()
        {
            ClubActFinishViewModel vmm = HttpContext.Session.GetObject<ClubActFinishViewModel>("ClubActFinishViewModel");
            string ActFinishPersonId = vmm.PrintModel.ActFinishPersonId;
            string ActFinishId = vmm.DetailModel.ActFinishId;

            ClubActFinishViewModel vm = new ClubActFinishViewModel();
            vm.DetailModel = dbAccess.GetDetailData(ActFinishId);
            vm.PrintModel = dbAccess.GetPrintData(ActFinishPersonId);

            return View(vm);
        }

        [Log(LogActionChineseName.匯出列印活動證明_社團)]
        public IActionResult PrintGroup(string id)
        {
            ClubActFinishViewModel vm = new ClubActFinishViewModel();
            vm.DetailModel = dbAccess.GetDetailData(id);
            vm.GroupPrintModel = dbAccess.GetPrintGroupData(id);

            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ClubActFinishViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel, LoginUser).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportSearchResult(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            string FileName = string.Format("{0}_{1}", "參與學號", DateTime.Now.ToString("yyyyMMdd"));

            ClubActFinishViewModel vm = new ClubActFinishViewModel();
            vm.ExcelModel = dbAccess.GetExportResult(id);

            if (vm.ExcelModel != null && vm.ExcelModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 50 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(PersonModel).GetProperties();

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
                    
                    dataRow.CreateCell(0).SetCellType(CellType.String);
                    dataRow.CreateCell(1).SetCellType(CellType.String);

                    dataRow.GetCell(0).SetCellValue(vm.ExcelModel[i].SNO);
                    dataRow.GetCell(1).SetCellValue(vm.ExcelModel[i].Name);

                    foreach (var cell in dataRow.Cells)
                        cell.CellStyle = contentStyle;
                }

                MemoryStream ms = new MemoryStream();
                workbook.Write(ms, true);
                ms.Flush();
                ms.Position = 0;

                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
            }

            AlertMsg.Add("無資料已供匯出");
            return Redirect("Index");

        }

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportALLSearchResult(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            if(id.Split("|").Length != 2)
                return RedirectToAction("Index");

            string year = id.Split("|")[0];
            string verify = id.Split("|")[1] == "undefined" ? null : id.Split("|")[1];

            string FileName = string.Format("{0}_{1}", "參與學號", DateTime.Now.ToString("yyyyMMdd"));

            ClubActFinishViewModel vm = new ClubActFinishViewModel();
            vm.ALLExcelModel = dbAccess.GetExportResult(year, verify, LoginUser);

            if (vm.ALLExcelModel != null && vm.ALLExcelModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 20, 80, 50 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(ALLPersonModel).GetProperties();

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
                for (int i = 0; i <= vm.ALLExcelModel.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    dataRow.CreateCell(0).SetCellType(CellType.String);
                    dataRow.CreateCell(1).SetCellType(CellType.String);
                    dataRow.CreateCell(2).SetCellType(CellType.String);
                    dataRow.CreateCell(3).SetCellType(CellType.String);

                    dataRow.GetCell(0).SetCellValue(vm.ALLExcelModel[i].SchoolYear);
                    dataRow.GetCell(1).SetCellValue(vm.ALLExcelModel[i].ActID);
                    dataRow.GetCell(2).SetCellValue(vm.ALLExcelModel[i].ActName);
                    dataRow.GetCell(3).SetCellValue(vm.ALLExcelModel[i].SNO);

                    foreach (var cell in dataRow.Cells)
                        cell.CellStyle = contentStyle;
                }

                MemoryStream ms = new MemoryStream();
                workbook.Write(ms, true);
                ms.Flush();
                ms.Position = 0;

                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
            }

            AlertMsg.Add("無資料已供匯出");
            return Redirect("Index");

        }

        [Log(LogActionChineseName.匯出列印活動證明_個人_傳送列印資料)]
        [ValidateInput(false)]
        public async Task<IActionResult> SendPersonalData(ClubActFinishViewModel vmm)
        {
            try
            {
                string ActFinishPersonId = vmm.PrintModel.ActFinishPersonId;
                string ActFinishId = vmm.DetailModel.ActFinishId;

                ClubActFinishViewModel vm = new ClubActFinishViewModel();
                vm.DetailModel = dbAccess.GetDetailData(ActFinishId);
                vm.PrintModel = dbAccess.GetPrintData(ActFinishPersonId);

                HttpContext.Session.SetObject("ClubActFinishViewModel", vm);

                return Json(vmRtn);
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                return RedirectToAction("ActFail");
            }
        }
    }
}
