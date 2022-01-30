using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Work_Wave.Data;
using Work_Wave.Models;

namespace Work_Wave.Services
{
    public class RolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly UserManager<WaveUser> _UserManager;

        public RolesService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<WaveUser> userManager)
        {
            _context = context;
            _RoleManager = roleManager;
            _UserManager = userManager;
        }

        public async Task<bool> AddUserToRoleAsync(WaveUser user, string roleName)
        {
            bool result = (await _UserManager.AddToRoleAsync(user, roleName)).Succeeded;

            return result;
        }

        public async Task<string> GetRoleNameByIdAsync(string roleId)
        {
            IdentityRole role = _context.Roles.Find(roleId);

            string result = await _RoleManager.GetRoleNameAsync(role);

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

        public async Task<IEnumerable<string>> GetUserRolesAsync(WaveUser user)
        {
            IEnumerable<string> result = await _UserManager.GetRolesAsync(user);

            return result;
        }

        public async Task<List<WaveUser>> GetUsersInRoleAsync(string roleName)
        {
            List<WaveUser> users = (await _UserManager.GetUsersInRoleAsync(roleName)).ToList();

            List<WaveUser> result = users.ToList();

            return result;

        }

        public async Task<List<WaveUser>> GetUsersNotInRoleAsync(string roleName)
        {
            List<string> userIds = (await _UserManager.GetUsersInRoleAsync(roleName)).Select(u => u.Id).ToList();

            List<WaveUser> roleUsers = _context.Users.Where(u => !userIds.Contains(u.Id)).ToList();

            List<WaveUser> result = roleUsers;

            return result;
        }

        public async Task<bool> IsUserInRoleAsync(WaveUser user, string roleName)
        {
            bool result = await _UserManager.IsInRoleAsync(user, roleName);

            return result;
        }

        public async Task<bool> RemoveUserFromRoleAsync(WaveUser user, string roleName)
        {
            bool result = (await _UserManager.RemoveFromRoleAsync(user, roleName)).Succeeded;

            return result;
        }

        public async Task<bool> RemoveUserFromRoleAsync(WaveUser user, IEnumerable<string> roles)
        {
            bool result = (await _UserManager.RemoveFromRolesAsync(user, roles)).Succeeded;

            return result;
        }
    }
}

