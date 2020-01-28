using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewBPMS.Areas.Dev.ViewModels.UserViewModels;
using NewBPMS.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using X.PagedList;

namespace NewBPMS.Areas.Dev.Controllers
{
    [Authorize]
    [Area("Dev")]
    //[Authorize(Roles = "Admin")]
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

        public IActionResult Index(int? page, string StaffName = "")
        {

            var linqVar = _userRepository.EntityItems
                .Where(p => p.StaffName.Contains(StaffName))
                .Select((p => new ApplicationUserViewModel
                {
                    Id = p.Id,
                    StaffNo=p.StaffNo,
                    Email = p.Email,
                    UserName = p.UserName,
                    StaffId = p.StaffId,
                    StaffName = p.StaffName
                }));
            
            var pageNumber = page ?? 1;
            var onePageOflinqVar = linqVar.OrderBy(x => x.StaffNo).ToPagedList(pageNumber, 10);
            var model = new UserIndexViewModel
            {
                StatusMessage = StatusMessage,
                StaffName = StaffName,
                ApplicationUserViewModels = onePageOflinqVar,
            };

            return View(model);
        }

        //public IActionResult Index()
        //{
        //    return View(new IndexViewModel { StatusMessage = StatusMessage });
        //}
        //public async Task<PartialViewResult> List(QueryPagingViewModel<ApplicationUserQuery> queryPagingModel)
        //{
        //    int pageIndex;
        //    int pageSize;
        //    //if (queryPagingModel.PagingInfo == null || !(ModelState.IsValid && TryValidateModel(queryPagingModel.PagingInfo)))
        //    //{
        //    //    pageIndex = _pageSettings.Value.PageIndex;
        //    //    pageSize = _pageSettings.Value.PageSize;
        //    //}
        //    //else
        //    //{
        //    pageIndex = queryPagingModel?.PagingInfo?.CurrentPage ?? _pageSettings.Value.PageIndex;
        //    pageSize = queryPagingModel?.PagingInfo?.ItemsPerPage ?? _pageSettings.Value.PageSize;
        //    //}
        //    var linqVar = IEnumApplicationUserViewModelFactory(queryPagingModel);

        //    Func<ApplicationUserViewModel, string> keySelector = x => x.Email;

        //    var newModel = new ItemListViewModel<ApplicationUserViewModel>
        //    {
        //        ItemViewModels = await _commonRepository.PageListAsync(linqVar, keySelector, pageIndex, pageSize),

        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = pageIndex,
        //            ItemsPerPage = pageSize,
        //            TotalItems = linqVar.Count()
        //        }

        //    };

        //    return PartialView("_ApplicationUserList", newModel);
        //}
        //private IEnumerable<ApplicationUserViewModel> IEnumApplicationUserViewModelFactory(QueryPagingViewModel<ApplicationUserQuery> queryPagingModel)
        //{
        //    var emailQuery = queryPagingModel.QueryItems.Email ?? "";
        //    var userNameQuery = queryPagingModel.QueryItems.UserName ?? "";


        //    var linqVar = _context.Users.AsQueryable()
        //        .Where(p => p.Email.Contains(emailQuery))
        //        .Where(p => p.UserName.Contains(userNameQuery))
        //        .Select((p => new ApplicationUserViewModel
        //        {
        //            Id = p.Id,
        //            Email = p.Email,
        //            UserName = p.UserName,
        //            StaffId = p.StaffId,
        //            StaffName = p.StaffName
        //        }));
        //    return linqVar;
        //}

        ///// <summary>
        ///// 将User表中的StaffId和Staff表中的Id进行匹配
        ///// </summary>
        ///// <param name="Id">User表中的Id</param>
        ///// <returns></returns>
        ///// 需要匹配如下几种情况：
        ///// 1、查询Staff表中where Staff.No==User.StaffNo && Staff.Name=User.StaffName,FirstOrDefault
        ///// 2、若查找到：
        ///// 2.1、User.StaffId!=Staff.Id=>User.StaffId=Staff.Id=>返回匹配成功的消息（请刷新列表查看效果）
        ///// 2.2、User.StaffId==Staff.Id=>返回已匹配过，无须再匹配的消息
        ///// 3、若查找不到：
        ///// 3.1：错误，返回查找不到的消息
        //public async Task<IActionResult> MatchStaff(string Id)
        //{
        //    var user = await _userManager.FindByIdAsync(Id);

        //    var staff = _staffRepository.EntityItems.Where(p => ((p.Name == user.StaffName) && (p.No == user.StaffNo))).FirstOrDefault();

