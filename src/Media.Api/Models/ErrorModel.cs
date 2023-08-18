// <copyright file="ErrorModel.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Api.Models
{
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Class ErrorModel
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class ErrorModel
	{
		/// <summary>
		/// Gets or sets ErrorCode
		/// </summary>
		public string ErrorCode { get; set; }

		/// <summary>
		/// Gets or sets ErrorMessage
		/// </summary>
		public string ErrorMessage { get; set; }
	}
}
