using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System;
using ltbdb.Core.Services;
using ltbdb.Extensions;
using ltbdb.ModelBinders;

namespace ltbdb
{
    public class Startup
	{
		public IConfigurationRoot Configuration { get; }

		static public ILoggerFactory Logger { get; private set; }

		public Startup(IHostingEnvironment env, ILoggerFactory logger)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
				.AddEnvironmentVariables();

			Configuration = builder.Build();

			Logger = logger;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// key ring
			var _keyStore = Configuration.GetSection("Settings")["KeyStore"];
			if(!String.IsNullOrEmpty(_keyStore))
			{
				services.AddDataProtection(options => {
					options.ApplicationDiscriminator = "ltbdb";
				}).PersistKeysToFileSystem(new DirectoryInfo(_keyStore));
			}

			// IIS integration
			services.Configure<IISOptions>(options => {
				
			});

			// add logging to DI
			services.AddLogging();

			// add options to DI
			services.AddOptions();
			services.Configure<Settings>(Configuration.GetSection("Settings"));

			// add custom model binders
			services.AddMvc(options => {
				options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());
			}).AddJsonOptions(options => {
				options.SerializerSettings.ContractResolver = new DefaultContractResolver();
			});

			// authorization policies
			services.AddAuthorization(options => {
				options.AddPolicy("AdministratorOnly", policy => {
					policy.RequireRole("Administrator");
				});
			});
			
			// DI
			services.AddAutoMapper();
			services.AddScoped<MongoContext>();
			services.AddTransient<BookService>();
			services.AddTransient<TagService>();
			services.AddTransient<CategoryService>();
			services.AddTransient<ImageService>();
		}
		
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
		{
			logger.AddConsole(Configuration.GetSection("Logging"));

			app.UseStatusCodePagesWithReExecute("/error/{0}");

			if(env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/error/500");
			}

			app.UseStaticFiles();

			app.UseCookieAuthentication(new CookieAuthenticationOptions(){
				AuthenticationScheme = "ltbdb",
				CookieName = "ltbdb",
				LoginPath = new PathString("/login"),
				AccessDeniedPath = new PathString("/"),
				AutomaticAuthenticate = true,
				AutomaticChallenge = true
			});

			app.AddRoutes();
		}
	}
}
