using Novell.Directory.Ldap;
using Duende.IdentityServer.Validation;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Identity.Api.Repositories;

namespace Identity.Api.Validators;

public class LdapResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator {
    private readonly ILdapRepository _ldapRepository;
    private readonly ILogger<LdapResourceOwnerPasswordValidator> _logger;

    public LdapResourceOwnerPasswordValidator(ILdapRepository ldapRepository, ILogger<LdapResourceOwnerPasswordValidator> logger) {
        _ldapRepository = ldapRepository;
        _logger = logger;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context) {
        var user = await _ldapRepository.AuthenticateUserAsync(context.UserName, context.Password);

        if (user == null) {
            return;
        }

        context.Result = new GrantValidationResult(
            subject: user.UserName,
            authenticationMethod: "custom",
            claims: new Claim[] {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            }
        );
    }
}
