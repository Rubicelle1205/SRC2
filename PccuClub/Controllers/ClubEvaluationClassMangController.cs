using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;
using System.Reflection;
using System.Web.Mvc;
using Utility;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.評鑑類別維護)]
    public class ClubEvaluationClassMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        UploadUtil upload = new UploadUtil();
        ClubEvaluationClassMangDataAccess dbAccess = new ClubEvaluationClassMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public ClubEvaluationClassMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubEvaluationClassMangViewModel vm = new ClubEvaluationClassMangViewModel();
            vm.ConditionModel = new ClubEvaluationClassMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            //ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubEvaluationClassMangViewModel vm = new ClubEvaluationClassMangViewModel();
            vm.CreateModel = new ClubEvaluationClassMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.匯入)]
        public IActionResult Upload()
        {
            ClubEvaluationClassMangViewModel vm = new ClubEvaluationClassMangViewModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, ClubEvaluationClassMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            //ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            //ClubEvaluationClassMangViewModel vm = new ClubEvaluationClassMangViewModel();
            vm.EditModel = dbAccess.GetEditData(submitBtn);
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ClubEvaluationClassMangViewModel vm)
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
        public IActionResult SaveNewData(ClubEvaluationClassMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

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
        public IActionResult EditOldData(ClubEvaluationClassMangViewModel vm)
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

        [LogAttribute(LogActionChineseName.匯入Excel)]
        public async Task<IActionResult> ImportExcelAsync(ClubEvaluationClassMangViewModel vm)
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

                if (!vm.File.FileName.Contains(LogActionChineseName.評鑑類別維護.ToString()))
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "選擇檔案錯誤";
                    return Json(vmRtn);
                }

                List<ClubEvaluationClassMangExcelModel> LstExcel = new List<ClubEvaluationClassMangExcelModel>();

                using (Stream stream = vm.File.OpenReadStream())
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; // 防呆：如果是空行就跳過

                        // 安全地將所有有資料的 Cell 轉為 String 型態
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            var cell = row.GetCell(j);
                            cell?.SetCellType(CellType.String);
                        }

                        // 讀取並清除欄位前後空白
                        string schoolYear = row.GetCell(0)?.StringCellValue.TrimStartAndEnd();
                        string ClassName = row.GetCell(1)?.StringCellValue.TrimStartAndEnd();
                        string Memo = row.GetCell(2)?.StringCellValue.TrimStartAndEnd();


                        // 封裝 Model
                        LstExcel.Add(new ClubEvaluationClassMangExcelModel
                        {
                            SchoolYear = schoolYear,
                            ClassName = ClassName,
                            Memo = Memo
                        });
                    }
                }

                // --- 資料庫寫入與交易控制 ---
                if (LstExcel.Count > 0)
                {
                    dbAccess.DbaInitialTransaction();
                    try
                    {
                        var dbResult = dbAccess.ImportData(LstExcel, LoginUser);
                        if (!dbResult.isSuccess)
                        {
                            dbAccess.DbaRollBack();
                            vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                            vmRtn.ErrorMsg = "上傳失敗";
                            return Json(vmRtn);
                        }
                        dbAccess.DbaCommit();
                    }
                    catch (Exception ex)
                    {
                        dbAccess.DbaRollBack();
                        // 可寫入 log 記錄 ex.Message
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "資料庫寫入異常";
                        return Json(vmRtn);
                    }
                }
            }
            else
            {
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "請選擇檔案上傳";
            }

            return Json(vmRtn);
        }

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportSearchResult(ClubEvaluationClassMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.評鑑類別維護, DateTime.Now.ToString("yyyyMMdd"));
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel);

            if (vm.ResultModel != null && vm.ResultModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 20, 50, 50 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                string[] allowedFields = new string[] { "SchoolYear", "ClassName", "Memo", "Created" };

                var properties = typeof(ClubEvaluationClassMangResultModel).GetProperties()
                        .Where(p => allowedFields.Contains(p.Name))
                        .ToArray();

                IRow headerRow = sheet.CreateRow(0);
                XSSFCellStyle headStyle = ExcelUtil.GetDefaultHeaderStyle(workbook);

                for (int i = 0; i <= properties.Length - 1; i++)
                {
                    var displayAttribute = (DisplayNameAttribute)properties[i].GetCustomAttribute(typeof(DisplayNameAttribute));
                    var displayName = displayAttribute?.DisplayName ?? properties[i].Name;

                    headerRow.CreateCell(i).SetCellValue(displayName);

                    // 效能優化提醒：原本你的 foreach 寫在 for 迴圈內，會導致每次建立新 Cell 時
                    // 都把整排 headerRow 的 Cell 全部重新跑一次迴圈設定 Style，這裡順便幫你改成只設定當前建立的 Cell 
                    headerRow.GetCell(i).CellStyle = headStyle;
                }

                XSSFCellStyle contentStyle = ExcelUtil.GetDefaultContentStyle(workbook);

                //設定資料
                for (int i = 0; i <= vm.ResultModel.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    dataRow.CreateCell(0).SetCellValue(vm.ResultModel[i].SchoolYear);
                    dataRow.CreateCell(1).SetCellValue(vm.ResultModel[i].ClassName);
                    dataRow.CreateCell(2).SetCellValue(vm.ResultModel[i].Memo);
                    dataRow.CreateCell(3).SetCellValue(vm.ResultModel[i].Created?.ToString("yyyy/MM/dd HH:mm:ss"));

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

        public IActionResult DownloadTemplate()
        {
            string FileName = "評鑑類別維護_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }
    }
}
