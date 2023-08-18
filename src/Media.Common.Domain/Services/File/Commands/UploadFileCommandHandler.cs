// <copyright file="UploadFileCommandHandler.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Domain.Services.File.Commands
{
	using LightInject;
	using Media.Common.Contracts;
	using Media.Common.Domain.Constants;
	using Contracts;
	using Media.Common.Enumerations;
	using Media.Common.Models;
	using MediatR;
	using Microsoft.AspNetCore.Http;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Class UploadFileCommandHandler
	/// </summary>
	public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, bool>
	{
		private readonly IFileService _diskFileSaver;
		private readonly ILogger<UploadFileCommandHandler> _logger;
		private readonly IRabbitMqWrapper _rabbitMqWrapper;

		/// <summary>
		/// Initializes a new instance of the <see cref="UploadFileCommandHandler"/> class.
		/// </summary>
		/// <param name="diskFileSaver">The diskFileSaver</param>
		/// <param name="logger">The logger</param>
		/// <param name="rabbitMqWrapper">The rabbitMqWrapper</param>
		public UploadFileCommandHandler(
			ILogger<UploadFileCommandHandler> logger,
			[Inject(DependencyRegistrationName.DiskFileSaverDependencyName)] IFileService diskFileSaver,
			IRabbitMqWrapper rabbitMqWrapper)
		{
			_diskFileSaver = diskFileSaver;
			_logger = logger;
			_rabbitMqWrapper = rabbitMqWrapper;
		}

		/// <inheritdoc />
		public async Task<bool> Handle(UploadFileCommand request, CancellationToken cancellationToken)
		{
			foreach (var requestFormFile in request.FormFiles)
			{
				if (await _diskFileSaver.ShouldSaveFile(requestFormFile))
				{
					await _diskFileSaver.Save(requestFormFile);

					await SendChangeDetectionMessage(requestFormFile);
				}
				else
				{
					_logger.LogDebug(string.Format(LoggingConstants.FileAlreadyExistsLogMessage, requestFormFile.FileName));
				}
			}

			return true;
		}

		private async Task SendChangeDetectionMessage(IFormFile requestFormFile)
		{
			var brokerMessage = new FileMessage()
			{
				FileName = requestFormFile.FileName,
				FileOperation = FileOperation.Add
			};

			await _rabbitMqWrapper.Publish(brokerMessage);
		}
	}
}
