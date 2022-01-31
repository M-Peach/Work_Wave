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

        public async Task<IEnumerable<string>> GetUserRolesAsync(WaveUser user)
        {
            IEnumerable<string> result = await _userManager.GetRolesAsync(user);
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