        //    if (user.StaffName == staff.Name && user.StaffNo == staff.No)    //查找到
        //    {
        //        if (user.StaffId == staff.Id)     //已经匹配过
        //        {
        //            StatusMessage = $"{user.StaffName}({user.StaffNo})已经匹配成功，无须再匹配";
        //        }
        //        else
        //        {
        //            user.StaffId = staff.Id;
        //            var result = await _userManager.UpdateAsync(user);
        //            if (result.Succeeded)
        //            {
        //                StatusMessage = $"成功匹配{user.StaffName}({user.StaffNo}),请刷新列表查看效果";
        //            }
        //            else
        //            {
        //                StatusMessage = $"错误：已在职工表查找到{user.StaffName}({user.StaffNo})数据，但匹配失败";
        //            }

        //        }

        //    }
        //    else
        //    {
        //        StatusMessage = $"错误：未在职工表查找到{user.StaffName}({user.StaffNo})数据，请先在职工表添加对应数据";
        //    }

        //    return PartialView("StatusMessagePartial", StatusMessage);

        //}

        ///// <summary>
        ///// 重置密码
        ///// </summary>
        ///// <param name="Id">账户Id</param>
        ///// <returns></returns>

        //public async Task<IActionResult> ResetPassword(string Id)
        //{
        //    var user = await _userManager.FindByIdAsync(Id);

        //    // For more information on how to enable account confirmation and password reset please
        //    // visit https://go.microsoft.com/fwlink/?LinkID=532713
        //    var code = await _userManager.GeneratePasswordResetTokenAsync(user);

        //    var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);

        //    StatusMessage = callbackUrl;

        //    return RedirectToAction(nameof(Index));
        //}



        ///// <summary>
        ///// 列出用户所属角色
        ///// </summary>
        ///// <param name="Id">用户Id</param>
        ///// <returns></returns>
        //public async Task<PartialViewResult> ListByUserId(string Id)
        //{

        //    var linqVar = from p in _context.UserRoles
        //                  join q in _context.Roles
        //                  on p.RoleId equals q.Id
        //                  where p.UserId == Id
        //                  select new UserRoleViewModel
        //                  {
        //                      Name = q.Name
        //                  };

        //    var re = _context.Users.Where(p => p.Id == Id)
        //        .Select(p => new { Email = p.Email, UserName = p.UserName }).FirstOrDefault();


        //    var model = new UserRoleListViewModel
        //    {
        //        UserId = Id,
        //        Email = re.Email,
        //        UserName = re.UserName,
        //        UserRoleViewModels = await linqVar.ToAsyncEnumerable().ToList(),

        //    };

        //    return PartialView("_UserRoles", model);
        //}

        ///// <summary>
        ///// 删除用户所属角色
        ///// </summary>
        ///// <param name="UserId"></param>
        ///// <param name="RoleName"></param>
        ///// <returns></returns>
        //public async Task<IActionResult> DeleteUserRole(string UserId, string RoleName)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = await _userManager.FindByIdAsync(UserId);

        //    IdentityResult result = null;

        //    //判断用户是否已经属于指定角色
        //    if (await _userManager.IsInRoleAsync(user, RoleName))
        //    {
        //        result = await _userManager.RemoveFromRoleAsync(user, RoleName);
        //        StatusMessage = $"已将用户从{RoleName}组中删除";
        //    }

        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError("", result.Errors.First().ToString());
        //    }

        //    var linqVar = from p in _context.UserRoles
        //                  join q in _context.Roles
        //                  on p.RoleId equals q.Id
        //                  where p.UserId == UserId
        //                  select new UserRoleViewModel
        //                  {
        //                      Name = q.Name
        //                  };

        //    var re = _context.Users.Where(p => p.Id == UserId)
        //        .Select(p => new { Email = p.Email, UserName = p.UserName }).FirstOrDefault();


        //    var model = new UserRoleListViewModel
        //    {
        //        UserId = UserId,
        //        Email = re.Email,
        //        UserName = re.UserName,
        //        UserRoleViewModels = await linqVar.ToAsyncEnumerable().ToList(),
        //        StatusMessage = StatusMessage,
        //    };

        //    return PartialView("_UserRoles", model);
        //}

        //public IActionResult CreateUserRolesByUserId(string Id)
        //{
        //    var model = new CreateUserRolesByUserIdViewModel
        //    {
        //        Id = Id,
        //    };
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateUserRolesByUserId(CreateUserRolesByUserIdViewModel model)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var _user = await _userManager.FindByIdAsync(model.Id);


