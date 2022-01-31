using Microsoft.AspNetCore.Identity;
using Work_Wave.Models;

namespace Work_Wave.Services.Interfaces
{
    public interface ITRolesService
    {
        public Task<bool> IsUserInRoleAsync(WaveUser user, string roleName);
        public Task<IEnumerable<string>> GetUserRolesAsync(WaveUser user);
        public Task<bool> AddUserToRoleAsync(WaveUser user, string roleName);
        public Task<bool> RemoveUserFromRoleAsync(WaveUser user, string roleName);
        public Task<bool> RemoveUserFromRolesAsync(WaveUser user, IEnumerable<string> roles);
        public Task<List<WaveUser>> GetUsersInRoleAsync(string roleName);
        public Task<List<WaveUser>> GetUsersNotInRoleAsync(string roleName);
        public Task<string> GetRoleNameByIdAsync(string roleId);
        public Task<List<IdentityRole>> GetRolesAync();
        public Task<string> GetRoleIdByNameAsync(string role);
    }
}
