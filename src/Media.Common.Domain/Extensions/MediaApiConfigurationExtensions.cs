// <copyright file="MediaApiConfigurationExtensions.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Domain.Extensions
{
	using Media.Common.Contracts;

	/// <summary>
	/// Class MediaApiConfigurationExtensions
	/// </summary>
	public static class MediaApiConfigurationExtensions
	{
		/// <summary>
		/// GetFullFilePath
		/// </summary>
		/// <param name="configuration">The configuration</param>
		/// <param name="fileName">The fileName</param>
		/// <returns>Full file path to save on the disk</returns>
		public static string GetFullFilePath(this IMediaApiConfiguration configuration, string fileName)
		{
			return Path.Combine(configuration.PathToSaveFiles, fileName);
		}
	}
}
