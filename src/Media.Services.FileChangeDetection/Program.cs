using System.Diagnostics.CodeAnalysis;
using Media.Common.Contracts;
using Media.Common.Models;
using Media.Services.FileChangeDetection.Wrappers;

namespace Media.Services.FileChangeDetection
{
	[ExcludeFromCodeCoverage]
	public class Program
	{
		private static IConfigurationRoot _configurationRoot;

		public static void Main(string[] args)
		{
			_configurationRoot = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

			var configuration = GetFileChangeDetectionConfiguration();

			IHost host = Host.CreateDefaultBuilder(args)
					.ConfigureServices(services =>
					{
						services.AddSingleton(configuration);
						services.AddSingleton<IRabbitMqWrapper, RabbitMqWrapper>();
						services.AddHostedService<Worker>();
					})
					.Build();

			host.Run();
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
	}
}