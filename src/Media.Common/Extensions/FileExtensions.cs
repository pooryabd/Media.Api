// <copyright file="FileExtensions.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Extensions
{
	/// <summary>
	/// Class FileExtensions
	/// </summary>
	public static class FileExtensions
	{
		/// <summary>
		/// Converts a value in bytes to megabytes.
		/// </summary>
		/// <param name="bytes">The number of bytes to convert.</param>
		/// <returns>The equivalent value in megabytes.</returns>
		public static decimal ToMegabytes(this long bytes)
		{
			if (bytes <= 0)
			{
				return 0;
			}

			return (decimal)bytes / 1000000;
		}
	}
}
