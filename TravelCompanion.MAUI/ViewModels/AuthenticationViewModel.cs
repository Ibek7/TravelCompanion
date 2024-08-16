using System.Windows.Input;
using TravelCompanion.SDK.Clients;

namespace TravelCompanion.MAUI.ViewModels
{
    public class AuthenticationViewModel : BaseViewModel
    {
        public AuthenticationViewModel()
        {
            GoogleSignInCommand = new Command(async () => await OnAuthenticate("Google"));
            AppleSignInCommand = new Command(async () => await OnAuthenticate("Apple"));
        }

        public ICommand SignInCommand => new Command(async () =>
        {
            MauiProgram.ApiKey = "cf21c5a9-ec51-4e02-a6dd-9f5bf7b8c703";
            var appUserClient = MauiProgram.ServiceProvider.GetRequiredService<AppUserClient>();
            var user = await appUserClient.GetUser();
            MauiProgram.CurrentUser = user;

            // Navigate to the HomePage upon sign-in
            await Shell.Current.GoToAsync("//HomePage");
        });

        public ICommand GoogleSignInCommand { get; private set; }
        public ICommand AppleSignInCommand { get; private set; }
        
        private readonly string _authenticationUrl = Constants.ApiBaseAddress + "mobileauth/";
        
        async Task OnAuthenticate(string scheme)
        {
            try
            {
                WebAuthenticatorResult r = null;

                var authUrl = new Uri(_authenticationUrl + scheme);
                var callbackUrl = new Uri("myapp://");

                r = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);

                if (r.Properties.TryGetValue("api_key", out var apiKey) && !string.IsNullOrEmpty(apiKey))
                    MauiProgram.ApiKey = apiKey;

                try
                {
                    var appUserClient = MauiProgram.ServiceProvider.GetRequiredService<AppUserClient>();
                    var user = await appUserClient.GetUser();
                    MauiProgram.CurrentUser = user;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                if (string.IsNullOrWhiteSpace(MauiProgram.ApiKey))
                {
                    throw new InvalidOperationException("Failed to get API key.");
                }

                await Shell.Current.GoToAsync("//HomePage");
            }
            catch (OperationCanceledException)
            {
                MauiProgram.ApiKey = null;
                Console.WriteLine("Login canceled.");

                await DisplayAlertAsync("Login canceled.");
            }
            catch (Exception ex)
            {
                MauiProgram.ApiKey = null;
                Console.WriteLine($"Failed: {ex.Message}");

                await DisplayAlertAsync($"Failed: {ex.Message}");
            }
        }
    }
}
