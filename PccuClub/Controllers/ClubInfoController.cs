﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Asn1.Ocsp;
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
    [LogAttribute(LogActionChineseName.前台社團簡介)]
    public class ClubInfoController : BaseController
    {
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubInfoDataAccess dbAccess = new ClubInfoDataAccess();
        UploadUtil upload = new UploadUtil();

		private readonly IHostingEnvironment hostingEnvironment;


		public ClubInfoController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

		[Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            return View();
        }

		#region 社團簡介

		[Log(LogActionChineseName.我的社團簡介)]
		public IActionResult MyClubIndex()
		{
			ClubInfoViewModel vm = new ClubInfoViewModel();
			vm.MyClubEditModel = dbAccess.GetEditData(LoginUser.LoginId);
			return View(vm);
		}

		[Log(LogActionChineseName.我的社團簡介編輯)]
		public IActionResult MyClubEdit()
		{
			ClubInfoViewModel vm = new ClubInfoViewModel();
			vm.MyClubEditModel = dbAccess.GetEditData(LoginUser.LoginId);
			return View(vm);
		}
		 
		[LogAttribute(LogActionChineseName.匯出Excel)]
		public IActionResult ExportSearchResult(ClubInfoViewModel vm)
		{
			string FileName = string.Format("{0}_{1}", LoginUser.LoginId + LogActionChineseName.社團基本資料, DateTime.Now.ToString("yyyyMMdd"));
			vm.ExcelResultModel = dbAccess.GetExportResult(LoginUser.LoginId);

			if (vm.ExcelResultModel != null)
			{
				IWorkbook workbook = new XSSFWorkbook();
				List<int> LstWidth = new List<int> { 20, 50, 20, 20, 20, 20, 20, 30, 40, 40, 40, 40, 40, 40, 40 };

				ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

				var properties = typeof(MyClubExcelModel).GetProperties();

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

				string host = HttpContext.Request.Host.Value + "/";

				//設定資料
				for (int i = 0; i <= vm.ExcelResultModel.Count - 1; i++)
				{


					IRow dataRow = sheet.CreateRow(i + 1);

					dataRow.CreateCell(0).SetCellValue(vm.ExcelResultModel[i].ClubId);
					dataRow.CreateCell(1).SetCellValue(vm.ExcelResultModel[i].ClubCName);
					dataRow.CreateCell(2).SetCellValue(vm.ExcelResultModel[i].ClubEName);
					dataRow.CreateCell(3).SetCellValue(vm.ExcelResultModel[i].SchoolYear);
					dataRow.CreateCell(4).SetCellValue(vm.ExcelResultModel[i].LifeClassText);
					dataRow.CreateCell(5).SetCellValue(vm.ExcelResultModel[i].ClubClassText);
					dataRow.CreateCell(6).SetCellValue(vm.ExcelResultModel[i].Address);
					dataRow.CreateCell(7).SetCellValue(vm.ExcelResultModel[i].Tel);
					dataRow.CreateCell(8).SetCellValue(vm.ExcelResultModel[i].EMail);
					dataRow.CreateCell(9).SetCellValue(vm.ExcelResultModel[i].Social1);
					dataRow.CreateCell(10).SetCellValue(vm.ExcelResultModel[i].Social2);
					dataRow.CreateCell(11).SetCellValue(vm.ExcelResultModel[i].Social3);
					dataRow.CreateCell(12).SetCellValue(host + vm.ExcelResultModel[i].LogoPath);
					dataRow.CreateCell(13).SetCellValue(host + vm.ExcelResultModel[i].ActImgPath);
					dataRow.CreateCell(14).SetCellValue(vm.ExcelResultModel[i].ShortInfo);

					foreach (var cell in dataRow.Cells)
						cell.CellStyle = contentStyle;
				}

				MemoryStream ms = new MemoryStream();
				workbook.Write(ms, true);
				ms.Flush();
				ms.Position = 0;

				return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
			}

			return View("MyClubIndex", vm);
		}

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> MyClubEditOldData(ClubInfoViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("LogoPath"))
                        {
                            var file = Request.Form.Files.GetFile("MyClubEditModel.LogoPath");

                            string strFilePath = await upload.UploadFileAsync("LogoPath", file);

                            vm.MyClubEditModel.LogoPath = strFilePath;
                        }
                        else if (Request.Form.Files[i].Name.Contains("ActImgPath"))
                        {
                            var file = Request.Form.Files.GetFile("MyClubEditModel.ActImgPath");

                            string strFilePath = await upload.UploadFileAsync("ActImgPath", file);

                            vm.MyClubEditModel.ActImgPath = strFilePath;
                        }
                    }
                }

                string EncryptPw = String.Empty;

                var dbResult = dbAccess.MyClubUpdateData(vm, LoginUser);

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

        #endregion


        #region 計畫表

        [Log(LogActionChineseName.活動績效管理)]
        public IActionResult ClubScheduleIndex()
        {
			ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

			ClubInfoViewModel vm = new ClubInfoViewModel();
			vm.ClubScheduleConditionModel = new ClubScheduleConditionModel();
			vm.ClubScheduleConditionModel.SchoolYear = PublicFun.GetNowSchoolYear();

            return View(vm);
        }

		[LogAttribute(LogActionChineseName.查詢)]
		public IActionResult GetScheduleSearchResult(ClubInfoViewModel vm)
		{
			vm.ClubScheduleResultModel = dbAccess.GetScheduleSearchResult(vm.ClubScheduleConditionModel, LoginUser).ToList();

			#region 分頁
			vm.ClubScheduleConditionModel.TotalCount = vm.ClubScheduleResultModel.Count();
			int StartRow = vm.ClubScheduleConditionModel.Page * vm.ClubScheduleConditionModel.PageSize;
			vm.ClubScheduleResultModel = vm.ClubScheduleResultModel.Skip(StartRow).Take(vm.ClubScheduleConditionModel.PageSize).ToList();
			#endregion

			return PartialView("_SearchClubScheduleResultPartial", vm);
		}

		[LogAttribute(LogActionChineseName.新增)]
		public IActionResult ClubScheduleCreate()
		{
			ViewBag.ddlAllActType = dbAccess.GetAllActType();

			ClubInfoViewModel vm = new ClubInfoViewModel();
			vm.ClubScheduleCreateModel = new ClubScheduleCreateModel();
			vm.ClubScheduleCreateModel.SchoolYear = PublicFun.GetNowSchoolYear();

			return View(vm);
		}

        [LogAttribute(LogActionChineseName.新增儲存)]
        public IActionResult ClubScheduleSaveNewData(ClubInfoViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.ClubScheduleInserNewData(vm, LoginUser);

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


        [Log(LogActionChineseName.編輯)]
        public IActionResult ClubScheduleEdit(string id, ClubInfoViewModel vm)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            ViewBag.ddlAllActType = dbAccess.GetAllActType();
            ViewBag.ddlAllActHoldType = dbAccess.GetAllActHoldType();


            vm.ClubScheduleEditModel = dbAccess.GetClubScheduleEditData(id);

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> ClubScheduleEditOldDataAsync(ClubInfoViewModel vm)
        {
            try
            {

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("Attachment"))
                        {
                            var file = Request.Form.Files.GetFile("ClubScheduleEditModel.Attachment");

                            string strFilePath = await upload.UploadFileAsync("Schedule", file);

                            vm.ClubScheduleEditModel.Attachment = strFilePath;
                        }
                    }
                }










                        dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.ClubScheduleUpdateData(vm, LoginUser);

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
        #endregion



    }
}