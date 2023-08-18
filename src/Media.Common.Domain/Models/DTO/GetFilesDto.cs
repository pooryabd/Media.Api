// <copyright file="GetFilesDto.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Domain.Models.DTO
{
	/// <summary>
	/// Class GetFilesDto
	/// </summary>
	public class GetFilesDto
	{
		/// <summary>
		/// Gets or sets FileName
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Gets or sets ContentType
		/// </summary>
		public long Length { get; set; }

		/// <summary>
		/// Gets or sets CreationDateTimeUtc
		/// </summary>
		public DateTime CreationDateTimeUtc { get; set; }

		/// <summary>
		/// Gets or sets Name
		/// </summary>
		public string Name { get; set; }
	}
}
