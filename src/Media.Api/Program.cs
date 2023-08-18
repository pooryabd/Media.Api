// <copyright file="Program.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Api
{
	using System.Diagnostics.CodeAnalysis;
	using LightInject;
	using Media.Common.Contracts;
	using Media.Common.Domain.Configuration;
	using Media.Common.Domain.Constants;
	using Media.Common.Domain.Contracts;
	using Media.Common.Domain.Services.File;
	using Media.Common.Models;
	using Media.Services.FileChangeDetection.Wrappers;
	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.AspNetCore.Http.Features;
	using Microsoft.IdentityModel.Tokens;
	using Newtonsoft.Json.Converters;

	/// <summary>
	/// Program class
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class Program
	{
		private static IConfigurationRoot _configurationRoot;

		/// <summary>
		/// main class
		/// </summary>
		/// <param name="args">The args</param>
		public static void Main(string[] args)
		{
			_configurationRoot = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

			var builder = WebApplication.CreateBuilder(args);

			builder.WebHost.ConfigureKestrel(serverOptions =>
			{
				// Set the maximum request body size to 1GB
				serverOptions.Limits.MaxRequestBodySize = 1073741824;
			});

			builder.Host.UseLightInject(services =>
			{
				var mediaApiConfiguration = GetMediaApiConfiguration();
				var changeDetectionConfiguration = GetFileChangeDetectionConfiguration();
				var diskFileService = new DiskFileService(mediaApiConfiguration);
				services.RegisterInstance(mediaApiConfiguration);
				services.RegisterInstance(changeDetectionConfiguration);
				services.RegisterInstance<IFileService>(diskFileService, DependencyRegistrationName.DiskFileSaverDependencyName);
				services.Register<IRabbitMqWrapper, RabbitMqWrapper>(lifetime: new PerContainerLifetime());
				(services as LightInject.IServiceContainer).RegisterApplicationServices();
			});

			builder.Services.Configure<FormOptions>(options =>
			{
				options.MultipartBodyLengthLimit = 1073741824;
			});

			builder.Services.AddLogging();
			builder.Services.AddScoped<ExceptionFilter>();
			builder.Services.AddControllers(op =>
				{
					op.Filters.Add<ExceptionFilter>();
				})
				.AddControllersAsServices()
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.Converters.Add(new StringEnumConverter());
				});

			AddAuthentication(builder.Services);

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}

		private static IMediaApiConfiguration GetMediaApiConfiguration()
		{
			return new MediaApiConfiguration()
			{
				MaxFileSizeInMB = int.Parse(_configurationRoot["MediaApi:MaxFileSizeInMB"]),
				PathToSaveFiles = _configurationRoot["MediaApi:PathToSaveFiles"]
			};
		}

		private static IFileChangeDetectionConfiguration GetFileChangeDetectionConfiguration()
		{
			return new FileChangeDetectionConfiguration()
			{
				AutoCompleteMessage = bool.Parse(_configurationRoot["SubscriptionConfiguration:AutoCompleteMessage"]),
				ConnectionString = _configurationRoot["SubscriptionConfiguration:ConnectionString"],
				ExchangeName = _configurationRoot["SubscriptionConfiguration:ExchangeName"],
				SubscriptionName = _configurationRoot["SubscriptionConfiguration:SubscriptionName"]
			};
		}

		private static void AddAuthentication(IServiceCollection services)
		{
			// Add Authentication services
			services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(x =>
				{
					x.RequireHttpsMetadata = false;
					x.SaveToken = true;
					x.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = false,
						ValidateAudience = false,
						ClockSkew = TimeSpan.Zero
					};
				});
		}
	}
}