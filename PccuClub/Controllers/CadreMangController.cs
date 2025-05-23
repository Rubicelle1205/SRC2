﻿using DataAccess;
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
    [LogAttribute(LogActionChineseName.幹部名冊)]
    public class CadreMangController : BaseController
    {
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        CadreMangDataAccess dbAccess = new CadreMangDataAccess();
        UploadUtil upload = new UploadUtil();
        AuthUtil StdService = new AuthUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public CadreMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        #region 幹部

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            CadreMangViewModel vm = new CadreMangViewModel();
            vm.ConditionModel = new CadreMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllClub = dbAccess.GetAllClub();
            ViewBag.ddlAllSex = dbAccess.GetAllSex();

            CadreMangViewModel vm = new CadreMangViewModel();
            vm.CreateModel = new CadreMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, CadreMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllClub = dbAccess.GetAllClub();
            ViewBag.ddlAllSex = dbAccess.GetAllSex();

            //CadreMangViewModel vm = new CadreMangViewModel();
            vm.EditModel = dbAccess.GetEditData(submitBtn);
            return View(vm);
        }

        [Log(LogActionChineseName.匯入)]
        public IActionResult Upload()
        {
            CadreMangViewModel vm = new CadreMangViewModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(CadreMangViewModel vm)
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
        public async Task<IActionResult> SaveNewDataAsync(CadreMangViewModel vm)
        {
            try
            {
                if (!string.IsNullOrEmpty(vm.CreateModel.SNo))
                {
                    bool isStudent = await StdService.ChkStudent(vm.CreateModel.SNo);

                    dbAccess.WriteLog("[API]判斷學生身分:" + isStudent.ToString(), LoginUser, enumLogConst.Information);

                    if (!isStudent)
                    {
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = string.Format("學號:{0}不是學生身分", vm.CreateModel.SNo);
                        return Json(vmRtn);
                    }
                }

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
        public async Task<IActionResult> EditOldDataAsync(CadreMangViewModel vm)
        {
            try
            {
                if (!string.IsNullOrEmpty(vm.EditModel.SNo))
                {
                    bool isStudent = await StdService.ChkStudent(vm.EditModel.SNo);

                    dbAccess.WriteLog("[API]判斷學生身分:" + isStudent.ToString(), LoginUser, enumLogConst.Information);

                    if (!isStudent)
                    {
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = string.Format("學號:{0}不是學生身分", vm.EditModel.SNo);
                        return Json(vmRtn);
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
        public IActionResult ExportSearchResult(CadreMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.幹部名冊, DateTime.Now.ToString("yyyyMMdd"));
            vm.ResultModel = dbAccess.GetExportResult(vm.ConditionModel);

            if (vm.ResultModel != null && vm.ResultModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 50, 20, 20, 20, 20, 20, 30 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(CadreMangExcelResultModel).GetProperties();

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
                for (int i = 0; i <= vm.ResultModel.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    dataRow.CreateCell(0).SetCellValue(vm.ResultModel[i].SchoolYear);
                    dataRow.CreateCell(1).SetCellValue(vm.ResultModel[i].ClubName);
                    dataRow.CreateCell(2).SetCellValue(vm.ResultModel[i].UserName);
                    dataRow.CreateCell(3).SetCellValue(vm.ResultModel[i].Department);
                    dataRow.CreateCell(4).SetCellValue(vm.ResultModel[i].CadreName);
                    dataRow.CreateCell(5).SetCellValue(vm.ResultModel[i].SDuring.Value.ToString("yyyy/MM/dd"));
                    dataRow.CreateCell(6).SetCellValue(vm.ResultModel[i].EDuring.Value.ToString("yyyy/MM/dd"));
                    dataRow.CreateCell(7).SetCellValue(vm.ResultModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm:ss"));

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

        [LogAttribute(LogActionChineseName.匯入Excel)]
        public async Task<IActionResult> ImportExcelAsync(CadreMangViewModel vm)
        {
            if (vm.File != null && vm.File.Length > 0)
            {
                string fileExtension = Path.GetExtension(vm.File.FileName);

                if (fileExtension != ".xlsx")
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "選擇檔案格式錯誤";
                    return Json(vmRtn);
                }

                if (!vm.File.FileName.Contains(LogActionChineseName.幹部名冊.ToString()))
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "選擇檔案錯誤";
                    return Json(vmRtn);
                }

                List<CadreMangImportExcelResultModel> LstExcel = new List<CadreMangImportExcelResultModel>();

                using (Stream stream = vm.File.OpenReadStream())
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        for (int j = 0; j <= row.Count() - 1; j++)
                        {
                            row.GetCell(j).SetCellType(CellType.String);
                        }

                        if (row != null)
                        {
                            string Sex = "";
                            string SDring = "";
                            string EDring = "";

                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstClubID = dbAccess.GetAllClub();
                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstSex = dbAccess.GetAllSex();

                            if (!LstClubID.Any(m => m.Value == row.GetCell(1)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(1).ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }

                            if (!LstSex.Any(m => m.Text == row.GetCell(4)?.ToString()))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(4).ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }
                            else
                            {
                                Sex = LstSex.Where(m => m.Text == row.GetCell(4)?.ToString()).FirstOrDefault().Value;
                            }

                            if (!PublicFun.ChkDateFormat(row.GetCell(9).ToString().TrimStartAndEnd(), out SDring))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(9).ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }

                            if (!PublicFun.ChkDateFormat(row.GetCell(10).ToString().TrimStartAndEnd(), out EDring))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(10).ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }

                            //檢查學號
                            if (!string.IsNullOrEmpty(row.GetCell(3)?.StringCellValue.TrimStartAndEnd()))
                            {
                                bool isStudent = await StdService.ChkStudent(row.GetCell(3)?.StringCellValue.TrimStartAndEnd());

                                dbAccess.WriteLog("[API]判斷學生身分:" + isStudent.ToString(), LoginUser, enumLogConst.Information);

                                if (!isStudent)
                                {
                                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                    vmRtn.ErrorMsg = string.Format("學號:{0}不是學生身分", row.GetCell(3)?.StringCellValue.TrimStartAndEnd());
                                    return Json(vmRtn);
                                }
                            }
                            else
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("資料行{0}學號為空", i + 1);
                                return Json(vmRtn);
                            }

                            CadreMangImportExcelResultModel excel = new CadreMangImportExcelResultModel
                            {
                                SchoolYear = row.GetCell(0)?.StringCellValue.TrimStartAndEnd(),
                                ClubID = row.GetCell(1)?.StringCellValue.TrimStartAndEnd(),
                                UserName = row.GetCell(2)?.StringCellValue.TrimStartAndEnd(),
                                SNo = row.GetCell(3)?.StringCellValue.TrimStartAndEnd(),
                                Sex = Sex,
                                CellPhone = row.GetCell(5)?.StringCellValue.TrimStartAndEnd(),
                                EMail = row.GetCell(6)?.StringCellValue.TrimStartAndEnd(),
                                CadreName = row.GetCell(7)?.StringCellValue.TrimStartAndEnd(),
                                Department = row.GetCell(8)?.StringCellValue.TrimStartAndEnd(),
                                SDuring = SDring,
                                EDuring = EDring,
                                Memo = row.GetCell(11)?.StringCellValue.TrimStartAndEnd()
                            };

                            LstExcel.Add(excel);
                        }
                    }
                }

                dbAccess.DbaInitialTransaction();
                if (LstExcel.Count > 0)
                {
                    var dbResult = dbAccess.ImportData(LstExcel, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "上傳失敗";
                    }
                }

                dbAccess.DbaCommit();
            }
            else
            {
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "請選擇檔案上傳";
            }

            return Json(vmRtn);
        }

        [LogAttribute(LogActionChineseName.下載template檔案)]
        public IActionResult DownloadTemplate()
        {
            string FileName = "幹部名冊_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }

        #endregion

        #region 個資

        [Log(LogActionChineseName.首頁)]
        public IActionResult PersonalConsentIndex()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            CadreMangViewModel vm = new CadreMangViewModel();
            vm.PersonalConsentConditionModel = new CadreMangPersonalConsentConditionModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetPersonalConsentSearchResult(CadreMangViewModel vm)
        {
            vm.PersonalConsentResultModel = dbAccess.GetPersonalConsentSearchResult(vm.PersonalConsentConditionModel).ToList();

            #region 分頁
            vm.PersonalConsentConditionModel.TotalCount = vm.PersonalConsentResultModel.Count();
            int StartRow = vm.PersonalConsentConditionModel.Page * vm.PersonalConsentConditionModel.PageSize;
            vm.PersonalConsentResultModel = vm.PersonalConsentResultModel.Skip(StartRow).Take(vm.PersonalConsentConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchPersonalConsentResultPartial", vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult PersonalConsentEdit(string submitBtn, CadreMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            //CadreMangViewModel vm = new CadreMangViewModel();
            vm.PersonalConsentEditModel = dbAccess.GetPersonalConsentEditData(submitBtn);
            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> PersonalConsentEditOldData(CadreMangViewModel vm)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("PersonalConsent"))
                        {
                            var file = Request.Form.Files.GetFile("PersonalConsentEditModel.FilePath");

                            string strFilePath = await upload.UploadFileAsync("PersonalConsent", file);

                            vm.PersonalConsentEditModel.FilePath = strFilePath;
                        }
                    }
                }

                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.CadreMangUpdatePersonalConsentData(vm, LoginUser);

                if (!dbResult.isSuccess && dbResult.ErrorCode != dbErrorCode._EC_NotAffect)
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
                vmRtn.ErrorMsg = "新增失敗" + ex.Message;
                return Json(vmRtn);
            }

            return Json(vmRtn);
        }

        [Log(LogActionChineseName.刪除)]
        [ValidateInput(false)]
        public IActionResult PersonalConsentDelete(string Ser)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.DeletetPersonalConsentData(Ser);

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
        #endregion
    }
}
