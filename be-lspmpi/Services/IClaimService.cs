using be_lspmpi.Models;

namespace be_lspmpi.Services
{
    public interface IClaimService
    {
        string? GetUserId();
        string? GetUserRole();
        string? GetUsername();
        string? GetEmail();
        string? GetProfileName();
        ClaimUser GetClaimUser();
        bool IsInRole(int[] roleId);
    }
}