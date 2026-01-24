using System.Security.Claims;
using be_lspmpi.Models;

namespace be_lspmpi.Services
{
    public class ClaimService(IHttpContextAccessor httpContextAccessor) : IClaimService
    {
        public string? GetUserId()
        {
            return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public string? GetUserRole()
        {
            return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        }

        public string? GetUsername()
        {
            return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
        }

        public string? GetEmail()
        {
            return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
        }

        public string GetProfileName()
        {
            return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.GivenName)?.Value;
        }

        public ClaimUser GetClaimUser()
        {
            return new ClaimUser
            {
                Id = Guid.Parse(GetUserId()!),
                Username = GetUsername(),
                Email = GetEmail(),
                RoleId = Convert.ToInt32(GetUserRole()),
                ProfileName = GetProfileName()
            };
        }

        public bool IsInRole(int[] roleId)
        {
            var userRole = GetUserRole();
            if (userRole == null) return false;
            return roleId.Contains(Convert.ToInt32(userRole));
        }
    }
}