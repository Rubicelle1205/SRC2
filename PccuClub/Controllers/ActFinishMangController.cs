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
    [LogAttribute(LogActionChineseName.活動結案管理)]
    public class ActFinishMangController : BaseController
	{
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        ActFinishMangDataAccess dbAccess = new ActFinishMangDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public ActFinishMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify("1");    //不篩選出批次單

            ActFinishMangViewModel vm = new ActFinishMangViewModel();
            vm.ConditionModel = new ActFinishMangConditionModel();

            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            
            ViewBag.ddlAllClub = dbAccess.GetAllClub();
            ViewBag.ddlAllActData = dbAccess.GetAllActData();
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear(1);
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify("1");

            ActFinishMangViewModel vm = new ActFinishMangViewModel();
            vm.CreateModel = new ActFinishMangCreateModel();
            
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, ActFinishMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlAllClub = dbAccess.GetAllClub();
            ViewBag.ddlAllActData = dbAccess.GetAllActData();
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear(1);
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify("1");

            vm.EditModel = dbAccess.GetEditData(submitBtn);
            vm.EditModel.PersonModel = dbAccess.GetDetailData(submitBtn);
            
            
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(ActFinishMangViewModel vm)
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
        public async Task<IActionResult> SaveNewData(ActFinishMangViewModel vm)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("ElseFile"))
                        {
                            var file = Request.Form.Files.GetFile("CreateModel.ElseFile");

                            string strFilePath = await upload.UploadFileAsync("ActFinish", file);

                            vm.CreateModel.ElseFile = strFilePath;
                        }
                    }
                }

                string ActDetailId = dbAccess.GetActDetailID(vm.CreateModel.ActID);
                vm.CreateModel.ActDetailId = ActDetailId;

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
                    string ActFinishId = dt.QueryFieldByDT("ActFinishId");
                    List<PersonModel> LstActFinishPersonDetail = new List<PersonModel>();

                    using (Stream stream = vm.File.OpenReadStream())
                    {
                        XSSFWorkbook workbook = new XSSFWorkbook(stream);
                        ISheet sheet = workbook.GetSheetAt(0);

                        for (int i = 1; i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);

                            row.GetCell(0).SetCellType(CellType.String);
                            row.GetCell(1).SetCellType(CellType.String);

                            if (row != null)
                            {
                                PersonModel excel = new PersonModel
                                {
                                    SNO = row.GetCell(0)?.StringCellValue.TrimStartAndEnd(),
                                    Name = row.GetCell(1)?.StringCellValue.TrimStartAndEnd()
                                };

                                LstActFinishPersonDetail.Add(excel);
                            }
                        }
                    }

                    dbResult = dbAccess.InsertDetailData(ActFinishId, LstActFinishPersonDetail, LoginUser);

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
        public async Task<IActionResult> EditOldData(ActFinishMangViewModel vm)
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("ElseFile"))
                        {
                            var file = Request.Form.Files.GetFile("EditModel.ElseFile");

                            string strFilePath = await upload.UploadFileAsync("ActFinish", file);

                            vm.EditModel.ElseFile = strFilePath;
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
                    List<PersonModel> LstAwdDetail = new List<PersonModel>();

                    using (Stream stream = vm.File.OpenReadStream())
                    {
                        XSSFWorkbook workbook = new XSSFWorkbook(stream);
                        ISheet sheet = workbook.GetSheetAt(0);

                        for (int i = 1; i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);

                            row.GetCell(0).SetCellType(CellType.String);
                            row.GetCell(1).SetCellType(CellType.String);

                            if (row != null)
                            {
                                PersonModel excel = new PersonModel
                                {
                                    SNO = row.GetCell(0)?.StringCellValue.TrimStartAndEnd(),
                                    Name = row.GetCell(1)?.StringCellValue.TrimStartAndEnd()
                                };

                                LstAwdDetail.Add(excel);
                            }
                        }
                    }

                    dbResult = dbAccess.EditDetailData(vm.EditModel.ActFinishId, LstAwdDetail, LoginUser);

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

                dbResult = dbAccess.DeletetPersonData(Ser);

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
        public IActionResult ExportSearchResult(ActFinishMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.活動結案管理, DateTime.Now.ToString("yyyyMMdd"));
            vm.ExcelModel = dbAccess.GetExportResult(vm.ConditionModel);

            if (vm.ExcelModel != null && vm.ExcelModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 20, 50, 20, 20, 20, 20, 20 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(ActFinishMangExcelModel).GetProperties();

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

                    dataRow.CreateCell(0).SetCellValue(vm.ExcelModel[i].ClubId);
                    dataRow.CreateCell(1).SetCellValue(vm.ExcelModel[i].SchoolYear);
                    dataRow.CreateCell(2).SetCellValue(vm.ExcelModel[i].ClubCName);
                    dataRow.CreateCell(3).SetCellValue(vm.ExcelModel[i].ActID);
                    dataRow.CreateCell(4).SetCellValue(vm.ExcelModel[i].ActName);
                    dataRow.CreateCell(5).SetCellValue(vm.ExcelModel[i].ActVerifyText);
                    dataRow.CreateCell(6).SetCellValue(vm.ExcelModel[i].Created.Value.ToString("yyyy/MM/dd"));

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

        [LogAttribute(LogActionChineseName.下載template檔案)]
        public IActionResult DownloadTemplate()
        {
            string FileName = "學號匯入_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportPersonData(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            string FileName = string.Format("{0}_{1}", "參與學號", DateTime.Now.ToString("yyyyMMdd"));

            ActFinishMangViewModel vm = new ActFinishMangViewModel();
            vm.PersonModel = dbAccess.GetPersonData(id);

            if (vm.PersonModel != null && vm.PersonModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 50 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(PersonModel).GetProperties();

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
                for (int i = 0; i <= vm.PersonModel.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    dataRow.CreateCell(0).SetCellType(CellType.String);
                    dataRow.CreateCell(1).SetCellType(CellType.String);

                    dataRow.GetCell(0).SetCellValue(vm.PersonModel[i].SNO);
                    dataRow.GetCell(1).SetCellValue(vm.PersonModel[i].Name);

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

        [LogAttribute(LogActionChineseName.匯出Excel)]
        public IActionResult ExportAllSNOResult(ActFinishMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", "參與學號", DateTime.Now.ToString("yyyyMMdd"));

            vm.ALLPersonModel = dbAccess.GetALLPersonResult(vm.ConditionModel);

            if (vm.ALLPersonModel != null && vm.ALLPersonModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 20, 50, 50 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(ALLPersonModel).GetProperties();

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
                for (int i = 0; i <= vm.ALLPersonModel.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    dataRow.CreateCell(0).SetCellType(CellType.String);
                    dataRow.CreateCell(1).SetCellType(CellType.String);
                    dataRow.CreateCell(2).SetCellType(CellType.String);
                    dataRow.CreateCell(3).SetCellType(CellType.String);

                    dataRow.GetCell(0).SetCellValue(vm.ALLPersonModel[i].SchoolYear);
                    dataRow.GetCell(1).SetCellValue(vm.ALLPersonModel[i].ActID);
                    dataRow.GetCell(2).SetCellValue(vm.ALLPersonModel[i].ActName);
                    dataRow.GetCell(3).SetCellValue(vm.ALLPersonModel[i].SNO);

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
    }
}
