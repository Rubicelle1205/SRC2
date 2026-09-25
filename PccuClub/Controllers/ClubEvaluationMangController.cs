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
    [LogAttribute(LogActionChineseName.社團評鑑計分維護)]
    public class ClubEvaluationMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubEvaluationMangDataAccess dbAccess = new ClubEvaluationMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public ClubEvaluationMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlClub = dbAccess.GetAllClub();
            ViewBag.ddlClassId = dbAccess.GetClassId();

            ClubEvaluationMangViewModel vm = new ClubEvaluationMangViewModel();
            vm.ConditionModel = new ClubEvaluationMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlClub = dbAccess.GetAllClub();
            ViewBag.ddlClassId = dbAccess.GetClassId();

            ClubEvaluationMangViewModel vm = new ClubEvaluationMangViewModel();
            vm.CreateModel = new ClubEvaluationMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, ClubEvaluationMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlClub = dbAccess.GetAllClub();
            ViewBag.ddlClassId = dbAccess.GetClassId();
            

            vm.EditModel = dbAccess.GetEditData(submitBtn);
            vm.EditModel.HistoryModel = dbAccess.GetHistoryData(vm, submitBtn);
            ViewBag.BaseScore = dbAccess.GetBaseScore(vm);

            if (!string.IsNullOrEmpty(vm.EditModel.ClubEvaluationClassId))
            {
                ViewBag.ddlItemId = dbAccess.GetItemId(vm.EditModel.ClubEvaluationClassId);

                if (!string.IsNullOrEmpty(vm.EditModel.ClubEvaluationItemId))
                    ViewBag.ddlScore = dbAccess.GetScore(vm.EditModel.ClubEvaluationItemId);
            }
                

            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ClubEvaluationMangViewModel vm)
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
        public IActionResult SaveNewData(ClubEvaluationMangViewModel vm)
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
        public IActionResult EditOldData(ClubEvaluationMangViewModel vm)
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

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportSearchResult(ClubEvaluationMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.社團評鑑計分維護, DateTime.Now.ToString("yyyyMMdd"));
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel);

            if (vm.ResultModel != null && vm.ResultModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 20, 50, 50, 50, 20, 50 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                string[] allowedFields = new string[] { "SchoolYear", "ClubID", "ClubCName", "ClassName", "ItemName", "Score", "Memo" };

                var properties = typeof(ClubEvaluationMangResultModel).GetProperties()
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
                    dataRow.CreateCell(1).SetCellValue(vm.ResultModel[i].ClubID);
                    dataRow.CreateCell(2).SetCellValue(vm.ResultModel[i].ClubCName);
                    dataRow.CreateCell(3).SetCellValue(vm.ResultModel[i].ClassName);
                    dataRow.CreateCell(4).SetCellValue(vm.ResultModel[i].ItemName);
                    dataRow.CreateCell(5).SetCellValue(vm.ResultModel[i].Score);
                    dataRow.CreateCell(6).SetCellValue(vm.ResultModel[i].Memo);

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

        public Microsoft.AspNetCore.Mvc.JsonResult GetItemsByClassId(string classId)
        {
            var items = dbAccess .GetItemId(classId)
                   .Select(x => new SelectListItem
                   {
                       Value = x.Value.ToString(), // 前端 option 的 value
                       Text = x.Text            // 前端 option 顯示的文字
                   })
                   .ToList();

            return Json(items);
        }

        public Microsoft.AspNetCore.Mvc.JsonResult GetScoreByItemId(string itemId)
        {
            var items = dbAccess.GetScore(itemId)
                   .Select(x => new SelectListItem
                   {
                       Value = x.Value.ToString(), // 前端 option 的 value
                       Text = x.Text            // 前端 option 顯示的文字
                   })
                   .ToList();

            return Json(items);
        }

    }
}
