using Microsoft.AspNetCore.Mvc;
using Work_Wave.Services.Interfaces;
using Work_Wave.Models.ViewModels;
using Work_Wave.Models;
using Work_Wave.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace Work_Wave.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly ITRolesService _rolesService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<WaveUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public UserRolesController(ITRolesService rolesService, ApplicationDbContext context, UserManager<WaveUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _rolesService = rolesService;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles()
        {
            List<ManageUserRolesViewModel> model = new();

            List<WaveUser> users = await _context.Users.ToListAsync();

            foreach(WaveUser user in users)
            {
                ManageUserRolesViewModel viewModel = new();

                viewModel.User = user;

                IEnumerable<string> selected = await _rolesService.GetUserRolesAsync(user);

                viewModel.Roles = new MultiSelectList(await _rolesService.GetRolesAync(), "Name", "Name", selected);

                model.Add(viewModel);
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel member)
        {
            WaveUser waveUser = (await _context.Users.ToListAsync()).FirstOrDefault(u => u.Id == member.User.Id);

            IEnumerable<string> roles = await _rolesService.GetUserRolesAsync(waveUser);

            string userRole = member.SelectedRoles.FirstOrDefault();

            if (!string.IsNullOrEmpty(userRole))
            {
                if(await _rolesService.RemoveUserFromRolesAsync(waveUser, roles))
                {
                    await _rolesService.AddUserToRoleAsync(waveUser, userRole);
                }

            }

            return RedirectToAction(nameof(ManageUserRoles));
        }
    }
}
