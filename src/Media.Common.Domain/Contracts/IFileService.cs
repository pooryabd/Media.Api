// <copyright file="IFileService.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Domain.Contracts
{
	using Media.Common.Domain.Models.DTO;
	using Microsoft.AspNetCore.Http;

	/// <summary>
	/// Interface IFileService
	/// </summary>
	public interface IFileService
	{
		/// <summary>
		/// Save the file.
		/// </summary>
		/// <param name="formFile">The formFile</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		Task Save(IFormFile formFile);

		/// <summary>
		/// Determines whether a new file should be saved to disk.
		/// </summary>
		/// <param name="newFormFile">The new formFile.</param>
		/// <returns>True if the new file should be saved, false otherwise.</returns>
		Task<bool> ShouldSaveFile(IFormFile newFormFile);

		/// <summary>
		/// Get list of files
		/// </summary>
		/// <returns>List of GetFilesDto</returns>
		Task<List<GetFilesDto>> GetFiles();
	}
}
