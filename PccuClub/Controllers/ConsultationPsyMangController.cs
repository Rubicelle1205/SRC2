using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Web.Mvc;
using Utility;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.心理師維護)]
    public class ConsultationPsyMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        ConsultationPsyMangDataAccess dbAccess = new ConsultationPsyMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public ConsultationPsyMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ConsultationPsyMangViewModel vm = new ConsultationPsyMangViewModel();
            vm.ConditionModel = new ConsultationPsyMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ConsultationPsyMangViewModel vm = new ConsultationPsyMangViewModel();
            vm.CreateModel = new ConsultationPsyMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, ConsultationPsyMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            vm.EditModel = dbAccess.GetEditData(submitBtn);
            vm.EditModel.LstAppointmentTimeModel = dbAccess.GetApppoointemntTime(submitBtn);
            vm.EditModel.strAppointmentTime = TransToStr(vm.EditModel.LstAppointmentTimeModel);

            var data = new Dictionary<string, List<int>>();

            List<int> Lsthours = new List<int>();
            List<string> LstWeekModel = vm.EditModel.LstAppointmentTimeModel.Select(x => x.Week).Distinct().ToList();

            foreach (string item in LstWeekModel)
            {
                string week = item;

                List<AppointmentTimeModel> LstHourModel = vm.EditModel.LstAppointmentTimeModel.Where(x => x.Week == week).ToList();

                foreach (AppointmentTimeModel item2 in LstHourModel)
                {
                    Lsthours.Add(int.Parse(item2.Hour));
                }

                data.Add(week, Lsthours);
            }



            vm.EditModel.strAppointmentTime = JsonSerializer.Serialize(data);


            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ConsultationPsyMangViewModel vm)
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
        public IActionResult EditOldData(ConsultationPsyMangViewModel vm)
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

                dbResult = dbAccess.UpdateAppointmentTime(vm, LoginUser);

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

        public IActionResult DownloadTemplate()
        {
            string FileName = "SDGs維護_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }

        private string? TransToStr(List<AppointmentTimeModel> models)
        {
            var groupedByWeek = models.GroupBy(t => t.Week).ToDictionary(g => g.Key, g => g.Select(t => t.Hour).ToList());

            return string.Join(", ", groupedByWeek.OrderBy(wh => wh.Key).Select(wh => $"{wh.Key}: [{string.Join(", ", wh.Value)}]"));


        }

    }
}
