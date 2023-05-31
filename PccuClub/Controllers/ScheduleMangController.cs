using DataAccess;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PccuClub.WebAuth;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web.Mvc;
using Utility;
using WebAuth.Entity;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.後台活動績效管理)]
    public class ScheduleMangController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        ReturnViewModel vmRtn = new ReturnViewModel();
        ScheduleMangDataAccess dbAccess = new ScheduleMangDataAccess();
        AuthManager auth = new AuthManager();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public ScheduleMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllActType = dbAccess.GetAllActType();
            ViewBag.ddlAllActHoldType = dbAccess.GetAllActHoldType();

            ScheduleMangViewModel vm = new ScheduleMangViewModel();
            vm.ConditionModel = new ScheduleMangConditionModel();

            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllActType = dbAccess.GetAllActType();
            ViewBag.ddlAllActHoldType = dbAccess.GetAllActHoldType();
            ViewBag.ddlAllClub = dbAccess.GetAllClub();

            ScheduleMangViewModel vm = new ScheduleMangViewModel();
            vm.CreateModel = new ScheduleMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, ScheduleMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllActType = dbAccess.GetAllActType();
            ViewBag.ddlAllActHoldType = dbAccess.GetAllActHoldType();

            vm.EditModel = dbAccess.GetEditData(submitBtn);

            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ScheduleMangViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> SaveNewData(ScheduleMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("Attachment"))
                        {
                            var file = Request.Form.Files.GetFile("CreateModel.Attachment");

                            string strFilePath = await upload.UploadFileAsync("Schedule", file);

                            vm.CreateModel.Attachment = strFilePath;
                        }
                    }
                }

                var dbResult = dbAccess.InsertData(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "新增失敗" + ex.Message;
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> EditOldData(ScheduleMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("Attachment"))
                        {
                            var file = Request.Form.Files.GetFile("EditModel.Attachment");
                            
                            string strFilePath = await upload.UploadFileAsync("Schedule", file);

                            vm.EditModel.Attachment = strFilePath;
                        }
                    }
                }

                var dbResult = dbAccess.UpdateData(vm, LoginUser);

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

        [Log(LogActionChineseName.刪除)]
        [ValidateInput(false)]
        public IActionResult Delete(string Ser)
        {
            try
            {

                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.DeletetData(Ser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "刪除失敗";
                    return Json(vmRtn);
                }

                dbAccess.DbaCommit();
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "刪除失敗" + ex.Message;
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportSearchResult(ScheduleMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.活動績效管理, DateTime.Now.ToString("yyyyMMdd"));
            vm.ExcelModel = dbAccess.GetExportResult(vm.ConditionModel);

            if (vm.ExcelModel != null && vm.ExcelModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 50, 20, 20, 20, 20, 50 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(ScheduleMangExcelModel).GetProperties();

                //設定欄位
                IRow headerRow = sheet.CreateRow(0);

                XSSFCellStyle headStyle = ExcelUtil.GetDefaultHeaderStyle(workbook);

                for (int i = 0; i <= properties.Length - 1; i++)
                {
                    var displayAttribute = (DisplayNameAttribute)properties[i].GetCustomAttribute(typeof(DisplayNameAttribute));
                    var displayName = displayAttribute?.DisplayName ?? properties[i].Name;

                    headerRow.CreateCell(i).SetCellValue(displayName);

                    foreach (ICell cell in headerRow.Cells)
                        cell.CellStyle = headStyle;

                }

                XSSFCellStyle contentStyle = ExcelUtil.GetDefaultContentStyle(workbook);

                //設定資料
                for (int i = 0; i <= vm.ExcelModel.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    dataRow.CreateCell(0).SetCellValue(vm.ExcelModel[i].ClubId);
                    dataRow.CreateCell(1).SetCellValue(vm.ExcelModel[i].SchoolYear);
                    dataRow.CreateCell(2).SetCellValue(vm.ExcelModel[i].ActTypeName);
                    dataRow.CreateCell(3).SetCellValue(vm.ExcelModel[i].CScheName);
                    dataRow.CreateCell(4).SetCellValue(vm.ExcelModel[i].CScheDate.Value.ToString("yyyy/MM/dd"));
                    dataRow.CreateCell(5).SetCellValue(vm.ExcelModel[i].ActHoldTypeText);
                    dataRow.CreateCell(6).SetCellValue(vm.ExcelModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm:ss"));

                    foreach (ICell cell in dataRow.Cells)
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



        #region Method

        #endregion
    }
}

