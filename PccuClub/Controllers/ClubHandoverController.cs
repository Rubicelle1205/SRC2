using DataAccess;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming.Values;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using NuGet.DependencyResolver;
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
    [LogAttribute(LogActionChineseName.交接資料管理)]
    public class ClubHandoverController : BaseController
    {
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubHandoverDataAccess dbAccess = new ClubHandoverDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

		public ClubHandoverController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        #region 申請

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.CheckModel = dbAccess.GetCheckData(LoginUser.LoginId);

            return View(vm);
        }


		[Log(LogActionChineseName.新增儲存)]
		[ValidateInput(false)]
		public IActionResult SendNewHandOver(ClubHandoverViewModel vm)
		{
			try
			{
				vm.CheckModel = dbAccess.GetCheckData(LoginUser.LoginId, true);

				if (vm.CheckModel != null && vm.CheckModel.Count > 0)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "已申請過交接作業，請耐心等待";
					return Json(vmRtn);
				}

				dbAccess.DbaInitialTransaction();

				DataTable dt = new DataTable();

				var dbResult = dbAccess.NewHandOver(LoginUser.LoginId);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "申請失敗";
					return Json(vmRtn);
				}

				dbAccess.DbaCommit();
			}
			catch (Exception ex)
			{
				dbAccess.DbaRollBack();
				vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
				vmRtn.ErrorMsg = "申請失敗" + ex.Message;
				return Json(vmRtn);
			}

			return Json(vmRtn);
		}

        #endregion

        #region 已填寫表單

        [Log(LogActionChineseName.已填寫表單)]
		public IActionResult HandOverHistory()
		{
			ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.HistoryConditionModel = new ClubHandoverHistroyConditionModel();
			vm.HistoryConditionModel.SchoolYear = PublicFun.GetNowSchoolYear();

			return View(vm);
		}

		[LogAttribute(LogActionChineseName.查詢)]
		public IActionResult GetHistorySearchResult(ClubHandoverViewModel vm)
		{
			vm.HistoryResultModel = dbAccess.GetHistorySearchResult(vm.HistoryConditionModel, LoginUser).ToList();

			#region 分頁
			vm.HistoryConditionModel.TotalCount = vm.HistoryResultModel.Count();
			int StartRow = vm.HistoryConditionModel.Page * vm.HistoryConditionModel.PageSize;
			vm.HistoryResultModel = vm.HistoryResultModel.Skip(StartRow).Take(vm.HistoryConditionModel.PageSize).ToList();
			#endregion

			return PartialView("_SearchHistoryResultPartial", vm);
		}

        #endregion

        #region 已上傳檔案

        [Log(LogActionChineseName.已上傳檔案)]
        public IActionResult HandOverFile()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.FileConditionModel = new ClubHandoverFileConditionModel();
            vm.FileConditionModel.SchoolYear = PublicFun.GetNowSchoolYear();

            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetFileSearchResult(ClubHandoverViewModel vm)
        {
            vm.FileResultModel = dbAccess.GetFileSearchResult(vm.FileConditionModel, LoginUser).ToList();

            if (vm.FileResultModel.Count > 0)
            {
                for (int i = 0; i <= vm.FileResultModel.Count - 1; i++)
                {
                    string DetailID = vm.FileResultModel[i].HoDetailID;

                    List<ClubHandoverFileDataModel> LstFileData = dbAccess.GetAllFileData(DetailID);

                    vm.FileResultModel[i].FileData = LstFileData;
                }
            }

            #region 分頁
            vm.FileConditionModel.TotalCount = vm.FileResultModel.Count();
            int StartRow = vm.FileConditionModel.Page * vm.FileConditionModel.PageSize;
            vm.FileResultModel = vm.FileResultModel.Skip(StartRow).Take(vm.FileConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchFileResultPartial", vm);
        }

        #endregion

        #region 表單撰寫

		public IActionResult HistorySwitch(string id, string docType)
		{
            switch (docType)
            {
                case "01":
                    return Redirect($"/ClubHandover/HandOver0101?id={id}");
                case "02":
                    return Redirect($"/ClubHandover/HandOver0102?id={id}");
                case "03":
                    return Redirect($"/ClubHandover/HandOver0103?id={id}");
                case "04":
                    return Redirect($"/ClubHandover/HandOver0204?id={id}");
                case "05":
                    return Redirect($"/ClubHandover/HandOver0205?id={id}");
                case "06":
                    return Redirect($"/ClubHandover/HandOver006?id={id}");
                case "07":
                    return Redirect($"/ClubHandover/HandOver0307?id={id}");
                case "08":
                    return Redirect($"/ClubHandover/HandOver0308?id={id}");
                case "09":
                    return Redirect($"/ClubHandover/HandOver0309?id={id}");
                default:
                    Redirect("Index");
                    break;
            }

            return View();
		}

        public IActionResult PrintSwitch(string id, string docType)
        {
            switch (docType)
            {
                case "01":
                    return Redirect($"/ClubHandover/Print0101?id={id}");
                case "02":
                    return Redirect($"/ClubHandover/Print0102?id={id}");
                case "03":
                    return Redirect($"/ClubHandover/Print0103?id={id}");
                case "04":
                    return Redirect($"/ClubHandover/Print0204?id={id}");
                case "05":
                    return Redirect($"/ClubHandover/Print0205?id={id}");
                case "06":
                    return Redirect($"/ClubHandover/Print0206?id={id}");
                case "07":
                    return Redirect($"/ClubHandover/Print0307?id={id}");
                case "08":
                    return Redirect($"/ClubHandover/Print0308?id={id}");
                case "09":
                    return Redirect($"/ClubHandover/Print0309?id={id}");
                default:
                    Redirect("Index");
                    break;
            }

            return View();
        }

		#region 01

		[Log(LogActionChineseName.社團負責人改選管理)]
		public IActionResult HandOver01()
		{
			return View();
		}

		#region 0101

		[Log(LogActionChineseName.社團負責人改選管理)]
		public IActionResult HandOver0101(string id)
		{
			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0101Model = new ();

			if (!string.IsNullOrEmpty(id))
			{
				vm.Handover0101Model = dbAccess.GetHandover0101Data(id, LoginUser);
            }

			return View(vm);
		}

		[Log(LogActionChineseName.編輯儲存)]
		[ValidateInput(false)]
		public async Task<IActionResult> Save0101(ClubHandoverViewModel vm)
		{
			try
			{
				DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
				string HoID = dt.QueryFieldByDT("HoID");

				ClubHandoverViewModel vm2 = new ClubHandoverViewModel();
				vm2.HandoverDocCheckModel = dbAccess.GetHandoverDocData(HoID, "01");

                if (vm2.HandoverDocCheckModel != null)
				{
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "此表單已存在";
					return Json(vmRtn);
				}

				dbAccess.DbaInitialTransaction();

				DataTable dtt = new DataTable();

				var dbResult = dbAccess.InsertDetail(HoID, "01", "01", LoginUser, out dtt);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
					return Json(vmRtn);
				}

				string HoDetailID = dtt.QueryFieldByDT("HoDetailID");

				dbResult = dbAccess.Insert0101(vm, LoginUser, HoID, HoDetailID);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
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

		[Log(LogActionChineseName.列印)]
		public IActionResult Print0101(string id)
        {
			DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
			string HoID = dt.QueryFieldByDT("HoID");

			if (!string.IsNullOrEmpty(id))
				HoID = id;

			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0101Model = dbAccess.GetHandover0101Data(HoID, LoginUser);

			return View(vm);
		}

		#endregion

        #region 0102

		[Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0102(string id)
        {
            ViewBag.ddlElectionType = dbAccess.getAllElectionType();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0102Model = new ClubHandover0102ViewModel();
            vm.Handover0102Model.SchoolYear = PublicFun.GetNowSchoolYear();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0102Model = dbAccess.GetHandover0102Data(id, LoginUser);
            }

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> Save0102(ClubHandoverViewModel vm)
        {
            try
            {
                DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
                string HoID = dt.QueryFieldByDT("HoID");

                ClubHandoverViewModel vm2 = new ClubHandoverViewModel();
                vm2.HandoverDocCheckModel = dbAccess.GetHandoverDocData(HoID, "02");

                if (vm2.HandoverDocCheckModel != null)
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "此表單已存在";
                    return Json(vmRtn);
                }

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("Handover0102Model.MeetingRecord"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass02", file);
                            vm.Handover0102Model.MeetingRecordName = file.FileName;
                            vm.Handover0102Model.MeetingRecord = strFilePath;
                        }
                        else if (Request.Form.Files[i].Name.Contains("Handover0102Model.MeetingSign"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass02", file);
							vm.Handover0102Model.MeetingSignName = file.FileName;
							vm.Handover0102Model.MeetingSign = strFilePath;
                        }
                    }
                }


                dbAccess.DbaInitialTransaction();

                DataTable dtt = new DataTable();

                var dbResult = dbAccess.InsertDetail(HoID, "01", "02", LoginUser, out dtt);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }

                string HoDetailID = dtt.QueryFieldByDT("HoDetailID");

                dbResult = dbAccess.Insert0102(vm, LoginUser, HoID, HoDetailID);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
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

        [Log(LogActionChineseName.列印)]
        public IActionResult Print0102(string id)
        {
            DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
            string HoID = dt.QueryFieldByDT("HoID");

            if (!string.IsNullOrEmpty(id))
                HoID = id;

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0102Model = dbAccess.GetHandover0102Data(HoID, LoginUser);

            return View(vm);
        }
        #endregion

        #region 0103

        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0103(string id)
        {
            ViewBag.ddlClubBuild = dbAccess.GetClubBuild();
			ViewBag.ddlSex = dbAccess.GetAllSex();
			ViewBag.ddldentityType = dbAccess.GetAllIdentityType();
			ViewBag.ddlConform = dbAccess.GetAllConform();

			ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0103Model = new ClubHandover0103ViewModel();
            vm.Handover0103Model.SchoolYear = PublicFun.GetNowSchoolYear();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0102Model = dbAccess.GetHandover0102Data(id, LoginUser);
            }

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> Save0103(ClubHandoverViewModel vm)
        {
            try
            {
                DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
                string HoID = dt.QueryFieldByDT("HoID");

                ClubHandoverViewModel vm2 = new ClubHandoverViewModel();
                vm2.HandoverDocCheckModel = dbAccess.GetHandoverDocData(HoID, "03");

                if (vm2.HandoverDocCheckModel != null)
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "此表單已存在";
                    return Json(vmRtn);
                }

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("Handover0103Model.Transcript"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass03", file);
                            vm.Handover0103Model.TranscriptName = file.FileName;
                            vm.Handover0103Model.Transcript = strFilePath;
                        }
                    }
                }


                dbAccess.DbaInitialTransaction();

                DataTable dtt = new DataTable();

                var dbResult = dbAccess.InsertDetail(HoID, "01", "03", LoginUser, out dtt);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }

                string HoDetailID = dtt.QueryFieldByDT("HoDetailID");

                dbResult = dbAccess.Insert0103(vm, LoginUser, HoID, HoDetailID);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
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

        [Log(LogActionChineseName.列印)]
        public IActionResult Print0103(string id)
        {
            DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
            string HoID = dt.QueryFieldByDT("HoID");

            if (!string.IsNullOrEmpty(id))
                HoID = id;

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0103Model = dbAccess.GetHandover0103Data(HoID, LoginUser);

            return View(vm);
        }

		#endregion

		#endregion

		#region 02

		[Log(LogActionChineseName.交接準備)]
		public IActionResult HandOver02()
		{
			return View();
		}

		#region 0204

		[Log(LogActionChineseName.交接準備)]
		public IActionResult HandOver0204(string id)
		{
			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0204Model = new ClubHandover0204ViewModel();
			vm.Handover0204Model.SchoolYear = PublicFun.GetNowSchoolYear();

			if (!string.IsNullOrEmpty(id))
			{
				vm.Handover0204Model = dbAccess.GetHandover0204Data(id, LoginUser);
			}

			return View(vm);
		}

		[Log(LogActionChineseName.編輯儲存)]
		[ValidateInput(false)]
		public IActionResult Save0204(ClubHandoverViewModel vm)
		{
			try
			{
				DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
				string HoID = dt.QueryFieldByDT("HoID");

				ClubHandoverViewModel vm2 = new ClubHandoverViewModel();
				vm2.HandoverDocCheckModel = dbAccess.GetHandoverDocData(HoID, "04");

				if (vm2.HandoverDocCheckModel != null)
				{
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "此表單已存在";
					return Json(vmRtn);
				}

				dbAccess.DbaInitialTransaction();

				DataTable dtt = new DataTable();

				var dbResult = dbAccess.InsertDetail(HoID, "02", "04", LoginUser, out dtt);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
					return Json(vmRtn);
				}

				string HoDetailID = dtt.QueryFieldByDT("HoDetailID");

				dbResult = dbAccess.Insert0204(vm, LoginUser, HoID, HoDetailID);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
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

		[Log(LogActionChineseName.列印)]
		public IActionResult Print0204(string id)
		{
			DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
			string HoID = dt.QueryFieldByDT("HoID");

			if (!string.IsNullOrEmpty(id))
				HoID = id;

			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0204Model = dbAccess.GetHandover0204Data(HoID, LoginUser);

			return View(vm);
		}


		#endregion

















		[Log(LogActionChineseName.交接準備)]
		public IActionResult HandOver0205()
		{
			return View();
		}

		[Log(LogActionChineseName.交接準備)]
		public IActionResult HandOver0206()
		{
			return View();
		}


		#endregion




		[Log(LogActionChineseName.新任營運資訊)]
		public IActionResult HandOver03()
		{
			return View();
		}

        [Log(LogActionChineseName.新任營運資訊)]
        public IActionResult HandOver0307()
        {
            return View();
        }

        [Log(LogActionChineseName.新任營運資訊)]
        public IActionResult HandOver0308()
        {
            return View();
        }

        [Log(LogActionChineseName.新任營運資訊)]
        public IActionResult HandOver0309()
        {
            return View();
        }



        #endregion

        #region 檔案上傳

        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOverFile01()
        {
            DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
            string HoID = dt.QueryFieldByDT("HoID");

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.FileDetailModel = dbAccess.GetFileDetail(HoID, LoginUser, "01");

            return View(vm);
        }

        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public async Task<IActionResult> SaveFile01(ClubHandoverViewModel vm)
        {
            try
            {
                DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
                string HoID = dt.QueryFieldByDT("HoID");

                vm.LstFileEditModel = new List<ClubHandoverFileEditModel>();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("File"))
                        {
                            var file = Request.Form.Files[i];

                            string strFilePath = await upload.UploadFileAsync("HandOverClass01", file);

                            ClubHandoverFileEditModel model = new ClubHandoverFileEditModel();
                            model.FilePath = strFilePath;

                            vm.LstFileEditModel.Add(model);
                        }
                    }
                }


                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.UpdateFileDetailToNoUse(HoID, "01", LoginUser);

				if (!dbResult.isSuccess && dbResult.ErrorCode != dbErrorCode._EC_NotAffect)
				{
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }

                DataTable dtt = new DataTable();

                dbResult = dbAccess.InsertFileDetail(HoID, "01", LoginUser, out dtt);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }

                string HoDetailID = dtt.QueryFieldByDT("HoDetailID");

                dbResult = dbAccess.InsertFile(vm, LoginUser, HoID, HoDetailID);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
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

        [Log(LogActionChineseName.交接準備)]
        public IActionResult HandOverFile02()
        {
			DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
			string HoID = dt.QueryFieldByDT("HoID");

			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.FileDetailModel = dbAccess.GetFileDetail(HoID, LoginUser, "02");

			return View(vm);
		}

		[Log(LogActionChineseName.新增儲存)]
		[ValidateInput(false)]
		public async Task<IActionResult> SaveFile02(ClubHandoverViewModel vm)
		{
			try
			{
				DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
				string HoID = dt.QueryFieldByDT("HoID");

				vm.LstFileEditModel = new List<ClubHandoverFileEditModel>();

				if (Request.Form.Files.Count > 0)
				{
					for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
					{
						if (Request.Form.Files[i].Name.Contains("File"))
						{
							var file = Request.Form.Files[i];

							string strFilePath = await upload.UploadFileAsync("HandOverClass02", file);

							ClubHandoverFileEditModel model = new ClubHandoverFileEditModel();
							model.FilePath = strFilePath;

							vm.LstFileEditModel.Add(model);
						}
					}
				}


				dbAccess.DbaInitialTransaction();

				var dbResult = dbAccess.UpdateFileDetailToNoUse(HoID, "02", LoginUser);

				if (!dbResult.isSuccess && dbResult.ErrorCode != dbErrorCode._EC_NotAffect)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
					return Json(vmRtn);
				}

				DataTable dtt = new DataTable();

				dbResult = dbAccess.InsertFileDetail(HoID, "02", LoginUser, out dtt);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
					return Json(vmRtn);
				}

				string HoDetailID = dtt.QueryFieldByDT("HoDetailID");

				dbResult = dbAccess.InsertFile(vm, LoginUser, HoID, HoDetailID);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
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

		[Log(LogActionChineseName.新任營運資訊)]
        public IActionResult HandOverFile03()
        {
			DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
			string HoID = dt.QueryFieldByDT("HoID");

			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.FileDetailModel = dbAccess.GetFileDetail(HoID, LoginUser, "03");

			return View(vm);
		}

		[Log(LogActionChineseName.新增儲存)]
		[ValidateInput(false)]
		public async Task<IActionResult> SaveFile03(ClubHandoverViewModel vm)
		{
			try
			{
				DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
				string HoID = dt.QueryFieldByDT("HoID");

				vm.LstFileEditModel = new List<ClubHandoverFileEditModel>();

				if (Request.Form.Files.Count > 0)
				{
					for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
					{
						if (Request.Form.Files[i].Name.Contains("File"))
						{
							var file = Request.Form.Files[i];

							string strFilePath = await upload.UploadFileAsync("HandOverClass03", file);

							ClubHandoverFileEditModel model = new ClubHandoverFileEditModel();
							model.FilePath = strFilePath;

							vm.LstFileEditModel.Add(model);
						}
					}
				}


				dbAccess.DbaInitialTransaction();

				var dbResult = dbAccess.UpdateFileDetailToNoUse(HoID, "03", LoginUser);

				if (!dbResult.isSuccess && dbResult.ErrorCode != dbErrorCode._EC_NotAffect)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
					return Json(vmRtn);
				}

				DataTable dtt = new DataTable();

				dbResult = dbAccess.InsertFileDetail(HoID, "03", LoginUser, out dtt);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
					return Json(vmRtn);
				}

				string HoDetailID = dtt.QueryFieldByDT("HoDetailID");

				dbResult = dbAccess.InsertFile(vm, LoginUser, HoID, HoDetailID);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
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
		#endregion

	}
}
