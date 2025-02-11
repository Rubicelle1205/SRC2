using DataAccess;
using MathNet.Numerics.RootFinding;
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

        [Log(LogActionChineseName.匯入)]
        public IActionResult Upload()
        {
            ScheduleMangViewModel vm = new ScheduleMangViewModel();
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

        [LogAttribute(LogActionChineseName.匯入Excel)]
        public IActionResult ImportExcel(ScheduleMangViewModel vm)
        {
            if (vm.File != null && vm.File.Length > 0)
            {
                string fileExtension = Path.GetExtension(vm.File.FileName);

                if (fileExtension != ".xlsx")
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "選擇檔案格式錯誤";
                    return Json(vmRtn);
                }

                if (!vm.File.FileName.Contains(LogActionChineseName.活動績效管理.ToString()))
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "選擇檔案錯誤";
                    return Json(vmRtn);
                }

                List<ScheduleMangImportExcelModel> LstExcel = new List<ScheduleMangImportExcelModel>();

                using (Stream stream = vm.File.OpenReadStream())
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        for (int j = 0; j <= row.Count() - 1; j++)
                        {
                            row.GetCell(j).SetCellType(CellType.String);
                        }

                        if (row != null)
                        {
                            string ActType = "";
                            string ActHoldType = "";
                            DateTime CScheDate = DateTime.Now;

                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstClubID = dbAccess.GetAllClub();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstActType = dbAccess.GetActType();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstActHoldType = dbAccess.GetActHoldType();

                            if (!LstClubID.Any(m => m.Value == row.GetCell(0)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(0).ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }

                            if (!LstActType.Any(m => m.Text == row.GetCell(2)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(4).ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                ActType = LstActType.Where(m => m.Text == row.GetCell(2)?.ToString()).FirstOrDefault().Value;
                            }

                            bool ok = DateTime.TryParse(row.GetCell(4)?.StringCellValue, out CScheDate);

                            if (!ok)
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(4).ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }

                            if (!LstActHoldType.Any(m => m.Text == row.GetCell(5)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(5).ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                ActHoldType = LstActHoldType.Where(m => m.Text == row.GetCell(5)?.ToString()).FirstOrDefault().Value;
                            }

                            ScheduleMangImportExcelModel excel = new ScheduleMangImportExcelModel
                            {
                                ClubId = row.GetCell(0)?.StringCellValue.TrimStartAndEnd(),
                                SchoolYear = row.GetCell(1)?.StringCellValue.TrimStartAndEnd(),
                                ActTypeID = ActType,
                                CScheName = row.GetCell(3)?.StringCellValue.TrimStartAndEnd(),
                                CScheDate = CScheDate.ToString("yyyy-MM-dd"),
                                ActHoldType = ActHoldType,
                                BookingPlace = row.GetCell(6)?.StringCellValue.TrimStartAndEnd(),
                                Budget = row.GetCell(7)?.StringCellValue.TrimStartAndEnd(),
                                ShortDesc = row.GetCell(8)?.StringCellValue.TrimStartAndEnd(),
                            };

                            LstExcel.Add(excel);
                        }
                    }
                }

                dbAccess.DbaInitialTransaction();
                if (LstExcel.Count > 0)
                {
                    var dbResult = dbAccess.ImportData(LstExcel, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "上傳失敗";
                    }
                }

                dbAccess.DbaCommit();
            }
            else
            {
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "請選擇檔案上傳";
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

            AlertMsg.Add("無資料已供匯出");
            return Redirect("Index");

        }

        [LogAttribute(LogActionChineseName.下載template檔案)]
        public IActionResult DownloadTemplate()
        {
            string FileName = "活動績效管理_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }

        #region Method

        #endregion
    }
}

