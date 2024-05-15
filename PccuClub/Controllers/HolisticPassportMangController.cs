using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
    [LogAttribute(LogActionChineseName.全人學習護照管理)]
    public class HolisticPassportMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        HolisticPassportMangDataAccess dbAccess = new HolisticPassportMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public HolisticPassportMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlAllSchoolYear = dbAccess.GetSchoolYear(2);
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify("1");

            HolisticPassportMangViewModel vm = new HolisticPassportMangViewModel();
            vm.ConditionModel = new HolisticPassportMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlAllClub = dbAccess.GetAllClubID();
            ViewBag.ddlAllActName = dbAccess.GetAllActName();
            ViewBag.ddlHolisticMainClass = dbAccess.GetddlHolisticMainClass();
            ViewBag.ddlHolisticSecondClass = dbAccess.GetddlHolisticSecondClass();
            ViewBag.ddlHolisticThridClass = dbAccess.GetddlHolisticThirdClass();
            ViewBag.ddlActInOrOut = dbAccess.GetddlActInOrOut();
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify("1");

            HolisticPassportMangViewModel vm = new HolisticPassportMangViewModel();
            vm.CreateModel = new HolisticPassportMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, HolisticPassportMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlAllClub = dbAccess.GetAllClubID();
            ViewBag.ddlAllActName = dbAccess.GetAllActName();
            ViewBag.ddlHolisticMainClass = dbAccess.GetddlHolisticMainClass();
            ViewBag.ddlHolisticSecondClass = dbAccess.GetddlHolisticSecondClass();
            ViewBag.ddlHolisticThridClass = dbAccess.GetddlHolisticThirdClass();
            ViewBag.ddlActInOrOut = dbAccess.GetddlActInOrOut();
            ViewBag.ddlAllActVerify = dbAccess.GetAllActVerify("1");
            
            vm.EditModel = dbAccess.GetEditData(submitBtn);
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(HolisticPassportMangViewModel vm)
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
        public IActionResult SaveNewData(HolisticPassportMangViewModel vm)
        {
            try
            {
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
        public IActionResult EditOldData(HolisticPassportMangViewModel vm)
        {
            try
            {
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
                    vmRtn.ErrorCode =  (int)DBActionChineseName.失敗;
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
        public IActionResult ExportSearchResult(HolisticPassportMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}", LogActionChineseName.全人學習護照管理, DateTime.Now.ToString("yyyyMMdd"));
            vm.ResultModel = dbAccess.GetExportResult(vm.ConditionModel);

            if (vm.ResultModel != null && vm.ResultModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 20, 20, 20, 80, 20, 20 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(HolisticPassportMangExcelHeaderModel).GetProperties();

                //設定欄位
                IRow headerRow = sheet.CreateRow(0);

                XSSFCellStyle headStyle = ExcelUtil.GetDefaultHeaderStyle(workbook);

                for (int i = 0; i <= properties.Length - 1; i++)
                {
                    var displayAttribute = (DisplayNameAttribute)properties[i].GetCustomAttribute(typeof(DisplayNameAttribute));
                    var displayName = displayAttribute?.DisplayName ?? properties[i].Name;

                    headerRow.CreateCell(i).SetCellValue(displayName);

                    foreach (ICell cell in headerRow.Cells)
                        cell.CellStyle = headStyle;

                }

                XSSFCellStyle contentStyle = ExcelUtil.GetDefaultContentStyle(workbook);

                //設定資料
                for (int i = 0; i <= vm.ResultModel.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    dataRow.CreateCell(0).SetCellValue(vm.ResultModel[i].ClubID);
                    dataRow.CreateCell(1).SetCellValue(vm.ResultModel[i].SchoolYear);
                    dataRow.CreateCell(2).SetCellValue(vm.ResultModel[i].ClubCName);
                    dataRow.CreateCell(3).SetCellValue(vm.ResultModel[i].ActID);
                    dataRow.CreateCell(4).SetCellValue(vm.ResultModel[i].ActName);
                    dataRow.CreateCell(5).SetCellValue(vm.ResultModel[i].ActVerifyText);
                    dataRow.CreateCell(6).SetCellValue(vm.ResultModel[i].Created != null ? vm.ResultModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm") : "");

                    foreach (ICell cell in dataRow.Cells)
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



        [Log(LogActionChineseName.取得樓館選單)]
        [ValidateInput(false)]
        public IActionResult InitBuildSelect(string PlaceSource)
        {
            if (PlaceSource == "01")
            {
                ViewBag.ddlBuild = dbAccess.GetBuild();
            }

            HolisticPassportMangViewModel vm = new HolisticPassportMangViewModel();
            vm.CreateModel = new HolisticPassportMangCreateModel();
            vm.CreateModel.PlaceSource = PlaceSource;

            return PartialView("_PlaceDataPartial", vm);
        }

        [Log(LogActionChineseName.取得場地選單)]
        [ValidateInput(false)]
        public IActionResult InitPlaceSelect(string PlaceSource, string Buildid)
        {
            ViewBag.ddlBuild = dbAccess.GetBuild();
            ViewBag.ddlPlace = dbAccess.GetPlace(PlaceSource, Buildid);


            HolisticPassportMangViewModel vm = new HolisticPassportMangViewModel();
            vm.CreateModel = new HolisticPassportMangCreateModel();
            vm.CreateModel.PlaceSource = PlaceSource;
            vm.CreateModel.BuildID = Buildid;


            return PartialView("_HolisticPlaceDataPartial", vm);
        }


    }
}
