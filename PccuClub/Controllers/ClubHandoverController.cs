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

        #region 0101


        [Log(LogActionChineseName.社團負責人改選管理)]
		public IActionResult HandOver01()
		{
			return View();
		}

		[Log(LogActionChineseName.社團負責人改選管理)]
		public IActionResult HandOver0101(string id)
		{
			ClubHandoverViewModel vm = new ClubHandoverViewModel();
			vm.Handover0101Model = new ClubHandover0101ViewModel();

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
				vm2.Handover0101Model = dbAccess.GetHandover0101Data(HoID, LoginUser);

                if (vm2.Handover0101Model != null)
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

		[Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0102()
        {
            return View();
        }

        [Log(LogActionChineseName.社團負責人改選管理)]
        public IActionResult HandOver0103()
        {
            return View();
        }


        [Log(LogActionChineseName.交接準備)]
		public IActionResult HandOver02()
		{
			return View();
		}

        [Log(LogActionChineseName.交接準備)]
        public IActionResult HandOver0204()
        {
            return View();
        }

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

                if (!dbResult.isSuccess)
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

                dbResult = dbAccess.InsertFile01(vm, LoginUser, HoID, HoDetailID);

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
            return View();
        }

        [Log(LogActionChineseName.新任營運資訊)]
        public IActionResult HandOverFile03()
        {
            return View();
        }

        #endregion



    }
}
