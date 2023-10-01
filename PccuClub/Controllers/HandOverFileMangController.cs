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
    [LogAttribute(LogActionChineseName.後台已上傳檔案)]
    public class HandOverFileMangController : BaseController
    {
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        HandOverFileMangDataAccess dbAccess = new HandOverFileMangDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public HandOverFileMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify();
            ViewBag.ddlLifeClass = dbAccess.GetAllLifeClass();

            HandOverFileMangViewModel vm = new HandOverFileMangViewModel();
            vm.ConditionModel = new HandOverFileMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, HandOverFileMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify();

            //HandOverFileMangViewModel vm = new HandOverFileMangViewModel();
            vm.EditModel = dbAccess.GetEditData(submitBtn);
            vm.EditModel.LstFile = dbAccess.GetFiles(vm.EditModel.HoDetailID);
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(HandOverFileMangViewModel vm)
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
        public async Task<IActionResult> EditOldData(HandOverFileMangViewModel vm)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("File"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass01", file);

                            HandOverFileMangFileModel model = new HandOverFileMangFileModel();
                            model.FilePath = strFilePath;

                            vm.EditModel.LstFile.Add(model);
                        }
                    }
                }

                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdateData(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "修改失敗";
                    return Json(vmRtn);
                }

                if (Request.Form.Files.Count > 0) {
                    dbResult = dbAccess.UpdateFile(vm, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "修改失敗";
                        return Json(vmRtn);
                    }

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

    }
}
