using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewBPMS.Areas.Dev.ViewModels.UserRoleViewModels;
using NewBPMS.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace NewBPMS.Areas.Dev.Controllers
{
    [Authorize]
    [Area("Dev")]
    public class UserRoleController : Controller
    {
        private readonly IUserManagerRepository _userManager;
        private readonly IRoleManager _roleManager;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public UserRoleController(
            IUserManagerRepository userManager
            ,IRoleManager roleManager
            , IUserRoleRepository userRoleRepository
            , IRoleRepository roleRepository
            ,IUserRepository userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// 列出用户所属角色
        /// </summary>
        /// <param name="Id">用户Id</param>
        /// <returns></returns>
        public async Task<IActionResult> ListByUserId(string Id)
        {
            var linqVar = from p in _userRoleRepository.EntityItems
                          join q in _roleRepository.EntityItems
                          on p.RoleId equals q.Id
                          where p.UserId == Id
                          select new UserRoleViewModel
                          {
                              RoleName = q.Name
                          };

            var re = _userRepository.EntityItems.Where(p => p.Id == Id)
                .Select(p => new { p.Email, p.UserName, p.StaffName}).FirstOrDefault();

            var model = new UserRoleListViewModel
            {
                UserId = Id,
                Email = re.Email,
                UserName = re.UserName,
                StaffName=re.StaffName,
                UserRoleViewModels = linqVar.ToList(),

            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Create(string UserId)
        {
            var model = new CreateUserRoleViewModel
            {
                StatusMessage=StatusMessage,
                UserId = UserId,
                RoleSelectViewModels = _roleRepository.EntityItems.Select(x => new RoleSelectViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserRoleViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var _user = await _userManager.FindByIdAsync(model.UserId);

            var _role = _roleRepository.EntityItems.Where(x => x.Id == model.RoleSelect).FirstOrDefault().Name;

            //若该角色不存在
            if (!(await _roleManager.RoleExistsAsync(_role)))
            {
                //TODO:StatusMessage 该角色不存在
                return RedirectToAction(nameof(Create), new { model.UserId });
            }

            IdentityResult result = null;

            //判断用户是否已经属于指定角色
            if (!(await _userManager.IsInRoleAsync(_user, _role)))
            {
                StatusMessage =$"您已成功将该用户加入角色{_role}";
                result = await _userManager.AddToRoleAsync(_user, _role);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Errors.First().ToString());
                return RedirectToAction(nameof(Create), new { model.UserId });
            }

            return RedirectToAction(nameof(Create),new { model.UserId });

        }
    }
}