        //    //var _role = RolesFactory(model.Roles);    //重构前代码

        //    var _role = model.Roles.ToString();    //重构后代码

        //    //若该角色不存在
        //    if (!(await _roleManager.RoleExistsAsync(_role)))
        //    {
        //        //StatusMessage 该角色不存在
        //        return View();
        //    }

        //    IdentityResult result = null;

        //    //判断用户是否已经属于指定角色
        //    if (!(await _userManager.IsInRoleAsync(_user, _role)))
        //    {

        //        result = await _userManager.AddToRoleAsync(_user, _role);

        //    }



        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError("", result.Errors.First().ToString());
        //        return View();
        //    }

        //    return View();

        //}

        ////重构后,RolesFactory没有用处
        //private string RolesFactory(RolesEnum rolesEnum)
        //{
        //    string roles = null;
        //    if (rolesEnum == RolesEnum.Admin)
        //    {
        //        roles = "Admin";
        //    }
        //    else if (rolesEnum == RolesEnum.MediumUser)
        //    {
        //        roles = "MediumUser";
        //    }
        //    else if (rolesEnum == RolesEnum.AdvancedUser)
        //    {
        //        roles = "AdvancedUser";
        //    }
        //    else if (rolesEnum == RolesEnum.Manager)
        //    {
        //        roles = "Manager";
        //    }
        //    else
        //    {
        //        roles = null;
        //    }
        //    return roles;
        //}


        ///// <summary>
        ///// 角色管理首页
        ///// </summary>
        ///// <returns></returns>
        //public IActionResult RolesIndex()
        //{
        //    return View();
        //}

        //public IActionResult ListRoles()
        //{
        //    var linqVar = from p in _context.Roles
        //                  select new RolesViewModel
        //                  {
        //                      Name = p.Name,
        //                  };

        //    return PartialView("_RolesList", linqVar);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateRoles(string Name)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    //判断角色是否存在
        //    if (!await _roleManager.RoleExistsAsync(Name))
        //    {
        //        var role = new IdentityRole
        //        {
        //            Name = Name
        //        };
        //        try
        //        {
        //            await _roleManager.CreateAsync(role);
        //        }
        //        catch (DbUpdateException ex)
        //        {

        //            throw ex;
        //        }

        //    }
        //    return RedirectToAction(nameof(RolesIndex));

        //}

        //[HttpGet]
        //public async Task<IActionResult> DeleteRoles(string Name)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    //判断角色是否存在
        //    if (await _roleManager.RoleExistsAsync(Name))
        //    {
        //        var role = await _roleManager.FindByNameAsync(Name);
        //        if (role != null)
        //        {
        //            try
        //            {
        //                var r = await _roleManager.DeleteAsync(role);
        //            }
        //            catch (DbUpdateException ex)
        //            {

        //                throw ex;
        //            }
        //        }
        //    }
        //    return RedirectToAction(nameof(RolesIndex));

        //}

        //// GET: Authorize/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Authorize/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Authorize/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Authorize/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Authorize/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}


        //[HttpGet]
        //public async Task<IActionResult> DeleteUser(string Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _userManager.FindByIdAsync(Id);

        //    var model = new DeleteUserViewModel
        //    {
        //        Id = user.Id,
        //        Email = user.Email,
        //        StaffNo = user.StaffNo,
        //        StaffName = user.StaffName,
        //    };
        //    return View(model);
        //}

        //[HttpPost, ActionName("DeleteUser")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string Id)
        //{
        //    var user = await _userManager.FindByIdAsync(Id);

        //    //当前登录用户不能对自己进行删除
        //    if (user == await _userManager.GetUserAsync(User))
        //    {
        //        StatusMessage = "错误：不能对自己进行删除";
        //    }
        //    else
        //    {
        //        try
        //        {

        //            if (user == null)
        //            {
        //                return NotFound();
        //            }

        //            var StaffNameToDelete = user.StaffName;
        //            var StaffNoToDelete = user.StaffNo;

        //            var varDeleted = await _userManager.DeleteAsync(user);

        //            if (varDeleted != null)
        //            {
        //                StatusMessage = $"成功删除{user.StaffName}({user.StaffNo})数据";
        //            }
        //        }
        //        catch (DbUpdateException /* ex */)
        //        {
        //            StatusMessage = "错误:删除失败。请重试, 如果该问题仍然存在 " + "请联系系统管理员。";
        //        }
        //    }


        //    return RedirectToAction(nameof(Index));
        //}

        //// POST: Authorize/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}