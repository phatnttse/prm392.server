using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Interfaces;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Account;
using PRM392.Services.Interfaces;
using PRM392.Utils;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace PRM392.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager, ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<ApplicationUser> HandleLoginAsync(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    throw new ApiException("Please provide a valid username and password", System.Net.HttpStatusCode.BadRequest);

                var user = await _unitOfWork.UserAccountRepository.GetUserByUserNameAsync(username);

                if (user == null)
                    throw new ApiException("Incorrect username or password", System.Net.HttpStatusCode.BadRequest);

                SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, true);

                if (result.IsNotAllowed) throw new ApiException("The specified user account is not allowed to sign in.", System.Net.HttpStatusCode.BadRequest);

                if (!result.Succeeded) throw new ApiException("Incorrect username or password", System.Net.HttpStatusCode.BadRequest);

                return user;

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(ApplicationUser user, IEnumerable<string> scopes)
        {
            try
            {
                var principal = await _signInManager.CreateUserPrincipalAsync(user);

                principal.SetScopes(scopes);

                var identity = principal.Identity as ClaimsIdentity
                    ?? throw new InvalidOperationException("The ClaimsPrincipal's Identity is null.");

                principal.SetDestinations(GetDestinations);

                principal.SetAccessTokenLifetime(TimeSpan.FromDays(7));
                principal.SetRefreshTokenLifetime(TimeSpan.FromDays(14));

                return principal;

            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private IEnumerable<string> GetDestinations(Claim claim)
        {
            if (claim.Subject == null)
                throw new InvalidOperationException("The Claim's Subject is null.");

            switch (claim.Type)
            {
                case Claims.Name:
                    if (claim.Subject.HasScope(Scopes.Profile))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Email:
                    if (claim.Subject.HasScope(Scopes.Email))
                        yield return Destinations.IdentityToken;

                    yield break;


                case CustomClaims.Configuration:
                    if (claim.Subject.HasScope(Scopes.Profile))
                        yield return Destinations.IdentityToken;
                    yield break;

                case Claims.Role:
                    yield return Destinations.AccessToken;

                    if (claim.Subject.HasScope(Scopes.Roles))
                        yield return Destinations.IdentityToken;

                    yield break;

                case CustomClaims.Permission:
                    yield return Destinations.AccessToken;

                    if (claim.Subject.HasScope(Scopes.Roles))
                        yield return Destinations.IdentityToken;
                    yield break;

                // IdentityOptions.ClaimsIdentity.SecurityStampClaimType
                case "AspNet.Identity.SecurityStamp":
                    // Never include the security stamp in the access and identity tokens, as it's a secret value.
                    yield break;

                default:
                    yield return Destinations.AccessToken;
                    yield break;
            }
        }

        public async Task<bool> CanSignInAsync(ApplicationUser user)
        {
            try
            {

                if (!await _signInManager.CanSignInAsync(user))
                {
                    throw new ApiException("The specified user account is not allowed to sign in.", System.Net.HttpStatusCode.BadRequest);
                }

                return true;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _unitOfWork.UserAccountRepository.GetByIdAsync(id);

                if (user == null) throw new ApiException("User does not exist!", System.Net.HttpStatusCode.NotFound);

                return user;

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApplicationResponse> SignUp(CreateAccountDTO body)
        {
            try
            {
                var existingUser = await _unitOfWork.UserAccountRepository.GetUserByUserNameAsync(body.UserName!);

                if (existingUser != null)
                {
                    throw new ApiException("Email has been registered", System.Net.HttpStatusCode.BadRequest);
                }

                ApplicationUser user = new ApplicationUser
                {
                    UserName = body.UserName,
                    FullName = body.FullName
                };

                var result = await _unitOfWork.UserAccountRepository.CreateUserAsync(user, new string[] { Constants.Roles.USER }, body.Password!);

                if (!result.Succeeded)
                {
                    throw new ApiException(string.Join("; ", result.Errors.Select(error => error)), System.Net.HttpStatusCode.BadRequest);
                }

                return new ApplicationResponse
                {
                    Message = "Account registration successful!",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true
                };
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
           
        }
    }
}
