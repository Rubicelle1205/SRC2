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
    [LogAttribute(LogActionChineseName.借用前台MenuBoard維護)]
    public class BorrowMenuBoardMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        BorrowMenuBoardMangDataAccess dbAccess = new BorrowMenuBoardMangDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public BorrowMenuBoardMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            BorrowMenuBoardMangViewModel vm = new BorrowMenuBoardMangViewModel();
            vm.ConditionModel = new BorrowMenuBoardMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, BorrowMenuBoardMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            //BorrowMenuBoardMangViewModel vm = new BorrowMenuBoardMangViewModel();
            vm.EditModel = dbAccess.GetEditData(submitBtn);
            return View(vm);
        }


        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(BorrowMenuBoardMangViewModel vm)
        {
            //LoginSystemCode >> MenuBoardCode
            //01 >> null, 02 >> 01, 03 >> 02, 04 >> 03, 05 >> 04
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel, "02").ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }


        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> EditOldData(BorrowMenuBoardMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("IconPath"))
                        {
                            var file = Request.Form.Files.GetFile("EditModel.IconPath");

                            string strFilePath = await upload.UploadFileAsync("IconPath", file);

                            vm.EditModel.IconPath = strFilePath;
                        }
                    }
                }

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

    }
}
