// <copyright file="BaseResponse.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Api.Models
{
	using System.Diagnostics.CodeAnalysis;
	using Media.Api.Contracts;

	/// <summary>
	/// Class BaseResponse
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class BaseResponse : IBaseResponse
	{
		/// <inheritdoc />
		public List<ErrorModel> Errors { get; set; }
	}
}
