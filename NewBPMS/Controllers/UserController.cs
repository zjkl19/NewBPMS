using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewBPMS.IRepository;
using NewBPMS.Models;
using NewBPMS.ViewModels;

namespace NewBPMS.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UserController(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [NonAction]
        public IActionResult Index()
        {
            return View();
        }

        //[PagingAction]
        public async Task<PartialViewResult> SelectList([Bind(Prefix = "QueryItems")]UserSelectQuery queryModel, [Bind(Prefix = "PagingInfo")]PagingInfo pagingInfo)
        {
            int pageIndex = pagingInfo?.CurrentPage ?? 1;
            int pageSize = pagingInfo?.ItemsPerPage ?? 10;
            IEnumerable<UserSelectViewModel> linqVar = IEnumUserSelectViewModelFactory(queryModel);

            var newModel = new ItemListViewModel<UserSelectViewModel>
            {

                ItemViewModels = await Task.FromResult(linqVar.OrderBy(x => x.No).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()).ConfigureAwait(false),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageIndex,
                    ItemsPerPage = pageSize,
                    TotalItems = linqVar.Count()
                }

            };

            return PartialView("_UserSelectListPartial", newModel);
        }

        private IEnumerable<UserSelectViewModel> IEnumUserSelectViewModelFactory(UserSelectQuery queryModel)
        {
            int StaffNoUpperBound;   //查询值上界
            int StaffNoLowerBound;   //查询值下界

            string nameQuery;   //按姓名查询
            nameQuery = queryModel.Name ?? "";

            StaffNoUpperBound = queryModel.No ?? int.MaxValue;
            StaffNoLowerBound = queryModel.No ?? 1;

            Expression<Func<ApplicationUser, bool>> queryExpression =
                p => p.StaffName.Contains(nameQuery)
                && (p.StaffNo >= StaffNoLowerBound && p.StaffNo <= StaffNoUpperBound);
            //var lq = _mainRepository.EntityItems.Where(p => p.Name.Contains(nameQuery) && (p.No >= StaffNoLowerBound && p.No <= StaffNoUpperBound));

            //var linqVar =  _mainRepository.QueryEntity<Staff>(queryExpression);

            var linqVar = _userRepository.QueryEntity<ApplicationUser>(queryExpression)
                .Select(p => new UserSelectViewModel
                {
                    Id=p.Id,
                    No = p.StaffNo,
                    Name = p.StaffName,
                });
            return linqVar;
        }
    }
}