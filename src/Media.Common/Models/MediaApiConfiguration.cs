// <copyright file="MediaApiConfiguration.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Models
{
	using Media.Common.Contracts;

	/// <summary>
	/// Class MediaApiConfiguration
	/// </summary>
	public class MediaApiConfiguration : IMediaApiConfiguration
	{
		/// <inheritdoc />
		public string PathToSaveFiles { get; set; }

		/// <inheritdoc />
		public long MaxFileSizeInMB { get; set; }
	}
}
