using DataAccess;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming.Values;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Utility;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.後台已填寫表單)]
    public class HandOverDocMangController : BaseController
    {
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        HandOverDocMangDataAccess dbAccess = new HandOverDocMangDataAccess();
        ClubHandoverDataAccess ClubdbAccess = new ClubHandoverDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public HandOverDocMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllDocType = dbAccess.GetAllDocType();

            HandOverDocMangViewModel vm = new HandOverDocMangViewModel();
            vm.ConditionModel = new HandOverDocMangConditionModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(HandOverDocMangViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
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

        public IActionResult HistorySwitch(string id, string docType)
        {
            switch (docType)
            {
                case "01":
                    return Redirect($"/HandOverDocMang/HandOver0101?id={id}");
                case "02":
                    return Redirect($"/HandOverDocMang/HandOver0102?id={id}");
                case "03":
                    return Redirect($"/HandOverDocMang/HandOver0103?id={id}");
                case "04":
                    return Redirect($"/HandOverDocMang/HandOver0204?id={id}");
                case "05":
                    return Redirect($"/HandOverDocMang/HandOver0205?id={id}");
                case "06":
                    return Redirect($"/HandOverDocMang/HandOver0206?id={id}");
                case "07":
                    return Redirect($"/HandOverDocMang/HandOver0307?id={id}");
                case "08":
                    return Redirect($"/HandOverDocMang/HandOver0308?id={id}");
                case "09":
                    return Redirect($"/HandOverDocMang/HandOver0309?id={id}");
                default:
                    Redirect("Index");
                    break;
            }

            return View();
        }

        #region 0101


        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0101(string id)
        {
            ViewBag.ddlAgree = dbAccess.getAllAgree();
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0101Model = new ClubHandover0101ViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0101Model = ClubdbAccess.GetHandover0101Data(id, LoginUser);
            }

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> Save0101(ClubHandoverViewModel vm)
        {
            try
            {
                var dbResult = ClubdbAccess.Update0101(vm, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        ClubdbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "儲存失敗";
                        return Json(vmRtn);
                    }
                    ClubdbAccess.DbaCommit();
                    return Json(vmRtn);
            }
            catch (Exception ex)
            {
                ClubdbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }
        }

        #endregion

        #region 0102


        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0102(string id)
        {
            ViewBag.ddlElectionType = dbAccess.getAllElectionType();
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0102Model = new ClubHandover0102ViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0102Model = ClubdbAccess.GetHandover0102Data(id, LoginUser);
            }

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> Save0102(ClubHandoverViewModel vm)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("Handover0102Model.MeetingRecord"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass02", file);
                            vm.Handover0102Model.MeetingRecordName = file.FileName;
                            vm.Handover0102Model.MeetingRecord = strFilePath;
                        }
                        else if (Request.Form.Files[i].Name.Contains("Handover0102Model.MeetingSign"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass02", file);
                            vm.Handover0102Model.MeetingSignName = file.FileName;
                            vm.Handover0102Model.MeetingSign = strFilePath;
                        }
                    }
                }

                var dbResult = ClubdbAccess.Update0102(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    ClubdbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }
                ClubdbAccess.DbaCommit();
                return Json(vmRtn);
            }
            catch (Exception ex)
            {
                ClubdbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }
        }

        #endregion

        #region 0103


        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0103(string id)
        {
            ViewBag.ddlClubBuild = dbAccess.GetClubBuild();
            ViewBag.ddlSex = dbAccess.GetAllSex();
            ViewBag.ddldentityType = dbAccess.GetAllIdentityType();
            ViewBag.ddlConform = dbAccess.GetAllConform();
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0103Model = new ClubHandover0103ViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0103Model = ClubdbAccess.GetHandover0103Data(id, LoginUser);
            }

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> Save0103(ClubHandoverViewModel vm)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("Handover0103Model.Transcript"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass03", file);
                            vm.Handover0103Model.TranscriptName = file.FileName;
                            vm.Handover0103Model.Transcript = strFilePath;
                        }
                    }
                }

                var dbResult = ClubdbAccess.Update0103(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    ClubdbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }
                ClubdbAccess.DbaCommit();
                return Json(vmRtn);
            }
            catch (Exception ex)
            {
                ClubdbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }
        }

        #endregion

        #region 0204


        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0204(string id)
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0204Model = new ClubHandover0204ViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0204Model = ClubdbAccess.GetHandover0204Data(id, LoginUser);
            }

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> Save0204(ClubHandoverViewModel vm)
        {
            try
            {

                var dbResult = ClubdbAccess.Update0204(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    ClubdbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }
                ClubdbAccess.DbaCommit();
                return Json(vmRtn);
            }
            catch (Exception ex)
            {
                ClubdbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }
        }

        #endregion

        #region 0205


        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver025(string id)
        {
            ViewBag.ddlYesOrNo = dbAccess.GetYesOrNo();
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0205Model = new ClubHandover0205ViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0205Model = ClubdbAccess.GetHandover0205Data(id, LoginUser);
            }

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> Save0205(ClubHandoverViewModel vm)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("Handover0205Model.UseRecord"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass05", file);
                            vm.Handover0205Model.UseRecordName = file.FileName;
                            vm.Handover0205Model.UseRecord = strFilePath;
                        }
                        else if (Request.Form.Files[i].Name.Contains("Handover0205Model.SchoolProperty"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass05", file);
                            vm.Handover0205Model.SchoolPropertyName = file.FileName;
                            vm.Handover0205Model.SchoolProperty = strFilePath;
                        }
                        else if (Request.Form.Files[i].Name.Contains("Handover0205Model.ClubProperty"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass05", file);
                            vm.Handover0205Model.ClubPropertyName = file.FileName;
                            vm.Handover0205Model.ClubProperty = strFilePath;
                        }
                    }
                }

                var dbResult = ClubdbAccess.Update0205(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    ClubdbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }
                ClubdbAccess.DbaCommit();
                return Json(vmRtn);
            }
            catch (Exception ex)
            {
                ClubdbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }
        }

        #endregion

        #region 0206


        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0206(string id)
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0206Model = new ClubHandover0206ViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0206Model = ClubdbAccess.GetHandover0206Data(id, LoginUser);
            }

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> Save0206(ClubHandoverViewModel vm)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("Handover0206Model.Sheet"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass06", file);
                            vm.Handover0206Model.SheetName = file.FileName;
                            vm.Handover0206Model.Sheet = strFilePath;
                        }
                        else if (Request.Form.Files[i].Name.Contains("Handover0206Model.InnerFile"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass06", file);
                            vm.Handover0206Model.InnerFileName = file.FileName;
                            vm.Handover0206Model.InnerFile = strFilePath;
                        }
                    }
                }

                var dbResult = ClubdbAccess.Update0206(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    ClubdbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }
                ClubdbAccess.DbaCommit();
                return Json(vmRtn);
            }
            catch (Exception ex)
            {
                ClubdbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }
        }

        #endregion

        #region 0307


        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0307(string id)
        {
            ViewBag.ddlSex = dbAccess.GetAllSex();
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0307Model = new ClubHandover0307ViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0307Model = ClubdbAccess.GetHandover0307Data(id, LoginUser);
            }

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> Save0307(ClubHandoverViewModel vm)
        {
            try
            {

                var dbResult = ClubdbAccess.Update0307(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    ClubdbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }
                ClubdbAccess.DbaCommit();
                return Json(vmRtn);
            }
            catch (Exception ex)
            {
                ClubdbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }
        }

        #endregion

        #region 0308


        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0308(string id)
        {
            ViewBag.ddlSex = dbAccess.GetAllSex();
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0308Model = new ClubHandover0308ViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0308Model = ClubdbAccess.GetHandover0308Data(id, LoginUser);
            }

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> Save0308(ClubHandoverViewModel vm)
        {
            try
            {

                var dbResult = ClubdbAccess.Update0308(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    ClubdbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }
                ClubdbAccess.DbaCommit();
                return Json(vmRtn);
            }
            catch (Exception ex)
            {
                ClubdbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }
        }

        #endregion

        #region 0309


        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0309(string id)
        {
            ViewBag.ddlSex = dbAccess.GetAllSex();
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0309Model = new ClubHandover0309ViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0309Model = ClubdbAccess.GetHandover0309Data(id, LoginUser);
            }

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> Save0309(ClubHandoverViewModel vm)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("Handover0309Model.BookCover"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass09", file);
                            vm.Handover0309Model.BookCoverName = file.FileName;
                            vm.Handover0309Model.BookCover = strFilePath;
                        }
                    }
                }

                var dbResult = ClubdbAccess.Update0309(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    ClubdbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }
                ClubdbAccess.DbaCommit();
                return Json(vmRtn);
            }
            catch (Exception ex)
            {
                ClubdbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "修改失敗" + ex.Message;
                return Json(vmRtn);
            }
        }

        #endregion

    }
}
