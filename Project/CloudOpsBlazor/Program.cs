using CloudOpsBlazor.Components;
using CloudOpsBlazor.Services;
using Supabase;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace CloudOpsBlazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(
                builder.Configuration["ApiBaseUrl"]
                ?? "https://localhost:7030/")
            });

            builder.Services.AddScoped<ProtectedSessionStorage>();

            builder.Services.AddScoped(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();

                var url = configuration["Supabase:Url"]!;
                var key = configuration["Supabase:Key"]!;

                return new Supabase.Client(url, key);
            });

            builder.Services.AddScoped<AuthService>();

            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();

        }
    }
}
