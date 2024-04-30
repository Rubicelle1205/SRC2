using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Web.Helpers;
using System.Web.Mvc;
using Utility;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.初談預約管理)]
    public class ConsultationFirstTalkMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        ConsultationFirstTalkMangDataAccess dbAccess = new ConsultationFirstTalkMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public ConsultationFirstTalkMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlPsy = dbAccess.GetddlPsy();
            ViewBag.ddlFirstTalkStatus = dbAccess.GetddlFirstTalkStatus();

            ConsultationFirstTalkMangViewModel vm = new ConsultationFirstTalkMangViewModel();
            vm.ConditionModel = new ConsultationFirstTalkMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, ConsultationFirstTalkMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlPsy = dbAccess.GetddlPsy();
            ViewBag.ddlFirstTalkStatus = dbAccess.GetddlFirstTalkStatus();
            ViewBag.ddlCounsellingStatus = dbAccess.GetddlCounsellingStatus();

            vm.EditModel = dbAccess.GetEditData(submitBtn);
            vm.EditModel.LstAppointmentTimeModel = dbAccess.GetApppoointemntTime(submitBtn);
            vm.EditModel.strAppointmentTime = TransToStr2(vm.EditModel.LstAppointmentTimeModel);
            ViewBag.Schedule = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(vm.EditModel.strAppointmentTime);

            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ConsultationFirstTalkMangViewModel vm)
        {
            
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            foreach (ConsultationFirstTalkMangResultModel item in vm.ResultModel)
                item.CounsellingStatusText = TransCounsellingStatusCodeToText(item.CounsellingStatus);

            return PartialView("_SearchResultPartial", vm);
        }


        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult EditOldData(ConsultationFirstTalkMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();
                var dbResult = new DbExecuteInfo();

                if(string.IsNullOrEmpty(vm.EditModel.AssignCaseMan))
                    dbResult = dbAccess.UpdateDataFirst(vm, LoginUser);
                else
                    dbResult = dbAccess.UpdateDataSecond(vm, LoginUser);

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

                dbResult = dbAccess.DeletetAppointmentTimeMangData(Ser);

                if (!dbResult.isSuccess)
                {
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
        public IActionResult ExportSearchResult(ConsultationFirstTalkMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.初談預約管理, DateTime.Now.ToString("yyyyMMdd"));
            vm.ExcelModel = dbAccess.GetExportResult(vm.ConditionModel);

            foreach (ConsultationFirstTalkMangExcelModel item in vm.ExcelModel)
                item.CounsellingStatusText = TransCounsellingStatusCodeToText(item.CounsellingStatus);

            if (vm.ExcelModel != null && vm.ExcelModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 20, 20, 70, 20, 30, 20, 20, 30, 20 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(ConsultationFirstTalkMangExcelHeaderModel).GetProperties();

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

                    dataRow.CreateCell(0).SetCellValue(vm.ExcelModel[i].Name);
                    dataRow.CreateCell(1).SetCellValue(vm.ExcelModel[i].Department);
                    dataRow.CreateCell(2).SetCellValue(vm.ExcelModel[i].SexText);
                    dataRow.CreateCell(3).SetCellValue(vm.ExcelModel[i].CounsellingStatusText);
                    dataRow.CreateCell(4).SetCellValue(vm.ExcelModel[i].AssignCaseManText);
                    dataRow.CreateCell(5).SetCellValue(vm.ExcelModel[i].AssignCaseTime != null ? vm.ExcelModel[i].AssignCaseTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : "");
                    dataRow.CreateCell(6).SetCellValue(vm.ExcelModel[i].PsychologistText);
                    dataRow.CreateCell(7).SetCellValue(vm.ExcelModel[i].FirstTalkStatusText);
                    dataRow.CreateCell(8).SetCellValue(vm.ExcelModel[i].FirstTalkTime != null ? vm.ExcelModel[i].FirstTalkTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : "");
                    dataRow.CreateCell(9).SetCellValue(vm.ExcelModel[i].Created != null ? vm.ExcelModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm:ss") : "");

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

        private string? TransToStr(List<AppointmentTimeModel> model)
        {
            if (null == model)
                return "{}";
            
            var data = new Dictionary<string, List<int>>();
            
            List<string> LstWeekModel = model.Select(x => x.Week).Distinct().ToList();

            foreach (string item in LstWeekModel)
            {
                string week = item;
                List<int> Lsthours = new List<int>();
                List<AppointmentTimeModel> LstHourModel = model.Where(x => x.Week == week).ToList();

                foreach (AppointmentTimeModel item2 in LstHourModel)
                {
                    Lsthours.Add(int.Parse(item2.Hour));
                }

                data.Add(week, Lsthours);
            }

            return System.Text.Json.JsonSerializer.Serialize(data);
        }

        private string? TransToStr2(List<AppointmentTimeModel> model)
        {
            if (null == model)
                return "{}";

            var data = new Dictionary<string, List<int>>();

            List<string> LstHourModel = model.Select(x => x.Hour).Distinct().ToList();

            for (int i = 9; i <= 15; i++)
            {
                if (!LstHourModel.Contains(i.ToString()))
                {
                    LstHourModel.Add(i.ToString());
                }
            }

            LstHourModel = LstHourModel.OrderBy(n => int.Parse(n)).ToList();

            foreach (string item in LstHourModel)
            {
                string hour = item;
                List<int> LstWeeks = new List<int>();
                List<AppointmentTimeModel> LstWeekModel = model.Where(x => x.Hour == hour).ToList();

                foreach (AppointmentTimeModel item2 in LstWeekModel)
                {
                    LstWeeks.Add(int.Parse(item2.Week));
                }

                data.Add(hour, LstWeeks);
            }

            return System.Text.Json.JsonSerializer.Serialize(data);
        }

        private string? TransCounsellingStatusCodeToText(string? strCode)
        {
            if (null == strCode || strCode.Length == 0)
                return "";

            string str = string.Empty;
            string[] arr = strCode.Split(",");

            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstCounsellingStatus = dbAccess.GetddlCounsellingStatus();

            for (int i = 0; i <= arr.Length - 1; i++)
            {
                if (i != arr.Length - 1)
                {
                    str += LstCounsellingStatus.Where(x => x.Value == arr[i]).FirstOrDefault().Text + "、";
                }
                else
                {
                    str += LstCounsellingStatus.Where(x => x.Value == arr[i]).FirstOrDefault().Text;
                }
            }

            return str;
        }
    }
}
