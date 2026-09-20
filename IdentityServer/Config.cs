using Duende.IdentityServer.Models;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
        new[]
        {
            new ApiScope(
                name: "bookstore.books",
                displayName: "Book CRUD"),

            new ApiScope(
                name: "bookstore.search",
                displayName: "Book Search")
        };

        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
            new ApiResource(
                name: "bookstore-api",
                displayName: "Bookstore API")
            {
                Scopes =
                {
                    "bookstore.books",
                    "bookstore.search"
                }
            }
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
            new Client
            {
                ClientId = "bookstore-crud-client",

                AllowedGrantTypes =
                    GrantTypes.ClientCredentials,

                ClientSecrets =
                {
                    new Secret("bookstore-secret".Sha256())
                },

                AllowedScopes =
                {
                    "bookstore.books"
                }
            },

            new Client
            {
                ClientId = "bookstore-search-client",

                AllowedGrantTypes =
                    GrantTypes.Implicit,

                AllowAccessTokensViaBrowser = true,

                RedirectUris =
                {
                    "https://localhost:5002/swagger/oauth2-redirect.html"
                },

                AllowedScopes =
                {
                    "bookstore.search"
                }
            }
            };
    }
}
