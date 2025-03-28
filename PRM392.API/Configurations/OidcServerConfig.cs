using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace PRM392.API.Configurations
{
    public static class OidcServerConfig
    {
        public const string ServerName = "PRM392 API";
        public const string PRM392ClientID = "prm392_spa";
        public const string SwaggerClientID = "swagger_ui";

        public static async Task RegisterClientApplicationsAsync(IServiceProvider provider)
        {
            var manager = provider.GetRequiredService<IOpenIddictApplicationManager>();

            //Client
            if (await manager.FindByClientIdAsync(PRM392ClientID) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = PRM392ClientID,
                    ClientType = ClientTypes.Public,
                    DisplayName = "PRM392 SPA",
                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.Password,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Roles
                    }
                });
            }

            // Swagger UI Client
            if (await manager.FindByClientIdAsync(SwaggerClientID) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = SwaggerClientID,
                    ClientType = ClientTypes.Public,
                    DisplayName = "Swagger UI",
                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.Password
                    }
                });
            }
        }
    }
}
