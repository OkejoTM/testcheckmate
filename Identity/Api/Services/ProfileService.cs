using Duende.IdentityServer.Services;
using Duende.IdentityServer.Models;

namespace Identity.Api.Services;

public class ProfileService : IProfileService {
    public async Task GetProfileDataAsync(ProfileDataRequestContext context) {
        context.IssuedClaims = context.Subject.Claims.ToList();
    }

    public async Task IsActiveAsync(IsActiveContext context) {
        context.IsActive = true;
    }
}
