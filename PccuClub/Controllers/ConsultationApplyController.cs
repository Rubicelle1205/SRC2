using Microsoft.AspNetCore.Mvc;
using WebPccuClub.Models;
using System.Diagnostics;
using WebPccuClub.Global;
using WebPccuClub.DataAccess;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Web.Mvc;
using System.Data;
using Utility;
using PccuClub.WebAuth;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.諮商初談申請表)]
    public class ConsultationApplyController : FBaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        ConsultationApplyDataAccess dbAccess = new ConsultationApplyDataAccess();
        MailUtil mail = new MailUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public ConsultationApplyController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.ddlAllNational = dbAccess.GetAllNational();
            ViewBag.ddlCounsellingStatus = dbAccess.GetAllCounsellingStatus();
            ViewBag.ddlSex = dbAccess.GetddlSex();

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

                if (vm.CreateModel.strCounsellingStatus.Contains("01") || vm.CreateModel.strCounsellingStatus.Contains("02") || vm.CreateModel.strCounsellingStatus.Contains("03"))
                {
                    DataTable dtReceiver = dbAccess.GetConsultationReceiver();
                    string MailBody = GenMailBody(vm, LoginUser);
                    string Receiver = dtReceiver.QueryFieldByDT("NotifyMail");

                    if (!string.IsNullOrEmpty(Receiver))
                    {
                        string [] arrReceiver = Receiver.Trim().TrimStartAndEnd().Split(',');

                        for (int i = 0; i <= arrReceiver.Length - 1; i++)
                        {
                            mail.ExecuteSendMail(arrReceiver[i].ToString(), "諮商緊急通知", MailBody, System.Net.Mail.MailPriority.High, null);
                        }
                    }
                }
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

        private string GenMailBody(ConsultationApplyViewModel vm, UserInfo loginUser)
        {
            string str = string.Empty;

            str = $@"<p>您好:</p>
                    <p>收到諮商申請，學生：{vm.CreateModel.Name} 有負面想法，請盡早安排諮商</p>";

            return str;
        }
    }
}