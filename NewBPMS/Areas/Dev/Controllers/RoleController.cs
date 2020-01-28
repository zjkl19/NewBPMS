using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewBPMS.Areas.Dev.ViewModels.RoleViewModels;
using NewBPMS.IRepository;
using NewBPMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NewBPMS.Areas.Dev.Controllers
{
    [Authorize]
    [Area("Dev")]
    public class RoleController : Controller
    {
        private readonly IRoleManager _roleManager;
        private readonly IRoleRepository _roleRepository;

        public RoleController(
            IRoleManager roleManager
            ,IRoleRepository roleRepository)
        {
            _roleManager = roleManager;
            _roleRepository = roleRepository;
        }

        [TempData]
        public string StatusMessage { get; set; }
        
        [HttpGet]
        public IActionResult Index()
        {
            var linqVar = _roleRepository.EntityItems
                 .Select((p => new RoleViewModel
                 {
                     Id = p.Id,
                     Name=p.Name,
                     Description=p.Description
                 }));
            var model = new RoleIndexViewModel
            {
                StatusMessage = StatusMessage,
                RoleViewModels=linqVar.OrderBy(x=>x.Name),
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //判断角色是否存在
            if (!await _roleManager.RoleExistsAsync(model.Name))
            {
                var role = new ApplicationRole
                {
                    Name = model.Name,
                    Description= model.Description
                };
                try
                {
                    await _roleManager.CreateAsync(role);
                }
                catch (DbUpdateException ex)
                {

                    throw ex;
                }

            }
            return RedirectToAction(nameof(Index));

        }
    }
}