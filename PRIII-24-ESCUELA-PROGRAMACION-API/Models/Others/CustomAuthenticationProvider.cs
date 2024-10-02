using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models.Others
{
    public class CustomAuthenticationProvider : IAuthenticationProvider
    {
        private readonly IConfidentialClientApplication _clientApp;

        public CustomAuthenticationProvider(IConfidentialClientApplication clientApp)
        {
            _clientApp = clientApp;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var authResult = await _clientApp.AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" }).ExecuteAsync();
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult.AccessToken);
        }
    }
}
