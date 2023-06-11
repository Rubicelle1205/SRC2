using DataAccess;
using Microsoft.AspNetCore.Http;
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
using WebPccuClub.Global.Extension;
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

        [Log(LogActionChineseName.新增)]
        public IActionResult Create()
        {
            ViewBag.ddlStaticOrDynamic = dbAccess2.GetStaticOrDynamic();
            ViewBag.ddlActInOrOut = dbAccess2.GetActInOrOut();
            ViewBag.ddlActType = dbAccess2.GetActType();
            ViewBag.ddlUseITEquip = dbAccess2.GetUseITEquip();
            ViewBag.ddlPassport = dbAccess2.GetPassport();
            ViewBag.ddlAllSDGs = dbAccess.GetAllSDGs();

            ClubActReportViewModel vm = new ClubActReportViewModel();
            vm.CreateModel = new ClubActReportCreateModel();
            vm.CreateModel.SchoolYear = PublicFun.GetNowSchoolYear();


			return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create2(ClubActReportViewModel vm)
        {
			
			HttpContext.Session.SetObject("MyModel", vm);

			ViewBag.ddlPlaceSource = dbAccess2.GetPlaceSource();
			ViewBag.ddlHour = dbAccess2.GetAllHour();

			return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult Create3(ClubActReportViewModel vm)
        {
			ClubActReportViewModel vm2 = HttpContext.Session.GetObject<ClubActReportViewModel>("MyModel");
            vm2.CreateModel.strRundown = vm.CreateModel.strRundown;
			HttpContext.Session.SetObject("MyModel", vm2);

			return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public async Task<IActionResult> Create4(ClubActReportViewModel vm)
        {
            ClubActReportViewModel vm3 = HttpContext.Session.GetObject<ClubActReportViewModel>("MyModel");

            if (Request.Form.Files.Count > 0)
            {
                for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                {
                  if (Request.Form.Files[i].Name.Contains("Proposal"))
                    {
                        var file = Request.Form.Files[i];

                        string strFilePath = await upload.UploadFileAsync("ActProposal", file);

                        ActListFilesModel model = new ActListFilesModel();
                        model.FileName = file.FileName;
                        model.FilePath = strFilePath;

                        vm3.CreateModel.LstProposal.Add(model);
                    }
                }
            }

            HttpContext.Session.SetObject("MyModel", vm3);

            return View(vm);
        }

        [Log(LogActionChineseName.新增)]
        public async Task<IActionResult> Create(ClubActReportViewModel vm)
        {
            ClubActReportViewModel vm4 = HttpContext.Session.GetObject<ClubActReportViewModel>("MyModel");

            if (Request.Form.Files.Count > 0)
            {
                for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                {
                    if (Request.Form.Files[i].Name.Contains("Proposal"))
                    {
                        var file = Request.Form.Files[i];

                        string strFilePath = await upload.UploadFileAsync("ActProposal", file);

                        ActListFilesModel model = new ActListFilesModel();
                        model.FileName = file.FileName;
                        model.FilePath = strFilePath;

                        vm4.CreateModel.LstProposal.Add(model);
                    }
                }
            }

            HttpContext.Session.SetObject("MyModel", vm4);

            return View(vm);
        }



        [ValidateInput(false)]
		public IActionResult GetUsedByDate(string SelectedDate)
		{
			ActListMangViewModel vm = new ActListMangViewModel();
			//vm.LstPlaceUsedModel = dbAccess.GetPlaceUsedData(SelectedDate);

			//先抓取DB資料
			List<ActListMangPlaceUsedModel> LstNewPlaceUsed = dbAccess2.GetPlaceUsedData(SelectedDate);
			List<ActListMangPlaceUsedModel> LstNewPlaceUsed2 = new List<ActListMangPlaceUsedModel>();

			foreach (ActListMangPlaceUsedModel item in LstNewPlaceUsed)
			{
				ActListMangPlaceUsedModel model = new ActListMangPlaceUsedModel();
				model.PlaceName = item.PlaceName;
				model.STime = item.STime;
				model.ETime = item.ETime;

				var SelectedItem = LstNewPlaceUsed2.Where(m => m.PlaceName == item.PlaceName).FirstOrDefault();

				if (SelectedItem != null)
				{
					if (int.Parse(model.STime) < int.Parse(SelectedItem.STime))
					{
						SelectedItem.STime = model.STime;
					}

					if (int.Parse(model.ETime) > int.Parse(SelectedItem.ETime))
					{
						SelectedItem.ETime = model.ETime;
					}
				}
				else
				{
					LstNewPlaceUsed2.Add(model);
				}
			}


			vm.LstPlaceUsedModel = LstNewPlaceUsed2;


			return PartialView("_PlaceUsedPartial", vm);
		}


        public IActionResult GetTodayAct(string PlaceId, string SelectedDate)
        {
            ActListMangViewModel vm = new ActListMangViewModel();


            vm.LstTodayActModel = dbAccess2.GetTodayAct(PlaceId, SelectedDate);


            return PartialView("_PlaceTodayActPartial", vm);
        }

        [Log(LogActionChineseName.取得樓館選單)]
        [ValidateInput(false)]
        public IActionResult InitBuildSelect(string PlaceSource)
        {
            if (PlaceSource == "01")
            {
                ViewBag.ddlBuild = dbAccess2.GetBuild();
            }

            ActListMangViewModel vm = new ActListMangViewModel();
            vm.CreateModel = new ActListMangCreateModel();
            vm.CreateModel.PlaceSource = PlaceSource;

            return PartialView("_PlaceDataPartial", vm);
        }

        [Log(LogActionChineseName.取得場地選單)]
        [ValidateInput(false)]
        public IActionResult InitPlaceSelect(string PlaceSource, string Buildid)
        {
            ViewBag.ddlBuild = dbAccess2.GetBuild();
            ViewBag.ddlPlace = dbAccess2.GetPlace(PlaceSource, Buildid);


            ActListMangViewModel vm = new ActListMangViewModel();
            vm.CreateModel = new ActListMangCreateModel();
            vm.CreateModel.PlaceSource = PlaceSource;
            vm.CreateModel.Buildid = Buildid;


            return PartialView("_PlaceDataPartial", vm);
        }

        [Log(LogActionChineseName.取得場地資料)]
        [ValidateInput(false)]
        public IActionResult InitPlaceData(string PlaceSource, string Buildid, string PlaceId)
        {
            ViewBag.ddlBuild = dbAccess2.GetBuild();
            ViewBag.ddlPlace = dbAccess2.GetPlace(PlaceSource, Buildid);


            ActListMangViewModel vm = new ActListMangViewModel();
            vm.CreateModel = new ActListMangCreateModel();
            vm.CreateModel.PlaceSource = PlaceSource;
            vm.CreateModel.Buildid = Buildid;
            vm.CreateModel.PlaceId = PlaceId;

            vm.PlaceDataModel = dbAccess2.GetPlaceData(PlaceSource, PlaceId);



            return PartialView("_PlaceDataPartial", vm);
        }


		[ValidateInput(false)]
		public IActionResult ChkRundown(ActListMangViewModel vm)
		{
			bool CanUse = true;
			List<int> hours = new List<int>();

			if (vm.RundownModel.PlaceSource == "01")
			{
				//確認場地開放狀態
				CanUse = dbAccess2.ChkPlaceSchoolCanUse(vm);

				if (!CanUse)
				{
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "該場地目前不可使用";
					return Json(vmRtn);
				}

				for (int j = int.Parse(vm.RundownModel.STime); j <= int.Parse(vm.RundownModel.ETime) - 1; j++)
				{
					hours.Add(j);
				}

				//確認場地使用狀態
				string msg = string.Empty;
				CanUse = ChkCanBatchData(vm, DateTime.Parse(vm.RundownModel.Date), hours, out msg);

				if (!CanUse)
				{
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = msg;
					return Json(vmRtn);
				}
			}

			//判斷是否重複增加，先拿出舊的資料
			if (!string.IsNullOrEmpty(vm.RundownModel.strRundown))
			{
				List<ActListMangRundownModel> LstRundown = new List<ActListMangRundownModel>();

				string[] arr = vm.RundownModel.strRundown.Split("|");

				for (int i = 0; i <= arr.Length - 1; i++)
				{
					string[] arr2 = vm.RundownModel.strRundown.Split(",");

					ActListMangRundownModel model = new ActListMangRundownModel();
					model.PlaceSource = arr2[0];
					model.Date = arr2[1];
					model.STime = arr2[2];
					model.ETime = arr2[3];
					model.PlaceID = arr2[4];
					model.PlaceText = arr2[5];

					LstRundown.Add(model);
				}


				CanUse = LstRundown.Where(m => m.PlaceSource == vm.RundownModel.PlaceSource && m.Date == vm.RundownModel.Date && m.STime == vm.RundownModel.STime && m.ETime == vm.RundownModel.ETime &&
											   m.PlaceID == vm.RundownModel.PlaceID && m.PlaceText == vm.RundownModel.PlaceText).ToList().Count > 0;

				if (CanUse)
				{
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "場地重複增加";
					return Json(vmRtn);
				}
			}

			return Json(vmRtn);
		}

		private bool ChkCanBatchData(ActListMangViewModel vm, DateTime date, List<int> hours, out string msg)
		{
			bool ok = true;
			msg = string.Empty;

			string PlaceID = vm.RundownModel.PlaceID;

			if (string.IsNullOrEmpty(PlaceID)) { msg = "查找場地失敗"; return false; }


			DataTable dt = dbAccess2.GetRundown(PlaceID, date);

			foreach (DataRow dr in dt.Rows)
			{
				int STime = int.Parse(dr["STime"].ToString());

				for (int i = 0; i <= hours.Count - 1; i++)
				{
					if (STime == hours[i])
					{
						ok = false;
						msg = string.Format("以下時段已有其他活動。<br/> 日期:{0} 時間:{1}:00 ", date.ToString("yyyy/MM/dd"), hours[i]);
					}
				}
			}


			return ok;
		}







































		[Log(LogActionChineseName.編輯)]
        public IActionResult Edit(string id, ClubActReportViewModel vm)
        {
            

            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");
            
            ViewBag.ddlAllSDGs = dbAccess.GetAllSDGs();
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            //ClubActReportViewModel vm = new ClubActReportViewModel();
            vm.EditModel = dbAccess.GetEditData(id);
			vm.EditModel.LstActRundown = dbAccess2.GetEditRundownData(id);
			vm.EditModel.LstProposal = dbAccess2.GetEditProposalData(id);
			vm.EditModel.LstOutSideFile = dbAccess2.GetEditOutSideFileData(id);

			return View(vm);
        }




        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> SaveCreate1(ClubActReportViewModel vm)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                dbAccess.DbaRollBack();
                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                vmRtn.ErrorMsg = "新增失敗" + ex.Message;
                return Json(vmRtn);
            }

            return RedirectToAction("Create2", vm);

        }





























        [ValidateInput(false)]
        public IActionResult GetConsentMang(string Selected)
        {
            ClubActReportViewModel vm = new ClubActReportViewModel();

            vm.ConsentModel = dbAccess.GetConsentData();
            vm.ConsentModel.Selected = Selected;

            return PartialView("_AgreeBoxPartial", vm);
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

                            //vm.CreateModel.Attachment = strFilePath;
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
