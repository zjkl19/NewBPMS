using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewBPMS.IControllerServices;
using NewBPMS.ViewModels.UserContractViewModels;

namespace NewBPMS.Controllers
{
    public class UserContractController : Controller
    {
        private readonly IUserContractService _userContractService;

        public UserContractController(
             IUserContractService userContractService)
        {
            _userContractService = userContractService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SummaryUserProductValue([Bind(Prefix = "SummaryUserProductValueQueryViewModel")]SummaryUserProductValueQueryViewModel queryModel)
        {
            var startDateTime = queryModel.StartDateTime;
            var endDateTime = queryModel.EndDateTime;

            var l = _userContractService.GetSummaryUserProductValue(queryModel).ToList();

            return PartialView("_SummaryProductValueList", l);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult List(ListViewModel model)
        //{
        //    var startDateTime = model.StartDateTime;
        //    var endDateTime = model.EndDateTime;

        //    var listUserScoreViewModels = new List<ListUserScoreViewModel>();

        //    //积分查询人数最大不超过maxListUsers
        //    for (int i = 0; i < (model.ItemChosen.Count <= maxListUsers ? model.ItemChosen.Count : maxListUsers); i++)
        //    {
        //        if (model.ItemChosen[i] != "false")
        //        {
        //            listUserScoreViewModels.Add(new ListUserScoreViewModel
        //            {
        //                StaffNo = _userRepository.EntityItems.Where(x => x.Id == model.ItemChosen[i]).FirstOrDefault().StaffNo,
        //                StaffName = _userRepository.EntityItems.Where(x => x.Id == model.ItemChosen[i]).FirstOrDefault().StaffName,
        //                Score = _scoreRecordRepository.EntityItems.Where(x => x.UserId == model.ItemChosen[i] && x.ScoreTime >= startDateTime && x.ScoreTime <= endDateTime).Sum(x => x.Score),
        //            });
        //        }
        //    }

        //    return PartialView("ListUserScorePartial", listUserScoreViewModels);
        //}
    }
}