// <copyright file="DiskFileService.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Domain.Services.File
{
	using System.Collections;
	using System.Diagnostics.CodeAnalysis;
	using System.Security.Cryptography;
	using Media.Common.Contracts;
	using Media.Common.Domain.Contracts;
	using Media.Common.Domain.Extensions;
	using Media.Common.Domain.Models.DTO;
	using Microsoft.AspNetCore.Http;

	/// <summary>
	/// Class DiskFileSaver
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class DiskFileService : IFileService
	{
		private readonly IMediaApiConfiguration _mediaApiConfiguration;

		/// <summary>
		/// Initializes a new instance of the <see cref="DiskFileService"/> class.
		/// </summary>
		/// <param name="mediaApiConfiguration">The mediaApiConfiguration</param>
		public DiskFileService(IMediaApiConfiguration mediaApiConfiguration)
		{
			_mediaApiConfiguration = mediaApiConfiguration;
		}

		/// <summary>
		/// Save the file to the disk.
		/// </summary>
		/// <param name="formFile">The formFile</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		public async Task Save(IFormFile formFile)
		{
			Directory.CreateDirectory(_mediaApiConfiguration.PathToSaveFiles);

			var filePath = _mediaApiConfiguration.GetFullFilePath(formFile.FileName);

			await using var fileStream = new FileStream(filePath, FileMode.Create);
			await formFile.CopyToAsync(fileStream);
		}

		/// <inheritdoc />
		public async Task<bool> ShouldSaveFile(IFormFile newFormFile)
		{
			string filePath = _mediaApiConfiguration.GetFullFilePath(newFormFile.FileName);

			using var memoryStream = new MemoryStream();
			await newFormFile.CopyToAsync(memoryStream);
			var newFileContents = memoryStream.ToArray();

			if (System.IO.File.Exists(filePath))
			{
				var existingFileContents = await System.IO.File.ReadAllBytesAsync(filePath);

				using var sha256 = SHA256.Create();
				var existingFileHash = sha256.ComputeHash(existingFileContents);
				var newFileHash = sha256.ComputeHash(newFileContents);

				if (StructuralComparisons.StructuralEqualityComparer.Equals(existingFileHash, newFileHash))
				{
					return false;
				}
			}

			return true;
		}

		/// <inheritdoc/>
		public async Task<List<GetFilesDto>> GetFiles()
		{
			var files = Directory.GetFiles(_mediaApiConfiguration.PathToSaveFiles)
				.Select(filePath => new FileInfo(filePath))
				.Select(fileInfo => new GetFilesDto
				{
					FileName = fileInfo.Name,
					Length = fileInfo.Length,
					CreationDateTimeUtc = fileInfo.CreationTimeUtc,
					Name = Path.GetFileNameWithoutExtension(fileInfo.Name)
				})
				.ToList();

			return await Task.FromResult(files);
		}
	}
}
