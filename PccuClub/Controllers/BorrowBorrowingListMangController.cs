using Microsoft.AspNetCore.Mvc;
using WebPccuClub.Models;
using System.Diagnostics;
using WebPccuClub.Global;
using WebPccuClub.DataAccess;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.資源借用一覽表)]
    public class BorrowBorrowingListMangController : BaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        BorrowBorrowingListMangDataAccess dbAccess = new BorrowBorrowingListMangDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public BorrowBorrowingListMangController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.ddlAllSecondClassID = dbAccess.GetAllMainResourceID();

            BorrowBorrowingListMangViewModel vm = new BorrowBorrowingListMangViewModel();
            vm.ConditionModel = new BorrowBorrowingListMangConditionModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(BorrowBorrowingListMangViewModel vm)
        {
            List<DateTime> LstDate = new List<DateTime>();
            
            
            DateTime SDate = DateTime.Parse(vm.ConditionModel.SDate);

            for (DateTime i = SDate; i <= SDate.AddDays(6); i = i.AddDays(1))
            {
                LstDate.Add(i);
            }

            List<BorrowBorrowingUnitData> LstBasePlaceData = dbAccess.GetResurceData(vm.ConditionModel.BorrowMainClassID);

            

            List<BorrowUnitData> LstActClubData = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            vm.ResultModel = new List<BorrowBorrowingListMangResultModel>();

            for (int i = 0; i <= LstDate.Count - 1; i++)
            {
                BorrowBorrowingListMangResultModel result = new BorrowBorrowingListMangResultModel();
                result.Date = LstDate[i].ToString("yyyy-MM-dd");

                var LstItemActClubData = LstActClubData.Where(x => x.Date.Value.ToString("yyyy-MM-dd") == result.Date).ToList();

                foreach (var item in LstBasePlaceData)
                {
                    BorrowBorrowingUnitData p = new BorrowBorrowingUnitData();
                    p.MainResourceID = item.MainResourceID;
                    p.SecondResourceName = item.SecondResourceName;
                    p.LstBorrowUnitData = LstItemActClubData.Where(x => x.MainResourceID == item.MainResourceID).ToList();

                    result.LstPlaceData.Add(p);
                }
                vm.ResultModel.Add(result);
            }

            #region 分頁
            vm.ConditionModel.TotalCount = vm.ResultModel.Count();
            int StartRow = vm.ConditionModel.Page * vm.ConditionModel.PageSize;
            vm.ResultModel = vm.ResultModel.Skip(StartRow).Take(vm.ConditionModel.PageSize).ToList();
            #endregion

            return PartialView("_SearchResultPartial", vm);
        }

    }
}