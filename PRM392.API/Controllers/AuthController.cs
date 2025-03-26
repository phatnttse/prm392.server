using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using PRM392.Services.DTOs.Account;
using PRM392.Services.Interfaces;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace PRM392.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPost("~/connect/token")]
        [Produces("application/json")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest()
                ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            if (request.IsPasswordGrantType())
            {
                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                    throw new InvalidOperationException("The username and password cannot be null or empty.");

                var user = await _authService.HandleLoginAsync(request.Username, request.Password);

                var principal = await _authService.CreateClaimsPrincipalAsync(user, request.GetScopes());

                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            else if (request.IsRefreshTokenGrantType())
            {
                var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                var userId = result?.Principal?.GetClaim(Claims.Subject);

                var user = userId != null ? await _authService.GetUserByIdAsync(userId) : null;

                await _authService.CanSignInAsync(user!);

                var scopes = request.GetScopes();
                if (scopes.Length == 0 && result?.Principal != null)
                    scopes = result.Principal.GetScopes();

                // Recreate the claims principal in case they changed since the refresh token was issued.
                var principal = await _authService.CreateClaimsPrincipalAsync(user!, scopes);

                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            //else if (request.GrantType == Constants.GrantTypes.ASSERTION)
            //{
            //    //var assertion = request.Assertion;

            //    //var provider = request.IdentityProvider;

            //    //if (string.IsNullOrEmpty(assertion) || string.IsNullOrEmpty(provider))
            //    //    throw new InvalidOperationException("The assertion and provider cannot be null or empty.");

            //    //var user = await _authService.HandleSocialLoginAsync(assertion, provider);

            //    //var principal = await _authService.CreateClaimsPrincipalAsync(user, request.GetScopes());

            //    //return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            //}

            throw new InvalidOperationException($"The specified grant type \"{request.GrantType}\" is not supported.");
        }

        /// <summary>
        /// Đăng ký
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("signup")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SignUp([FromBody] CreateAccountDTO body)
        {
            return Ok(await _authService.SignUp(body));
        }
    }
}
