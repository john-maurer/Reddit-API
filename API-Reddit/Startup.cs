using RedditAuthorizationFilter.Interfaces;
using RedditAuthorizationFilter.Services;
using RedditAuthorizationFilter;
using RedditClient;
using API_Reddit.Services;

namespace API_Reddit
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //Load settings from configuration
            Settings.OAUTHUSER = Configuration.GetSection("Settings")["OAUTHUSER"]!;
            Settings.OAUTHPASSWORD = Configuration.GetSection("Settings")["OAUTHPASSWORD"]!;
            Settings.OAUTHURL = Configuration.GetSection("Settings")["OAUTHURL"]!;
            Settings.GRANTTYPE = Configuration.GetSection("Settings")["GRANTTYPE"]!;
            Settings.SCOPE = Configuration.GetSection("Settings")["SCOPE"]!;
            Settings.RETRIES = Configuration.GetSection("Settings")["RETRIES"]!;
            Settings.RETRYSLEEP = Configuration.GetSection("Settings")["RETRYSLEEP"]!;
            Settings.TOKENLIFETIME = Configuration.GetSection("Settings")["TOKENLIFETIME"]!;

            //Register HttpClient
            services.AddHttpClient();

            //Register other services
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddHostedService<RedditBackgroundService>();
            services.AddSingleton<IDistributedCacheHelper, DistributedCacheHelper>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<ISendMessage, RedItClient>();

            //Register the action filter
            services.AddSingleton<AuthenticationFilter<ITokenService>>(ServiceProvider =>
            {
                var tokenService = ServiceProvider.GetRequiredService<ITokenService>();
                var cacheHelper = ServiceProvider.GetRequiredService<IDistributedCacheHelper>();
                var logger = ServiceProvider.GetRequiredService<ILogger<AuthenticationFilter<ITokenService>>>();

                return new AuthenticationFilter<ITokenService>(tokenService, cacheHelper, logger);
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}
