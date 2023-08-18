// <copyright file="IMediaApiConfiguration.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Contracts
{
	/// <summary>
	/// Interface IMediaApiConfiguration
	/// </summary>
	public interface IMediaApiConfiguration
	{
		/// <summary>
		/// Gets or sets PathToSaveFiles
		/// </summary>
		string PathToSaveFiles { get; set; }

		/// <summary>
		/// Gets or sets MaxFileSizeInMB
		/// </summary>
		long MaxFileSizeInMB { get; set; }
	}
}
