using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web.Mvc;
using Utility;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.子資源維護)]
    public class BorrowSecondResourceMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        BorrowSecondResourceMangDataAccess dbAccess = new BorrowSecondResourceMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public BorrowSecondResourceMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlMainClass = dbAccess.GetddlMainClass();
            ViewBag.ddlShelvesStatus = dbAccess.GetddlShelvesStatus();
            ViewBag.ddlBorrowStatus = dbAccess.GetddlBorrowStatus();

            BorrowSecondResourceMangViewModel vm = new BorrowSecondResourceMangViewModel();
            vm.ConditionModel = new BorrowSecondResourceMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, BorrowSecondResourceMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlMainClass = dbAccess.GetddlMainClass();
            ViewBag.ddlMainResource = dbAccess.GetddlMainResource();
            ViewBag.ddlShelvesStatus = dbAccess.GetddlShelvesStatus();
            ViewBag.ddlBorrowStatus = dbAccess.GetddlBorrowStatus();

            vm.EditModel = dbAccess.GetEditData(submitBtn);
            
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(BorrowSecondResourceMangViewModel vm)
        {
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
        public IActionResult EditOldData(BorrowSecondResourceMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

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

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult EditOldEventData(BorrowSecondResourceMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

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
                    vmRtn.ErrorCode =  (int)DBActionChineseName.失敗;
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

        [Log(LogActionChineseName.匯入)]
        public IActionResult Upload()
        {
            BorrowSecondResourceMangViewModel vm = new BorrowSecondResourceMangViewModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.下載template檔案)]
        public IActionResult DownloadTemplate()
        {
            string FileName = "子資源維護_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }

        [LogAttribute(LogActionChineseName.匯入Excel)]
        public IActionResult ImportExcel(BorrowSecondResourceMangViewModel vm)
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

                if (!vm.File.FileName.Contains("子資源維護_template"))
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "選擇檔案錯誤";
                    return Json(vmRtn);
                }

                List<BorrowSecondResourceMangImportModel> LstExcel = new List<BorrowSecondResourceMangImportModel>();

                using (Stream stream = vm.File.OpenReadStream())
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);
                    List<string> LstSNo = new List<string>();

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        bool CanGo = true;

                        for (int j = 0; j <= row.Count() - 1; j++)
                        {
                            row.GetCell(j)?.SetCellType(CellType.String);
                            string Celldata = row.GetCell(j)?.ToString();
                            if (string.IsNullOrEmpty(Celldata))
                                CanGo = false;
                        }

                        if (row != null && CanGo)
                        {

                            string MainResourceID = "";
                            string ShelvesStatus = "";
                            string BorrowStatus = "";

                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlMainResource = dbAccess.GetddlMainResource();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlShelvesStatus = dbAccess.GetddlShelvesStatus();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstddlBorrowStatus = dbAccess.GetddlBorrowStatus();


                            if (!LstddlMainResource.Any(m => m.Text == row.GetCell(0)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:主資源名稱{0}不存在", row.GetCell(0)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                MainResourceID = LstddlMainResource.Where(m => m.Text == row.GetCell(0)?.ToString()).FirstOrDefault().Value;
                            }

                            if (!LstddlShelvesStatus.Any(m => m.Text == row.GetCell(3)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗: 查無上下架狀態 {0}", row.GetCell(3)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                ShelvesStatus = LstddlShelvesStatus.Where(m => m.Text == row.GetCell(3)?.ToString()).FirstOrDefault().Value;
                            }

                            if (!LstddlBorrowStatus.Any(m => m.Text == row.GetCell(4)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗: 查無借用狀態 {0}", row.GetCell(4)?.ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                BorrowStatus = LstddlBorrowStatus.Where(m => m.Text == row.GetCell(4)?.ToString()).FirstOrDefault().Value;
                            }


                            BorrowSecondResourceMangImportModel excel = new BorrowSecondResourceMangImportModel
                            {
                                MainResourceID = MainResourceID,
                                SecondResourceNo = row.GetCell(1)?.StringCellValue.TrimStartAndEnd(),
                                SecondResourceName = row.GetCell(2)?.StringCellValue.TrimStartAndEnd(),
                                ShelvesStatus = ShelvesStatus,
                                BorrowStatus = BorrowStatus,
                                Memo = row.GetCell(5)?.StringCellValue.TrimStartAndEnd(),
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

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportSearchResult(BorrowSecondResourceMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.子資源維護, DateTime.Now.ToString("yyyyMMdd"));
            vm.ResultModel = dbAccess.GetExportResult(vm.ConditionModel);

            if (vm.ResultModel != null && vm.ResultModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 20, 20, 20, 20, 20 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(BorrowSecondResourceMangExcelHeaderModel).GetProperties();

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
                for (int i = 0; i <= vm.ResultModel.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    dataRow.CreateCell(0).SetCellValue(vm.ResultModel[i].MainResourceIDText);
                    dataRow.CreateCell(1).SetCellValue(vm.ResultModel[i].MainClassText);
                    dataRow.CreateCell(2).SetCellValue(vm.ResultModel[i].SecondResourceNo);
                    dataRow.CreateCell(3).SetCellValue(vm.ResultModel[i].SecondResourceName);
                    dataRow.CreateCell(4).SetCellValue(vm.ResultModel[i].ShelvesStatusText);
                    dataRow.CreateCell(5).SetCellValue(vm.ResultModel[i].BorrowStatusText);
                    dataRow.CreateCell(6).SetCellValue(vm.ResultModel[i].Memo);

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
    }
}
