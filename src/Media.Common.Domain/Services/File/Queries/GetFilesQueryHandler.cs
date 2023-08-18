// <copyright file="GetFilesQueryHandler.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Domain.Services.File.Queries
{
	using LightInject;
	using Media.Common.Contracts;
	using Media.Common.Domain.Constants;
	using Media.Common.Domain.Contracts;
	using Media.Common.Domain.Models.DTO;
	using Media.Common.Enumerations;
	using Media.Common.Models;
	using Media.Services.FileChangeDetection.Wrappers;
	using MediatR;

	/// <summary>
	/// Class GetFilesQueryHandler
	/// </summary>
	public class GetFilesQueryHandler : IRequestHandler<GetFilesQuery, List<GetFilesDto>>
	{
		private readonly IFileService _diskFileService;
		private readonly IRabbitMqWrapper _rabbitMqWrapper;

		/// <summary>
		/// Initializes a new instance of the <see cref="GetFilesQueryHandler"/> class.
		/// </summary>
		/// <param name="diskFileService">The diskFileService</param>
		/// <param name="rabbitMqWrapper">The rabbitMqWrapper</param>
		public GetFilesQueryHandler(
			[Inject(DependencyRegistrationName.DiskFileSaverDependencyName)] IFileService diskFileService,
			IRabbitMqWrapper rabbitMqWrapper)
		{
			_diskFileService = diskFileService;
			_rabbitMqWrapper = rabbitMqWrapper;
		}

		/// <inheritdoc />
		public async Task<List<GetFilesDto>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
		{
			var files = await _diskFileService.GetFiles();

			foreach (var file in files)
			{
				await SendChangeDetectionMessage(file);
			}

			return files;
		}

		private async Task SendChangeDetectionMessage(GetFilesDto getFilesDto)
		{
			var brokerMessage = new FileMessage()
			{
				FileName = getFilesDto.FileName,
				FileOperation = FileOperation.Read
			};

			await _rabbitMqWrapper.Publish(brokerMessage);
		}
	}
}
