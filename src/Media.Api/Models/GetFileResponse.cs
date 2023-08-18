// <copyright file="GetFileResponse.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Api.Models
{
	using Media.Api.Constants;
	using Media.Common.Domain.Models.DTO;
	using Newtonsoft.Json;

	/// <summary>
	/// Class GetFileResponse
	/// </summary>
	public class GetFileResponse : BaseResponse
	{
		/// <summary>
		/// Gets or sets GetFilesDtos
		/// </summary>
		[JsonProperty(ApiConstants.Files, NullValueHandling = NullValueHandling.Ignore)]
		public List<GetFilesDto> GetFilesDtos { get; set; }
	}
}
