using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading;

namespace Persona
{
    class Program
    {


        private static IConfigurationRoot configuration;
        private static IPublicClientApplication app;
        static void Main(string[] args)
        {
            Console.WriteLine("App_Start!");

            configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .Build();

            Task t = OAuthSSO();
            t.Wait();
            Console.WriteLine("App_Complete!");
            Console.ReadLine();
        }

        private static async Task OAuthSSO()
        {
            string[] scopes = new string[] { "user.read" };

            app = PublicClientApplicationBuilder.Create(configuration["Azure:ClientID"])
                            //.WithRedirectUri(configuration["Azure:Redirect"])
                            .WithAuthority(AzureCloudInstance.AzurePublic, configuration["Azure:TenantID"])
                            .Build();


            var accounts = await app.GetAccountsAsync();
            AuthenticationResult result = null;
            if (accounts.Any())
            {
                result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();
            }
            else
            {
                try
                {
                    result = await app.AcquireTokenByIntegratedWindowsAuth(scopes)
                       .ExecuteAsync(CancellationToken.None);
                }
                catch (MsalUiRequiredException ex)
                {
                    Console.WriteLine(string.Concat("AAD error: ", ex.Message, " Code: ", ex.ErrorCode));
                }
                catch (MsalServiceException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (MsalClientException ex)
                {
                    Console.WriteLine(ex.ErrorCode);

                    // ErrorCode kann hier nachgeschlagen werden : https://docs.microsoft.com/en-us/dotnet/api/microsoft.identity.client.msalerror?view=azure-dotnet

                    Console.WriteLine(ex.Message);

                    if (ex.InnerException != null && ex.InnerException is MsalServiceException)
                    {
                        Console.WriteLine(((MsalServiceException)ex.InnerException).Message);
                        Console.WriteLine(((MsalServiceException)ex.InnerException).ResponseBody);
                    }


                }
            }
        }

    }
}
