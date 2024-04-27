using Microsoft.AspNetCore.Mvc;
using WebPccuClub.Models;
using System.Diagnostics;
using WebPccuClub.Global;
using WebPccuClub.DataAccess;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Web.Mvc;
using System.Data;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.諮商初談申請表)]
    public class ConsultationApplyController : FBaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        ConsultationApplyDataAccess dbAccess = new ConsultationApplyDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public ConsultationApplyController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.ddlAllNational = dbAccess.GetAllNational();
            ViewBag.ddlCounsellingStatus = dbAccess.GetAllCounsellingStatus();

            ConsultationApplyViewModel vm = new ConsultationApplyViewModel();
            vm.CreateModel = new ConsultationApplyCreateModel();

            return View(vm);
        }

        public IActionResult Result()
        {
            ConsultationApplyViewModel vm = new ConsultationApplyViewModel();

            return View(vm);
        }

        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public IActionResult SaveNewData(ConsultationApplyViewModel vm)
        {
            try
            {
                DataTable dt = new DataTable();

                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.InsertData(vm, LoginUser, out dt);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                dbResult = dbAccess.UpdateAppointmentTime(vm, LoginUser, dt);

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
    }
}