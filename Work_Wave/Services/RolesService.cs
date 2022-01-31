using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Work_Wave.Data;
using Work_Wave.Models;
using Work_Wave.Services.Interfaces;
using Work_Wave.Models.Enums;

namespace Work_Wave.Services
{
    public class RolesService : ITRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<WaveUser> _userManager;

        public RolesService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<WaveUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> AddUserToRoleAsync(WaveUser user, string roleName)
        {
            bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;
            return result;
        }

        public async Task<string> GetRoleNameByIdAsync(string roleId)
        {
            IdentityRole role = _context.Roles.Find(roleId);
            string result = await _roleManager.GetRoleNameAsync(role);
            return result;
        }

        public async Task<string> GetRoleIdByNameAsync(string role)
        {
            IdentityRole id = _context.Roles.Find(role);
            string result = await _roleManager.GetRoleIdAsync(id);
            return result;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(WaveUser user)
        {
            IEnumerable<string> result = await _userManager.GetRolesAsync(user);
            return result;
        }

        public async Task<List<WaveUser>> GetUsersInRoleAsync(string roleName)
        {
            List<WaveUser> users = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
            List<WaveUser> result = users.ToList();
            return result;
        }

        public async Task<List<WaveUser>> GetUsersNotInRoleAsync(string roleName)
        {
            List<string> userIds = (await _userManager.GetUsersInRoleAsync(roleName)).Select(u => u.Id).ToList();
            List<WaveUser> roleUsers = _context.Users.Where(u => !userIds.Contains(u.Id)).ToList();

            List<WaveUser> result = roleUsers.ToList();
            return result;

        }

        public async Task<bool> IsUserInRoleAsync(WaveUser user, string roleName)
        {
            bool result = await _userManager.IsInRoleAsync(user, roleName);
            return result;
        }

        public async Task<bool> RemoveUserFromRoleAsync(WaveUser user, string roleName)
        {
            bool result = (await _userManager.RemoveFromRoleAsync(user, roleName)).Succeeded;
            return result;
        }

        public async Task<bool> RemoveUserFromRolesAsync(WaveUser user, IEnumerable<string> roles)
        {
            bool result = (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;
            return result;
        }

        public async Task<List<IdentityRole>> GetRolesAync()
        {
            try
            {
                List<IdentityRole> result = new();

                
                result = await _context.Roles.ToListAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

