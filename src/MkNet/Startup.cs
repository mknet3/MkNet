using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace mknet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder(AzureADDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();
                })
                .AddAuthentication()
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(configure =>
            {
                configure.MapGet("{**fileName}", async (context) =>
                {
                    string fileName = (string) context.GetRouteValue("fileName");
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        fileName = "index.html";
                    }

                    await SendFileFromDocsAsync(context, fileName);
                }).RequireAuthorization();
            });
        }

        private async static Task SendFileFromDocsAsync(HttpContext context, string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(fileName, out string contentType);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "docs", fileName);
            context.Response.ContentType = contentType;
            await context.Response.SendFileAsync(filePath);
        }
    }
}
