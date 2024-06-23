using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
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
    [LogAttribute(LogActionChineseName.諮商紀錄維護)]
    public class ConsultationCaseMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        ConsultationCaseMangDataAccess dbAccess = new ConsultationCaseMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public ConsultationCaseMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlPsy = dbAccess.GetddlPsy();
            ViewBag.ddlAssignCase = dbAccess.GetddlAssignCase();
            ViewBag.ddlRoom = dbAccess.GetddlRoom();

            ConsultationCaseMangViewModel vm = new ConsultationCaseMangViewModel();
            vm.ConditionModel = new ConsultationCaseMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create(string submitBtn, ConsultationCaseMangViewModel vm)
        {
            ViewBag.ddlPsy = dbAccess.GetddlPsy();
            ViewBag.ddlRoom = dbAccess.GetddlRoom();
            ViewBag.ddlTalkSTime = dbAccess.GetddlTalkSTime();
            ViewBag.ddlTalkETime = dbAccess.GetddlTalkETime();

            vm.CreateModel = new ConsultationCaseMangCreateModel();

            List<Order> LstOrder = dbAccess.GetOrder();
            List<string> LstDate = new List<string>();
            List<RoomDataModel> initData = new List<RoomDataModel>();

            foreach (Order item in LstOrder)
            {
                string date = LstDate.Where(x => x == item.Date).FirstOrDefault();

                if (date == null)
                {
                    RoomDataModel room = new RoomDataModel();
                    room.date = item.Date;
                    room.classname = "";
                    room.markup = "<span class='badge rounded-pill bg-success'>[day]</span>";

                    List<Order> lstOrder = LstOrder.Where(x => x.Date == item.Date).ToList();

                    foreach (Order item2 in lstOrder)
                    {
                        var order = new List<object>
                        {
                            new 
                            { 
                                id = item2.ID, 
                                room_id = item2.RoomId, 
                                room_title = item2.RoomTitle, 
                                psychologist_title = item2.PsychologistTitle, 
                                student_number = item2.StudentNumber, 
                                start_time = item2.StartTime, 
                                end_time = item2.EndTime 
                            }
                        };

                        room.orders.AddRange(order);
                    }

                    initData2.Add(room);
                }

                LstDate.Add(item.Date);
            }


            ViewBag.InitData = JsonConvert.SerializeObject(initData);

            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, ConsultationCaseMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlAssignCase = dbAccess.GetddlAssignCase();
            vm.EditModel = dbAccess.GetEditData(submitBtn);

            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ConsultationCaseMangViewModel vm)
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
        public async Task<IActionResult> SaveNewDataAsync(ConsultationCaseMangViewModel vm)
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
        public IActionResult EditOldData(ConsultationCaseMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();
                var dbResult = new DbExecuteInfo();

                    dbResult = dbAccess.UpdateData(vm, LoginUser);

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
        public IActionResult ExportSearchResult(ConsultationCaseMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.諮商紀錄維護, DateTime.Now.ToString("yyyyMMdd"));
            vm.ExcelModel = dbAccess.GetExportResult(vm.ConditionModel);

            if (vm.ExcelModel != null && vm.ExcelModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 30, 30, 30, 30, 30, 30, 30, 30, 30 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(ConsultationCaseMangExcelHeaderModel).GetProperties();

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

                    dataRow.CreateCell(0).SetCellValue(string.Format("{0} {1}~{2}", vm.ExcelModel[i].TalkDate, vm.ExcelModel[i].TalkSTime, vm.ExcelModel[i].TalkETime));
                    dataRow.CreateCell(1).SetCellValue(vm.ExcelModel[i].Name);
                    dataRow.CreateCell(2).SetCellValue(vm.ExcelModel[i].SNO);
                    dataRow.CreateCell(3).SetCellValue(vm.ExcelModel[i].AssignCaseManText);
                    dataRow.CreateCell(4).SetCellValue(vm.ExcelModel[i].AssignCaseTime != null ? vm.ExcelModel[i].AssignCaseTime.Value.ToString("yyyy/MM/dd HH:mm:ss") : "");
                    dataRow.CreateCell(5).SetCellValue(vm.ExcelModel[i].PsychologistText);
                    dataRow.CreateCell(6).SetCellValue(vm.ExcelModel[i].RoomIDText);
                    dataRow.CreateCell(7).SetCellValue(vm.ExcelModel[i].AssignCaseStatusText);
                    dataRow.CreateCell(8).SetCellValue(vm.ExcelModel[i].Created != null ? vm.ExcelModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm:ss") : "");

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
    }
}
