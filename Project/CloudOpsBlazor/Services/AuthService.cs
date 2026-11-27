
using CloudOpsBlazor.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Supabase.Gotrue;
using System.Net.Http;
using System.Net.Http.Headers;
using static System.Net.WebRequestMethods;

namespace CloudOpsBlazor.Services
{
    public class AuthService
    {
        private readonly Supabase.Client _supabase;
        private readonly ProtectedSessionStorage _storage;
        private readonly HttpClient _httpClient;

        public AuthService(Supabase.Client supabase, ProtectedSessionStorage storage, HttpClient httpClient)
        {
            _supabase = supabase;
            _storage = storage;
            _httpClient = httpClient;
        }

        public event Action? OnAuthStateChanged;

        private void NotifyAuthStateChanged()
        {
            OnAuthStateChanged?.Invoke();
        }

        public string? UserId { get; private set; } = "Logged Out";

        public async Task<bool> Login(string email, string password)
        {
            try
            {
                var session = await _supabase.Auth.SignIn(email, password);

                Console.WriteLine($"Login user ID: {session?.User?.Id}");
                Console.WriteLine($"Current user ID: {_supabase.Auth.CurrentUser?.Id}");

                UserId = session.User.Id;
                await _storage.SetAsync("UserId", UserId);

                NotifyAuthStateChanged();

                return session?.User != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LoadUser()
        {
            try
            {
                var result = await _storage.GetAsync<string>("UserId");

                UserId = result.Success && !string.IsNullOrWhiteSpace(result.Value)
                    ? result.Value
                    : "Logged Out";

                NotifyAuthStateChanged();

                return UserId != "Logged Out";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LOADUSER ERROR: {ex}");

                UserId = "Logged Out";
                return false;
            }
        }

        public async Task<string> Register(
            string email,
            string password,
            string displayName)
        {
            try
            {
                var options = new SignUpOptions
                {
                    Data = new Dictionary<string, object>
            {
                { "display_name", displayName }
            }
                };

                // Register with Supabase
                var signupResponse = await _supabase.Auth.SignUp(
                    email,
                    password,
                    options
                );

                if (signupResponse?.User == null)
                    return "Registration failed.";

                // Get Supabase UUID
                string supabaseUserId = signupResponse.User.Id;

                // Create profile
                AppUser user = new AppUser
                {
                    UserId = supabaseUserId,
                    Email = email,
                    ProfileName = displayName,
                    ProfilePicture = null,
                    ProfileDescription = null
                };

                // Send profile to your API
                var apiResponse = await _httpClient.PostAsJsonAsync(
                    "Authentication/AddUser",
                    user
                );

                if (!apiResponse.IsSuccessStatusCode)
                {
                    string error = await apiResponse.Content.ReadAsStringAsync();

                    Console.WriteLine($"HTTP Status: {apiResponse.StatusCode}");
                    Console.WriteLine($"API Error: {error}");

                    return $"Profile creation failed: {apiResponse.StatusCode} - {error}";
                }
                else
                {
                    UserId = supabaseUserId;
                    await _storage.SetAsync("UserId", UserId);

                    NotifyAuthStateChanged();
                    return "Registration successful!";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.Message;
            }
        }

        public async Task CheckStorage()
        {
            var result = await _storage.GetAsync<string>("UserId");

            Console.WriteLine("===== PROTECTED STORAGE CHECK =====");
            Console.WriteLine($"Success: {result.Success}");
            Console.WriteLine($"UserId: {result.Value ?? "NULL"}");
            Console.WriteLine("===================================");
        }
    }

    /*public class AuthService
    {
        private readonly Supabase.Client _supabase;
        private readonly ProtectedLocalStorage _storage;
        private readonly HttpClient _http;

        public AuthService(
            Supabase.Client supabase,
            ProtectedLocalStorage storage,
            HttpClient http)
        {
            _supabase = supabase;
            _storage = storage;
            _http = http;
        }

        public string? UserId { get; private set; }

        public async Task<bool> Login(string email, string password)
        {
            try
            {
                var session = await _supabase.Auth.SignIn(email, password);

                if (session?.User == null || string.IsNullOrEmpty(session.AccessToken))
                    return false;

                UserId = session.User.Id;

                await _storage.SetAsync("UserId", UserId);
                await _storage.SetAsync("AccessToken", session.AccessToken);

                SetToken(session.AccessToken);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
                return false;
            }
        }

        public async Task LoadUser()
        {
            var user = await _storage.GetAsync<string>("UserId");
            var token = await _storage.GetAsync<string>("AccessToken");

            UserId = user.Success ? user.Value : null;

            if (token.Success && !string.IsNullOrEmpty(token.Value))
                SetToken(token.Value);
        }

        private void SetToken(string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task Logout()
        {
            await _supabase.Auth.SignOut();

            await _storage.DeleteAsync("UserId");
            await _storage.DeleteAsync("AccessToken");

            _http.DefaultRequestHeaders.Authorization = null;

            UserId = null;
        }

        public async Task<string> Register(
            string email,
            string password,
            string displayName)
        {
            try
            {
                var options = new SignUpOptions
                {
                    Data = new Dictionary<string, object>
                {
                    { "display_name", displayName }
                }
                };

                var response = await _supabase.Auth.SignUp(
                    email, password, options);

                if (response?.User == null)
                    return "Registration failed.";

                return "Registration successful!";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.Message;
            }
        }
    }*/
}
