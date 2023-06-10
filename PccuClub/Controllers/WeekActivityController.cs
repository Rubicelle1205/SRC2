using Microsoft.AspNetCore.Mvc;
using WebPccuClub.Models;
using System.Diagnostics;
using WebPccuClub.Global;
using WebPccuClub.DataAccess;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebPccuClub.Controllers
{
    [LogAttribute(LogActionChineseName.一周場地時間)]
    public class WeekActivityController : FBaseController
    {
        ReturnViewModel vmRtn = new ReturnViewModel();
        WeekActivityDataAccess dbAccess = new WeekActivityDataAccess();

        private readonly IHostingEnvironment hostingEnvironment;

        public WeekActivityController(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.ddlAllBuild = dbAccess.GetAllBuild();

            WeekActivityViewModel vm = new WeekActivityViewModel();
            vm.ConditionModel = new WeekActivityConditionModel();
            return View(vm);
        }

        [LogAttribute(LogActionChineseName.查詢)]
        public IActionResult GetSearchResult(WeekActivityViewModel vm)
        {
            List<DateTime> LstDate = new List<DateTime>();
            
            
            DateTime SDate = DateTime.Parse(vm.ConditionModel.SDate);

            for (DateTime i = SDate; i <= SDate.AddDays(6); i = i.AddDays(1))
            {
                LstDate.Add(i);
            }

            List<PlaceData> LstBasePlaceData = dbAccess.GetPlaceData(vm.ConditionModel.BuildID);

            

            List<WeekActClubData> LstActClubData = dbAccess.GetSearchResult(vm.ConditionModel).ToList();

            vm.ResultModel = new List<WeekActivityResultModel>();

            for (int i = 0; i <= LstDate.Count - 1; i++)
            {
                WeekActivityResultModel result = new WeekActivityResultModel();
                result.Date = LstDate[i].ToString("yyyy-MM-dd");

                var LstItemActClubData = LstActClubData.Where(x => x.Date == result.Date).ToList();

                foreach (var item in LstBasePlaceData)
                {
                    PlaceData p = new PlaceData();
                    p.PlaceID = item.PlaceID;
                    p.PlaceName = item.PlaceName;
                    p.LstActClubData = LstItemActClubData.Where(x => x.ActPlaceID == item.PlaceID).ToList();

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