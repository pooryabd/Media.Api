// <copyright file="FileMessage.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Models
{
	using Media.Common.Enumerations;

	/// <summary>
	/// Class FileMessage
	/// </summary>
	public class FileMessage
	{
		/// <summary>
		/// Gets or sets FileName
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// Gets or sets FileOperation
		/// </summary>
		public FileOperation FileOperation { get; set; }
	}
}
