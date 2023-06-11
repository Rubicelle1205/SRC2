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
    [LogAttribute(LogActionChineseName.前台活動報備)]
    public class ClubActReportController : BaseController
    {
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubActReportDataAccess dbAccess = new ClubActReportDataAccess();
		ActListMangDataAccess dbAccess2 = new ActListMangDataAccess();
		UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public ClubActReportController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubActReportViewModel vm = new ClubActReportViewModel();
            vm.ConditionModel = new ClubActReportConditionModel();
            vm.ConditionModel.SchoolYear = PublicFun.GetNowSchoolYear();
            return View(vm);
        }

        //[Log(LogActionChineseName.新增)]
        //public IActionResult Create()
        //{
        //    ViewBag.ddlAllClub = dbAccess.GetAllClub();
        //    ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
        //    ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify();

        //    ClubActReportViewModel vm = new ClubActReportViewModel();
        //    vm.CreateModel = new ClubActReportCreateModel();
        //    vm.CreateModel.SchoolYear = PublicFun.GetNowSchoolYear();
        //    return View(vm);
        //}

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string id, ClubActReportViewModel vm)
        {
            ViewBag.ddlAllSDGs = dbAccess.GetAllSDGs();

            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            //ClubActReportViewModel vm = new ClubActReportViewModel();
            vm.EditModel = dbAccess.GetEditData(id);
			vm.EditModel.LstActRundown = dbAccess2.GetEditRundownData(id);
			vm.EditModel.LstProposal = dbAccess2.GetEditProposalData(id);
			vm.EditModel.LstOutSideFile = dbAccess2.GetEditOutSideFileData(id);

			return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ClubActReportViewModel vm)
        {
            vm.ResultModel = dbAccess.GetSearchResult(vm.ConditionModel, LoginUser).ToList();

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> SaveNewData(ClubActReportViewModel vm)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("Attachment"))
                        {
                            var file = Request.Form.Files.GetFile("CreateModel.Attachment");

                            string strFilePath = await upload.UploadFileAsync("Award", file);

                            vm.CreateModel.Attachment = strFilePath;
                        }
                    }
                }


                dbAccess.DbaInitialTransaction();
                DataTable dt = new DataTable();

                var dbResult = dbAccess.InsertData(vm, LoginUser, out dt);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                if (vm.File != null)
                {
                    string AwdID = dt.QueryFieldByDT("AwdID");
                    List<AwdDetailModel> LstAwdDetail = new List<AwdDetailModel>();

                    using (Stream stream = vm.File.OpenReadStream())
                    {
                        XSSFWorkbook workbook = new XSSFWorkbook(stream);
                        ISheet sheet = workbook.GetSheetAt(0);

                        for (int i = 1; i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);

                            if (row != null)
                            {
                                AwdDetailModel excel = new AwdDetailModel
                                {
                                    Department = row.GetCell(0)?.StringCellValue.TrimStartAndEnd(),
                                    Name = row.GetCell(1)?.StringCellValue.TrimStartAndEnd(),
                                    SNO = row.GetCell(2)?.StringCellValue.TrimStartAndEnd()
                                };

                                LstAwdDetail.Add(excel);
                            }
                        }
                    }
                    dbResult = dbAccess.InsertDetailData(AwdID, LstAwdDetail, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "新增失敗";
                        return Json(vmRtn);
                    }
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
        public async Task<IActionResult> EditOldData(ClubActReportViewModel vm)
        {
            try
            {
                //if (Request.Form.Files.Count > 0)
                //{
                //    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                //    {
                //        if (Request.Form.Files[i].Name.Contains("Attachment"))
                //        {
                //            var file = Request.Form.Files.GetFile("EditModel.Attachment");

                //            string strFilePath = await upload.UploadFileAsync("Award", file);

                //            vm.EditModel.Attachment = strFilePath;
                //        }
                //    }
                //}

                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdateData(vm, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "修改失敗";
                    return Json(vmRtn);
                }

                if (vm.File != null)
                {
                    List<AwdDetailModel> LstAwdDetail = new List<AwdDetailModel>();

                    using (Stream stream = vm.File.OpenReadStream())
                    {
                        XSSFWorkbook workbook = new XSSFWorkbook(stream);
                        ISheet sheet = workbook.GetSheetAt(0);

                        for (int i = 1; i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);

                            if (row != null)
                            {
                                AwdDetailModel excel = new AwdDetailModel
                                {
                                    Department = row.GetCell(0)?.StringCellValue.TrimStartAndEnd(),
                                    Name = row.GetCell(1)?.StringCellValue.TrimStartAndEnd(),
                                    SNO = row.GetCell(2)?.StringCellValue.TrimStartAndEnd()
                                };

                                LstAwdDetail.Add(excel);
                            }
                        }
                    }
                    dbResult = dbAccess.EditDetailData(vm, LstAwdDetail, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "新增失敗";
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
