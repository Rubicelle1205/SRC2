using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.WebPages;
using Utility;
using WebPccuClub.DataAccess;
using WebPccuClub.Global;
using WebPccuClub.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.校內場地)]
    public class PlaceSchoolMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        PlaceSchoolMangDataAccess dbAccess = new PlaceSchoolMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public PlaceSchoolMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }


        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ViewBag.ddlAllBuild = dbAccess.GetAllBuild();
            ViewBag.ddlAllPlaceStatus = dbAccess.GetAllPlaceStatus();

            PlaceSchoolMangViewModel vm = new PlaceSchoolMangViewModel();
            vm.ConditionModel = new PlaceSchoolMangConditionModel();
            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlHour = dbAccess.GetAllHour();
            ViewBag.ddlFloor = dbAccess.GetAllFloor();
            ViewBag.ddlAllBuild = dbAccess.GetAllBuild();
            ViewBag.ddlAllPlaceStatus = dbAccess.GetAllPlaceStatus();

            PlaceSchoolMangViewModel vm = new PlaceSchoolMangViewModel();
            vm.CreateModel = new PlaceSchoolMangCreateModel();
            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string submitBtn, PlaceSchoolMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlHour = dbAccess.GetAllHour();
            ViewBag.ddlFloor = dbAccess.GetAllFloor();
            ViewBag.ddlAllBuild = dbAccess.GetAllBuild();
            ViewBag.ddlAllPlaceStatus = dbAccess.GetAllPlaceStatus();

            //PlaceSchoolMangViewModel vm = new PlaceSchoolMangViewModel();
            vm.EditModel = dbAccess.GetEditData(submitBtn);
            return View(vm);
        }

        [Log(LogActionChineseName.批次借用或關閉場地)]
        public IActionResult BatchAddAct(string submitBtn, PlaceSchoolMangViewModel vm)
        {
            ViewBag.ddlHour = dbAccess.GetAllHour();
            ViewBag.ddlAllBuild = dbAccess.GetAllBuild();
            ViewBag.dllAllWeek = dbAccess.GetAllWeek();
            ViewBag.ddlAllBorrowType = dbAccess.GetAllBorrowType();
            ViewBag.ddlAllPlaceSchool = dbAccess.GetAllPlaceSchool();

            //PlaceSchoolMangViewModel vm = new PlaceSchoolMangViewModel();
            vm.BatchAddActModel = dbAccess.GetBatchAddActData(submitBtn);
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(PlaceSchoolMangViewModel vm)
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
        public IActionResult SaveNewData(PlaceSchoolMangViewModel vm)
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
        public IActionResult EditOldData(PlaceSchoolMangViewModel vm)
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

        [Log(LogActionChineseName.執行批次借用或關閉場地)]
        [ValidateInput(false)]
        public IActionResult BatchAddActInsert(PlaceSchoolMangViewModel vm)
        {
            try
            {
                string[] arr = vm.BatchAddActModel.Week.Split(",");

                DateTime SDate = DateTime.Parse(vm.BatchAddActModel.SDate);
                DateTime EDate = DateTime.Parse(vm.BatchAddActModel.EDate);
                List<DateTime> dates = new List<DateTime>();
                string[] daysOfWeek = GetSelectedWeek(arr);

                for (DateTime currentDate = SDate; currentDate <= EDate; currentDate = currentDate.AddDays(1))
                {
                    string dayOfWeek = currentDate.ToString("dddd");

                    if (Array.Exists(daysOfWeek, element => element.Equals(dayOfWeek, StringComparison.OrdinalIgnoreCase)))
                    {
                        dates.Add(currentDate);
                    }
                }

                dbAccess.DbaInitialTransaction();

                DataTable dt = new DataTable();

                var dbResult = dbAccess.InsertActMainData(vm, LoginUser, out dt);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                string ActId = dt.QueryFieldByDT("ActID");

                dbResult = dbAccess.InsertActDetailData(vm, ActId, LoginUser);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "新增失敗";
                    return Json(vmRtn);
                }

                for (int i = 0; i <= dates.Count - 1; i++)
                {
                    dbResult = dbAccess.InsertActRundownData(vm, ActId, dates[i], LoginUser);

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

        private string[] GetSelectedWeek(string[] arr)
        {
            List<string> LstSelectedWeek = new List<string>();
            foreach (string str in arr)
            {
                string strWeek = string.Empty;
                switch (str)
                {
                    case "01":
                        strWeek = "星期一";
                        break;
                    case "02":
                        strWeek = "星期二";
                        break;
                    case "03":
                        strWeek = "星期三";
                        break;
                    case "04":
                        strWeek = "星期四";
                        break;
                    case "05":
                        strWeek = "星期五";
                        break;
                    case "06":
                        strWeek = "星期六";
                        break;
                    case "07":
                        strWeek = "星期日";
                        break;
                }

                LstSelectedWeek.Add(strWeek);
            }

            return LstSelectedWeek.ToArray();
        }



    }
}
