// <copyright file="IBaseResponse.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Api.Contracts
{
	using Media.Api.Constants;
	using Media.Api.Models;
	using Newtonsoft.Json;

	/// <summary>
	/// Interface IBaseResponse
	/// </summary>
	public interface IBaseResponse
	{
		/// <summary>
		/// Gets or sets Errors
		/// </summary>
		[JsonProperty(PropertyName = ApiConstants.Errors, NullValueHandling = NullValueHandling.Ignore)]
		public List<ErrorModel> Errors { get; set; }
	}
}
