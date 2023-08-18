using System.Diagnostics.CodeAnalysis;
using Media.Common.Contracts;
using Media.Common.Models;
using Media.Services.FileChangeDetection.Wrappers;
using System.Threading;

namespace Media.Services.FileChangeDetection
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IRabbitMqWrapper _rabbitMqWrapper;

		public Worker(
			ILogger<Worker> logger,
			IRabbitMqWrapper rabbitMqWrapper)
		{
			_logger = logger;
			_rabbitMqWrapper = rabbitMqWrapper;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await _rabbitMqWrapper.Subscribe(MessageHandler);

			stoppingToken.Register(async () =>
			{
				await _rabbitMqWrapper.DisposeAsync();
			});
		}

		[ExcludeFromCodeCoverage]
		private async Task MessageHandler(FileMessage fileMessage)
		{
			_logger.LogInformation("Change has been recieved for file {0}, the operation: {1}", fileMessage.FileName, fileMessage.FileOperation);
			await Task.CompletedTask;
		}
	}
}