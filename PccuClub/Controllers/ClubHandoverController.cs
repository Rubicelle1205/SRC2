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
    public class ClubHandoverController : FBaseController
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

				if (vm.CheckModel != null)
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

		[ValidateInput(false)]
		public IActionResult PassToNewLeader(ClubHandoverViewModel vm)
		{
			try
			{
				dbAccess.DbaInitialTransaction();
				string NewLeaderSNO = string.Empty;

				vm.CheckModel = dbAccess.GetCheckData(LoginUser.LoginId, false);

				if (vm.CheckModel != null)
				{
					string SchoolYear = vm.CheckModel.SchoolYear;

					//先取得交接的學生學號
					NewLeaderSNO = dbAccess.GetNewLeader(LoginUser.LoginId, SchoolYear);

					if (!string.IsNullOrEmpty(NewLeaderSNO))
					{
						//更新ClubUser
						var dbResult = dbAccess.UpdateUserClub(LoginUser.LoginId, NewLeaderSNO);

						if (!dbResult.isSuccess)
						{
							dbAccess.DbaRollBack();
							vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
							vmRtn.ErrorMsg = "更新失敗";
							return Json(vmRtn);
						}

						dbResult = dbAccess.UpdateHandoverMain(vm);

						if (!dbResult.isSuccess)
						{
							dbAccess.DbaRollBack();
							vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
							vmRtn.ErrorMsg = "更新失敗";
							return Json(vmRtn);
						}
					}
					else {
						dbAccess.DbaRollBack();
						vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
						vmRtn.ErrorMsg = "查無交接學生學號";
						return Json(vmRtn);
					}
				}
				else
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "查無申請單";
					return Json(vmRtn);
				}

				dbAccess.DbaCommit();
				vmRtn.ErrorMsg = "已成功將社長交接至 " + NewLeaderSNO;
			}
			catch (Exception ex)
			{
				dbAccess.DbaRollBack();
				vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
				vmRtn.ErrorMsg = "更新失敗" + ex.Message;
				return Json(vmRtn);
			}

			

			return Json(vmRtn);
		}



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
                    return Redirect($"/ClubHandover/HandOver0206?id={id}");
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
			ViewBag.ddlAgree = dbAccess.getAllAgree();

			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0101Model = new ClubHandover0101ViewModel();
			vm.Handover0101Model.SchoolYear = PublicFun.GetNowSchoolYear();
			if (!string.IsNullOrEmpty(id))
			{
				vm.Handover0101Model = dbAccess.GetHandover0101Data(id, LoginUser);
				vm.Handover0101Model.IsEdit = "1";
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

                dbAccess.DbaInitialTransaction();

                var dbResult = new DbExecuteInfo();
				if (vm.Handover0101Model.IsEdit == "1")
				{
					dbResult = dbAccess.Update0101(vm, LoginUser);

					if (!dbResult.isSuccess)
					{
						dbAccess.DbaRollBack();
						vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
						vmRtn.ErrorMsg = "儲存失敗";
						return Json(vmRtn);
					}
					dbAccess.DbaCommit();
					return Json(vmRtn);
				}

				DataTable dtt = new DataTable();

				dbResult = dbAccess.InsertDetail(HoID, "01", "01", LoginUser, out dtt);

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

                var dbResult = new DbExecuteInfo();
                if (vm.Handover0102Model.IsEdit == "1")
                {
                    dbResult = dbAccess.Update0102(vm, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "儲存失敗";
                        return Json(vmRtn);
                    }
                    dbAccess.DbaCommit();
                    return Json(vmRtn);
                }

                DataTable dtt = new DataTable();

                dbResult = dbAccess.InsertDetail(HoID, "01", "02", LoginUser, out dtt);

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
                vm.Handover0103Model = dbAccess.GetHandover0103Data(id, LoginUser);
                vm.Handover0103Model.IsEdit = "1";
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


                var dbResult = new DbExecuteInfo();
                if (vm.Handover0103Model.IsEdit == "1")
                {
                    dbResult = dbAccess.Update0103(vm, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "儲存失敗";
                        return Json(vmRtn);
                    }
                    dbAccess.DbaCommit();
                    return Json(vmRtn);
                }


                DataTable dtt = new DataTable();

                dbResult = dbAccess.InsertDetail(HoID, "01", "03", LoginUser, out dtt);

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
                vm.Handover0204Model.IsEdit = "1";
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

                dbAccess.DbaInitialTransaction();

                var dbResult = new DbExecuteInfo();
                if (vm.Handover0204Model.IsEdit == "1")
                {
                    dbResult = dbAccess.Update0204(vm, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "儲存失敗";
                        return Json(vmRtn);
                    }
                    dbAccess.DbaCommit();
                    return Json(vmRtn);
                }

				DataTable dtt = new DataTable();

				dbResult = dbAccess.InsertDetail(HoID, "02", "04", LoginUser, out dtt);

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

		#region 0205

		[Log(LogActionChineseName.交接準備)]
		public IActionResult HandOver0205(string id)
		{
			ViewBag.ddlYesOrNo = dbAccess.GetYesOrNo();

			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0205Model = new ClubHandover0205ViewModel();
			vm.Handover0205Model.SchoolYear = PublicFun.GetNowSchoolYear();

			if (!string.IsNullOrEmpty(id))
			{
				vm.Handover0205Model = dbAccess.GetHandover0205Data(id, LoginUser);
                vm.Handover0205Model.IsEdit = "1";
            }

			return View(vm);
		}

		[Log(LogActionChineseName.編輯儲存)]
		[ValidateInput(false)]
		public async Task<IActionResult> Save0205(ClubHandoverViewModel vm)
		{
			try
			{
				DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
				string HoID = dt.QueryFieldByDT("HoID");

                if (Request.Form.Files.Count > 0)
				{
					for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
					{
						if (Request.Form.Files[i].Name.Contains("Handover0205Model.UseRecord"))
						{
							var file = Request.Form.Files[i];

							string strFilePath = await upload.UploadFileAsync("HandOverClass05", file);
							vm.Handover0205Model.UseRecordName = file.FileName;
							vm.Handover0205Model.UseRecord = strFilePath;
						}
						else if (Request.Form.Files[i].Name.Contains("Handover0205Model.SchoolProperty"))
						{
							var file = Request.Form.Files[i];

							string strFilePath = await upload.UploadFileAsync("HandOverClass05", file);
							vm.Handover0205Model.SchoolPropertyName = file.FileName;
							vm.Handover0205Model.SchoolProperty = strFilePath;
						}
						else if (Request.Form.Files[i].Name.Contains("Handover0205Model.ClubProperty"))
						{
							var file = Request.Form.Files[i];

							string strFilePath = await upload.UploadFileAsync("HandOverClass05", file);
							vm.Handover0205Model.ClubPropertyName = file.FileName;
							vm.Handover0205Model.ClubProperty = strFilePath;
						}
					}
				}

				dbAccess.DbaInitialTransaction();

                var dbResult = new DbExecuteInfo();
                if (vm.Handover0205Model.IsEdit == "1")
                {
                    dbResult = dbAccess.Update0205(vm, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "儲存失敗";
                        return Json(vmRtn);
                    }
                    dbAccess.DbaCommit();
                    return Json(vmRtn);
                }

                DataTable dtt = new DataTable();

				dbResult = dbAccess.InsertDetail(HoID, "02", "05", LoginUser, out dtt);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
					return Json(vmRtn);
				}

				string HoDetailID = dtt.QueryFieldByDT("HoDetailID");

				dbResult = dbAccess.Insert0205(vm, LoginUser, HoID, HoDetailID);

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
		public IActionResult Print0205(string id)
		{
			DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
			string HoID = dt.QueryFieldByDT("HoID");

			if (!string.IsNullOrEmpty(id))
				HoID = id;

			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0205Model = dbAccess.GetHandover0205Data(HoID, LoginUser);

			return View(vm);
		}

		#endregion

		#region 0206

		[Log(LogActionChineseName.交接準備)]
		public IActionResult HandOver0206(string id)
		{
			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0206Model = new ClubHandover0206ViewModel();
			vm.Handover0206Model.SchoolYear = PublicFun.GetNowSchoolYear();

			if (!string.IsNullOrEmpty(id))
			{
				vm.Handover0206Model = dbAccess.GetHandover0206Data(id, LoginUser);
                vm.Handover0206Model.IsEdit = "1";
            }

			return View(vm);
		}

		[Log(LogActionChineseName.編輯儲存)]
		[ValidateInput(false)]
		public async Task<IActionResult> Save0206(ClubHandoverViewModel vm)
		{
			try
			{
				DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
				string HoID = dt.QueryFieldByDT("HoID");


                if (Request.Form.Files.Count > 0)
				{
					for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
					{
						if (Request.Form.Files[i].Name.Contains("Handover0206Model.Sheet"))
						{
							var file = Request.Form.Files[i];

							string strFilePath = await upload.UploadFileAsync("HandOverClass05", file);
							vm.Handover0206Model.SheetName = file.FileName;
							vm.Handover0206Model.Sheet = strFilePath;
						}
						else if (Request.Form.Files[i].Name.Contains("Handover0206Model.InnerFile"))
						{
							var file = Request.Form.Files[i];

							string strFilePath = await upload.UploadFileAsync("HandOverClass05", file);
							vm.Handover0206Model.InnerFileName = file.FileName;
							vm.Handover0206Model.InnerFile = strFilePath;
						}
					}
				}

				dbAccess.DbaInitialTransaction();

                var dbResult = new DbExecuteInfo();
                if (vm.Handover0206Model.IsEdit == "1")
                {
                    dbResult = dbAccess.Update0206(vm, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "儲存失敗";
                        return Json(vmRtn);
                    }
                    dbAccess.DbaCommit();
                    return Json(vmRtn);
                }

                DataTable dtt = new DataTable();

				dbResult = dbAccess.InsertDetail(HoID, "02", "06", LoginUser, out dtt);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
					return Json(vmRtn);
				}

				string HoDetailID = dtt.QueryFieldByDT("HoDetailID");

				dbResult = dbAccess.Insert0206(vm, LoginUser, HoID, HoDetailID);

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
		public IActionResult Print0206(string id)
		{
			DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
			string HoID = dt.QueryFieldByDT("HoID");

			if (!string.IsNullOrEmpty(id))
				HoID = id;

			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0206Model = dbAccess.GetHandover0206Data(HoID, LoginUser);

			return View(vm);
		}

		#endregion

		#endregion

		#region 03

		[Log(LogActionChineseName.新任營運資訊)]
		public IActionResult HandOver03()
		{
			return View();
		}

		#region 0307

		[Log(LogActionChineseName.新任營運資訊)]
		public IActionResult HandOver0307(string id)
		{
			ViewBag.ddlSex = dbAccess.GetAllSex();

			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0307Model = new ClubHandover0307ViewModel();
			vm.Handover0307Model.SchoolYear = PublicFun.GetNowSchoolYear();

			if (!string.IsNullOrEmpty(id))
			{
				vm.Handover0307Model = dbAccess.GetHandover0307Data(id, LoginUser);
                vm.Handover0307Model.IsEdit = "1";
            }

			return View(vm);
		}

		[Log(LogActionChineseName.編輯儲存)]
		[ValidateInput(false)]
		public IActionResult Save0307(ClubHandoverViewModel vm)
		{
			try
			{
				DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
				string HoID = dt.QueryFieldByDT("HoID");


				dbAccess.DbaInitialTransaction();

                var dbResult = new DbExecuteInfo();
                if (vm.Handover0307Model.IsEdit == "1")
                {
                    dbResult = dbAccess.Update0307(vm, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "儲存失敗";
                        return Json(vmRtn);
                    }
                    dbAccess.DbaCommit();
                    return Json(vmRtn);
                }


                DataTable dtt = new DataTable();

				dbResult = dbAccess.InsertDetail(HoID, "03", "07", LoginUser, out dtt);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
					return Json(vmRtn);
				}

				string HoDetailID = dtt.QueryFieldByDT("HoDetailID");

				dbResult = dbAccess.Insert0307(vm, LoginUser, HoID, HoDetailID);

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
		public IActionResult Print0307(string id)
		{
			DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
			string HoID = dt.QueryFieldByDT("HoID");

			if (!string.IsNullOrEmpty(id))
				HoID = id;



			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0307Model = dbAccess.GetHandover0307Data(HoID, LoginUser);

			return View(vm);
		}

        #endregion

        #region 0308

        [Log(LogActionChineseName.新任營運資訊)]
        public IActionResult HandOver0308(string id)
        {
            ViewBag.ddlSex = dbAccess.GetAllSex();

            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0308Model = new ClubHandover0308ViewModel();
            vm.Handover0308Model.SchoolYear = PublicFun.GetNowSchoolYear();

            if (!string.IsNullOrEmpty(id))
            {
                vm.Handover0308Model = dbAccess.GetHandover0308Data(id, LoginUser);
                vm.Handover0308Model.IsEdit = "1";
            }

            return View(vm);
        }

        [Log(LogActionChineseName.編輯儲存)]
        [ValidateInput(false)]
        public IActionResult Save0308(ClubHandoverViewModel vm)
        {
            try
            {
                DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
                string HoID = dt.QueryFieldByDT("HoID");

                dbAccess.DbaInitialTransaction();

                var dbResult = new DbExecuteInfo();
                if (vm.Handover0308Model.IsEdit == "1")
                {
                    dbResult = dbAccess.Update0308(vm, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "儲存失敗";
                        return Json(vmRtn);
                    }
                    dbAccess.DbaCommit();
                    return Json(vmRtn);
                }

                DataTable dtt = new DataTable();

                dbResult = dbAccess.InsertDetail(HoID, "03", "08", LoginUser, out dtt);

                if (!dbResult.isSuccess)
                {
                    dbAccess.DbaRollBack();
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "儲存失敗";
                    return Json(vmRtn);
                }

                string HoDetailID = dtt.QueryFieldByDT("HoDetailID");

                dbResult = dbAccess.Insert0308(vm, LoginUser, HoID, HoDetailID);

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
        public IActionResult Print0308(string id)
        {
            DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
            string HoID = dt.QueryFieldByDT("HoID");

            if (!string.IsNullOrEmpty(id))
                HoID = id;



            ClubHandoverViewModel vm = new ClubHandoverViewModel();
            vm.Handover0308Model = dbAccess.GetHandover0308Data(HoID, LoginUser);

            return View(vm);
        }

		#endregion

		#region 0309

		[Log(LogActionChineseName.新任營運資訊)]
		public IActionResult HandOver0309(string id)
		{
			ViewBag.ddlSex = dbAccess.GetAllSex();

			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0309Model = new ClubHandover0309ViewModel();
			vm.Handover0309Model.SchoolYear = PublicFun.GetNowSchoolYear();

			if (!string.IsNullOrEmpty(id))
			{
				vm.Handover0309Model = dbAccess.GetHandover0309Data(id, LoginUser);
                vm.Handover0309Model.IsEdit = "1";
            }

			return View(vm);
		}

		[Log(LogActionChineseName.編輯儲存)]
		[ValidateInput(false)]
		public async Task<IActionResult> Save0309Async(ClubHandoverViewModel vm)
		{
			try
			{
				DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
				string HoID = dt.QueryFieldByDT("HoID");

                if (Request.Form.Files.Count > 0)
				{
					for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
					{
						if (Request.Form.Files[i].Name.Contains("Handover0309Model.BookCover"))
						{
							var file = Request.Form.Files[i];

							string strFilePath = await upload.UploadFileAsync("HandOverClass09", file);
							vm.Handover0309Model.BookCoverName = file.FileName;
							vm.Handover0309Model.BookCover = strFilePath;
						}
					}
				}

				dbAccess.DbaInitialTransaction();

                var dbResult = new DbExecuteInfo();
                if (vm.Handover0309Model.IsEdit == "1")
                {
                    dbResult = dbAccess.Update0309(vm, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "儲存失敗";
                        return Json(vmRtn);
                    }
                    dbAccess.DbaCommit();
                    return Json(vmRtn);
                }

                DataTable dtt = new DataTable();

				dbResult = dbAccess.InsertDetail(HoID, "03", "09", LoginUser, out dtt);

				if (!dbResult.isSuccess)
				{
					dbAccess.DbaRollBack();
					vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
					vmRtn.ErrorMsg = "儲存失敗";
					return Json(vmRtn);
				}

				string HoDetailID = dtt.QueryFieldByDT("HoDetailID");

				dbResult = dbAccess.Insert0309(vm, LoginUser, HoID, HoDetailID);

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
		public IActionResult Print0309(string id)
		{
			DataTable dt = dbAccess.GetHoID(LoginUser.LoginId, PublicFun.GetNowSchoolYear());
			string HoID = dt.QueryFieldByDT("HoID");

			if (!string.IsNullOrEmpty(id))
				HoID = id;



			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0309Model = dbAccess.GetHandover0309Data(HoID, LoginUser);

			return View(vm);
		}

		#endregion

		#endregion

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
