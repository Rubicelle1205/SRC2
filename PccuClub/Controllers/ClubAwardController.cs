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
    [LogAttribute(LogActionChineseName.校外獲獎紀錄)]
    public class ClubAwardController : FBaseController
	{
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubAwardDataAccess dbAccess = new ClubAwardDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public ClubAwardController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify();

            ClubAwardViewModel vm = new ClubAwardViewModel();
            vm.ConditionModel = new ClubAwardConditionModel();
            vm.ConditionModel.SchoolYear = PublicFun.GetNowSchoolYear();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlAllClub = dbAccess.GetAllClub();
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify();

            ClubAwardViewModel vm = new ClubAwardViewModel();
            vm.CreateModel = new ClubAwardCreateModel();
            vm.CreateModel.SchoolYear = PublicFun.GetNowSchoolYear();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string id, ClubAwardViewModel vm)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify();

            //ClubAwardViewModel vm = new ClubAwardViewModel();
            vm.EditModel = dbAccess.GetEditData(id);
            vm.DetailModel = dbAccess.GetDetailData(id);
            
            return View(vm);
        }

        [Log(LogActionChineseName.匯入)]
        public IActionResult Upload()
        {
            ClubAwardViewModel vm = new ClubAwardViewModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ClubAwardViewModel vm)
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
        public async Task<IActionResult> SaveNewData(ClubAwardViewModel vm)
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
        public async Task<IActionResult> EditOldData(ClubAwardViewModel vm)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("Attachment"))
                        {
                            var file = Request.Form.Files.GetFile("EditModel.Attachment");

                            string strFilePath = await upload.UploadFileAsync("Award", file);

                            vm.EditModel.Attachment = strFilePath;
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
        public IActionResult ExportSearchResult(ClubAwardViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.校外獲獎紀錄, DateTime.Now.ToString("yyyyMMdd"));
            vm.ExcelModel = dbAccess.GetExportResult(vm.ConditionModel, LoginUser);

            if (vm.ExcelModel != null && vm.ExcelModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 20, 50, 20, 20, 20, 20, 20 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(ClubAwardExcelResultModel).GetProperties();

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

                    dataRow.CreateCell(0).SetCellValue(vm.ExcelModel[i].SchoolYear);
                    dataRow.CreateCell(1).SetCellValue(vm.ExcelModel[i].ClubID);
                    dataRow.CreateCell(2).SetCellValue(vm.ExcelModel[i].ClubCName);
                    dataRow.CreateCell(3).SetCellValue(vm.ExcelModel[i].AwdActName);
                    dataRow.CreateCell(4).SetCellValue(vm.ExcelModel[i].AwdType);
                    dataRow.CreateCell(5).SetCellValue(vm.ExcelModel[i].AwdName);
                    dataRow.CreateCell(6).SetCellValue(vm.ExcelModel[i].AwdDate.Value.ToString("yyyy/MM/dd"));
                    dataRow.CreateCell(7).SetCellValue(vm.ExcelModel[i].ActVerifyText);

                    foreach (var cell in dataRow.Cells)
                        cell.CellStyle = contentStyle;
                }

                MemoryStream ms = new MemoryStream();
                workbook.Write(ms, true);
                ms.Flush();
                ms.Position = 0;

                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
            }

            return View("Index", vm);

        }

        [LogAttribute(LogActionChineseName.下載template檔案)]
        public IActionResult DownloadTemplate()
        {
            string FileName = "獲獎名冊_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }
    }
}
