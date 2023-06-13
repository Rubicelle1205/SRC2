using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
    [LogAttribute(LogActionChineseName.會員及幹部登錄)]
    public class ClubPersonMangController : FBaseController
	{
        PublicFun PublicFun = new PublicFun();
        ReturnViewModel vmRtn = new ReturnViewModel();
        ClubPersonMangDataAccess dbAccess = new ClubPersonMangDataAccess();
        UploadUtil upload = new UploadUtil();

        private readonly IHostingEnvironment hostingEnvironment;

        public ClubPersonMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        [Log(LogActionChineseName.首頁)]
        public IActionResult Index()
        {
            return View();
        }

        #region 幹部

        [Log(LogActionChineseName.前台幹部名冊)]
		public IActionResult CadreIndex()
		{
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
            vm.CadreMangConditionModel = new ClubCadreMangConditionModel();
            vm.CadreMangConditionModel.SchoolYear = PublicFun.GetNowSchoolYear();
            return View(vm);
        }

		[LogAttribute(LogActionChineseName.查詢)]
		public IActionResult GetCadreMangSearchResult(ClubPersonMangViewModel vm)
		{
			vm.CadreMangResultModel = dbAccess.GetCadreSearchResult(vm.CadreMangConditionModel, LoginUser).ToList();

			#region 分頁
			vm.CadreMangConditionModel.TotalCount = vm.CadreMangResultModel.Count();
			int StartRow = vm.CadreMangConditionModel.Page * vm.CadreMangConditionModel.PageSize;
			vm.CadreMangResultModel = vm.CadreMangResultModel.Skip(StartRow).Take(vm.CadreMangConditionModel.PageSize).ToList();
			#endregion

			return PartialView("_SearchCadreMangResultPartial", vm);
		}

		[Log(LogActionChineseName.新增)]
		public IActionResult CadreCreate()
		{
			ViewBag.ddlAllSex = dbAccess.GetAllSex();

            ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
			vm.CadreMangCreateModel = new ClubCadreMangCreateModel();
            vm.CadreMangCreateModel.SchoolYear = PublicFun.GetNowSchoolYear();

            return View(vm);
		}

		[Log(LogActionChineseName.編輯)]
        public IActionResult CadreEdit(string submitBtn, ClubPersonMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlAllSex = dbAccess.GetAllSex();

            //ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
            vm.CadreMangEditModel = dbAccess.GetCadreEditData(submitBtn);
			return View(vm);
        }

		[Log(LogActionChineseName.匯入)]
		public IActionResult CadreUpload()
		{
			ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
			return View(vm);
		}

        [LogAttribute(LogActionChineseName.下載template檔案)]
        public IActionResult DownloadCadreTemplate()
        {
            string FileName = "幹部名冊_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }

        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public IActionResult CadreMangSaveNewData(ClubPersonMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.CadreMangInsertData(vm, LoginUser);

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
        public IActionResult CadreMangEditOldData(ClubPersonMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.CadreMangUpdateData(vm, LoginUser);

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
        public IActionResult CadreMangDelete(string Ser)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.CadreMangDeletetData(Ser);

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
		public IActionResult ExportCadreMangSearchResult(ClubPersonMangViewModel vm)
		{
			string FileName = string.Format("{0}_{1}_{2}",LoginUser.LoginId, LogActionChineseName.幹部名冊, DateTime.Now.ToString("yyyyMMdd"));
			vm.CadreMangExcelModel = dbAccess.GetCadreMangExportResult(vm.CadreMangConditionModel, LoginUser);

			if (vm.CadreMangExcelModel != null && vm.CadreMangExcelModel.Count > 0)
			{
				IWorkbook workbook = new XSSFWorkbook();
				List<int> LstWidth = new List<int> { 20, 50, 20, 20, 20, 20, 20, 30 };

				ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

				var properties = typeof(ClubCadreMangExcelResultModel).GetProperties();

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
				for (int i = 0; i <= vm.CadreMangExcelModel.Count - 1; i++)
				{
					IRow dataRow = sheet.CreateRow(i + 1);

					dataRow.CreateCell(0).SetCellValue(vm.CadreMangExcelModel[i].SchoolYear);
					dataRow.CreateCell(1).SetCellValue(vm.CadreMangExcelModel[i].ClubName);
					dataRow.CreateCell(2).SetCellValue(vm.CadreMangExcelModel[i].UserName);
					dataRow.CreateCell(3).SetCellValue(vm.CadreMangExcelModel[i].Department);
					dataRow.CreateCell(4).SetCellValue(vm.CadreMangExcelModel[i].CadreName);
					dataRow.CreateCell(5).SetCellValue(vm.CadreMangExcelModel[i].SDuring.Value.ToString("yyyy/MM/dd"));
					dataRow.CreateCell(6).SetCellValue(vm.CadreMangExcelModel[i].EDuring.Value.ToString("yyyy/MM/dd"));
					dataRow.CreateCell(7).SetCellValue(vm.CadreMangExcelModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm:ss"));

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

		[LogAttribute(LogActionChineseName.匯入Excel)]
		public IActionResult ImportCadreMangExcel(ClubPersonMangViewModel vm)
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

				List<ClubCadreMangImportExcelResultModel> LstExcel = new List<ClubCadreMangImportExcelResultModel>();

				using (Stream stream = vm.File.OpenReadStream())
				{
					XSSFWorkbook workbook = new XSSFWorkbook(stream);
					ISheet sheet = workbook.GetSheetAt(0);

					for (int i = 1; i <= sheet.LastRowNum; i++)
					{
						IRow row = sheet.GetRow(i);

						for (int j = 0; j <= row.Count() - 1; j++)
						{
							row.GetCell(j)?.SetCellType(CellType.String);
						}

						if (row != null && !string.IsNullOrEmpty(row.GetCell(0)?.ToString()))
						{
							string Sex = "";
							string SDring = "";
							string EDring = "";
							
							List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstSex = dbAccess.GetAllSex();

							if (PublicFun.GetNowSchoolYear() != row.GetCell(0)?.ToString())
							{
								vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
								vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", "僅可上傳本學年度幹部資料");
								return Json(vmRtn);
							}

							if (LoginUser.LoginId != row.GetCell(1)?.ToString())
							{
								vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
								vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", "僅可上傳自己社團的幹部資料");
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

							ClubCadreMangImportExcelResultModel excel = new ClubCadreMangImportExcelResultModel
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
					var dbResult = dbAccess.CadreMangImportData(LstExcel, LoginUser);

					if (!dbResult.isSuccess)
					{
						dbAccess.DbaRollBack();
						vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
						vmRtn.ErrorMsg = "上傳失敗";
					}
				}
				else {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "檢核檔案可匯入檔案資料為0";
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

        #region 幹部

        [LogAttribute(LogActionChineseName.前台幹部個資同意書)]
        public IActionResult CadreUploadPersonalConsent()
		{
            ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
			vm.CadreMangPersonalConsentModel = new ClubCadreMangPersonalConsentModel();
            return PartialView("CadreUploadPersonalConsent", vm);
		}

        [LogAttribute(LogActionChineseName.前台幹部個資同意書上傳)]
        public async Task<IActionResult> CadreUploadPersonalCon(ClubPersonMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("PersonalConsent"))
                        {
                            var file = Request.Form.Files.GetFile("CadreMangPersonalConsentModel.PersonalConsent");

                            string strFilePath = await upload.UploadFileAsync("PersonalConsent", file);

                            vm.CadreMangPersonalConsentModel.PersonalConsent = strFilePath;
                        }
                    }
                }

				vm.CadreMangPersonalConsentModel.ClubID = LoginUser.LoginId;
				vm.CadreMangPersonalConsentModel.SchoolYear = PublicFun.GetNowSchoolYear();

				DataTable dt = dbAccess.ChkHasCadrePersonConData(vm, LoginUser);

				if (dt.Rows.Count > 0)
				{
					var dbResult = dbAccess.CadreMangUpdatePersonalConsentData(vm, LoginUser);

					if (!dbResult.isSuccess)
					{
						dbAccess.DbaRollBack();
						vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
						vmRtn.ErrorMsg = "修改失敗";
						return Json(vmRtn);
					}
				}
				else {
                    var dbResult = dbAccess.CadreMangInsertPersonalConsentData(vm, LoginUser);

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

        #endregion

        #endregion

        #region 會員

        [Log(LogActionChineseName.前台會員名冊)]
        public IActionResult MemberIndex()
        {
            ViewBag.ddlSchoolYear = dbAccess.GetSchoolYear();

            ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
            vm.MemberMangConditionModel = new ClubMemberMangConditionModel();
            vm.MemberMangConditionModel.SchoolYear = PublicFun.GetNowSchoolYear();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetMemberMangSearchResult(ClubPersonMangViewModel vm)
        {
            vm.MemberMangResultModel = dbAccess.GetMemberSearchResult(vm.MemberMangConditionModel, LoginUser).ToList();

            #region 分頁
            vm.MemberMangConditionModel.TotalCount = vm.MemberMangResultModel.Count();
            int StartRow = vm.MemberMangConditionModel.Page * vm.MemberMangConditionModel.PageSize;
            vm.MemberMangResultModel = vm.MemberMangResultModel.Skip(StartRow).Take(vm.MemberMangConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchMemberMangResultPartial", vm);
        }

        [Log(LogActionChineseName.新增)]
        public IActionResult MemberCreate()
        {
            ViewBag.ddlAllSex = dbAccess.GetAllSex();

            ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
            vm.MemberMangCreateModel = new ClubMemberMangCreateModel();
            vm.MemberMangCreateModel.SchoolYear = PublicFun.GetNowSchoolYear();

            return View(vm);
        }

        [Log(LogActionChineseName.編輯)]
        public IActionResult MemberEdit(string submitBtn, ClubPersonMangViewModel vm)
        {
            if (string.IsNullOrEmpty(submitBtn))
                return RedirectToAction("Index");

            ViewBag.ddlAllSex = dbAccess.GetAllSex();

            //ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
            vm.MemberMangEditModel = dbAccess.GetMemberEditData(submitBtn);
            return View(vm);
        }

        [Log(LogActionChineseName.匯入)]
        public IActionResult MemberUpload()
        {
            ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.下載template檔案)]
        public IActionResult DownloadMemberTemplate()
        {
            string FileName = "會員名冊_template.xlsx";

            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, "Template", FileName);

            byte[] fileContents = System.IO.File.ReadAllBytes(filePath);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);

        }

        [Log(LogActionChineseName.新增儲存)]
        [ValidateInput(false)]
        public IActionResult MemberMangSaveNewData(ClubPersonMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.MemberMangInsertData(vm, LoginUser);

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
        public IActionResult MemberMangEditOldData(ClubPersonMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.MemberMangUpdateData(vm, LoginUser);

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
        public IActionResult MemberMangDelete(string Ser)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                var dbResult = dbAccess.MemberMangDeletetData(Ser);

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
        public IActionResult ExportMemberMangSearchResult(ClubPersonMangViewModel vm)
        {
            string FileName = string.Format("{0}_{1}_{2}", LoginUser.LoginId, LogActionChineseName.會員名冊, DateTime.Now.ToString("yyyyMMdd"));
            vm.MemberMangExcelModel = dbAccess.GetMemberMangExportResult(vm.MemberMangConditionModel, LoginUser);

            if (vm.MemberMangExcelModel != null && vm.MemberMangExcelModel.Count > 0)
            {
                IWorkbook workbook = new XSSFWorkbook();
                List<int> LstWidth = new List<int> { 20, 50, 20, 20, 20, 20, 20, 30 };

                ISheet sheet = ExcelUtil.GenNewSheet(workbook, "Sheet1", LstWidth);

                var properties = typeof(ClubMemberMangExcelResultModel).GetProperties();

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
                for (int i = 0; i <= vm.MemberMangExcelModel.Count - 1; i++)
                {
                    IRow dataRow = sheet.CreateRow(i + 1);

                    dataRow.CreateCell(0).SetCellValue(vm.MemberMangExcelModel[i].SchoolYear);
                    dataRow.CreateCell(1).SetCellValue(vm.MemberMangExcelModel[i].ClubName);
                    dataRow.CreateCell(2).SetCellValue(vm.MemberMangExcelModel[i].UserName);
                    dataRow.CreateCell(3).SetCellValue(vm.MemberMangExcelModel[i].Department);
                    dataRow.CreateCell(4).SetCellValue(vm.MemberMangExcelModel[i].SDuring.Value.ToString("yyyy/MM/dd"));
                    dataRow.CreateCell(5).SetCellValue(vm.MemberMangExcelModel[i].EDuring.Value.ToString("yyyy/MM/dd"));
                    dataRow.CreateCell(6).SetCellValue(vm.MemberMangExcelModel[i].Created.Value.ToString("yyyy/MM/dd HH:mm:ss"));

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

        [LogAttribute(LogActionChineseName.匯入Excel)]
        public IActionResult ImportMemberMangExcel(ClubPersonMangViewModel vm)
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

                if (!vm.File.FileName.Contains(LogActionChineseName.會員名冊.ToString()))
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "選擇檔案錯誤";
                    return Json(vmRtn);
                }

                List<ClubMemberMangImportExcelResultModel> LstExcel = new List<ClubMemberMangImportExcelResultModel>();

                using (Stream stream = vm.File.OpenReadStream())
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);

                        for (int j = 0; j <= row.Count() - 1; j++)
                        {
                            row.GetCell(j)?.SetCellType(CellType.String);
                        }

                        if (row != null && !string.IsNullOrEmpty(row.GetCell(0)?.ToString()))
                        {
                            string Sex = "";
                            string SDring = "";
                            string EDring = "";

                            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> LstSex = dbAccess.GetAllSex();

                            if (PublicFun.GetNowSchoolYear() != row.GetCell(0)?.ToString())
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", "僅可上傳本學年度幹部資料");
                                return Json(vmRtn);
                            }

                            if (LoginUser.LoginId != row.GetCell(1)?.ToString())
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", "僅可上傳自己社團的幹部資料");
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

                            if (!PublicFun.ChkDateFormat(row.GetCell(8).ToString().TrimStartAndEnd(), out SDring))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(8).ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }

                            if (!PublicFun.ChkDateFormat(row.GetCell(9).ToString().TrimStartAndEnd(), out EDring))
                            {
                                vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                                vmRtn.ErrorMsg = string.Format("檢核資料失敗:{0}", row.GetCell(9).ToString().TrimStartAndEnd());
                                return Json(vmRtn);
                            }

                            ClubMemberMangImportExcelResultModel excel = new ClubMemberMangImportExcelResultModel
                            {
                                SchoolYear = row.GetCell(0)?.StringCellValue.TrimStartAndEnd(),
                                ClubID = row.GetCell(1)?.StringCellValue.TrimStartAndEnd(),
                                UserName = row.GetCell(2)?.StringCellValue.TrimStartAndEnd(),
                                SNo = row.GetCell(3)?.StringCellValue.TrimStartAndEnd(),
                                Sex = Sex,
                                CellPhone = row.GetCell(5)?.StringCellValue.TrimStartAndEnd(),
                                EMail = row.GetCell(6)?.StringCellValue.TrimStartAndEnd(),
                                Department = row.GetCell(7)?.StringCellValue.TrimStartAndEnd(),
                                SDuring = SDring,
                                EDuring = EDring,
                                Memo = row.GetCell(10)?.StringCellValue.TrimStartAndEnd()
                            };

                            LstExcel.Add(excel);
                        }
                    }
                }

                dbAccess.DbaInitialTransaction();
                if (LstExcel.Count > 0)
                {
                    var dbResult = dbAccess.MemberMangImportData(LstExcel, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "上傳失敗";
                    }
                }
                else
                {
                    vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                    vmRtn.ErrorMsg = "檢核檔案可匯入檔案資料為0";
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

        #region 幹部

        [LogAttribute(LogActionChineseName.前台會員個資同意書)]
        public IActionResult MemberUploadPersonalConsent()
        {
            ClubPersonMangViewModel vm = new ClubPersonMangViewModel();
            vm.MemberMangPersonalConsentModel = new ClubMemberMangPersonalConsentModel();
            return PartialView("MemberUploadPersonalConsent", vm);
        }

        

        [LogAttribute(LogActionChineseName.前台會員個資同意書上傳)]
        public async Task<IActionResult> MemberUploadPersonalCon(ClubPersonMangViewModel vm)
        {
            try
            {
                dbAccess.DbaInitialTransaction();

                if (Request.Form.Files.Count > 0)
                {
                    for (int i = 0; i <= Request.Form.Files.Count - 1; i++)
                    {
                        if (Request.Form.Files[i].Name.Contains("PersonalConsent"))
                        {
                            var file = Request.Form.Files.GetFile("MemberMangPersonalConsentModel.PersonalConsent");

                            string strFilePath = await upload.UploadFileAsync("PersonalConsent", file);

                            vm.MemberMangPersonalConsentModel.PersonalConsent = strFilePath;
                        }
                    }
                }

                vm.MemberMangPersonalConsentModel.ClubID = LoginUser.LoginId;
                vm.MemberMangPersonalConsentModel.SchoolYear = PublicFun.GetNowSchoolYear();

                DataTable dt = dbAccess.ChkHasMemberPersonConData(vm, LoginUser);

                if (dt.Rows.Count > 0)
                {
                    var dbResult = dbAccess.MemberMangUpdatePersonalConsentData(vm, LoginUser);

                    if (!dbResult.isSuccess)
                    {
                        dbAccess.DbaRollBack();
                        vmRtn.ErrorCode = (int)DBActionChineseName.失敗;
                        vmRtn.ErrorMsg = "修改失敗";
                        return Json(vmRtn);
                    }
                }
                else
                {
                    var dbResult = dbAccess.MemberMangInsertPersonalConsentData(vm, LoginUser);

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

        #endregion


        #endregion

        [LogAttribute(LogActionChineseName.前台個資同意書template下載)]
        public IActionResult DownloadPDF()
        {
            return View("_PersonalConsent");
        }
    }
}
