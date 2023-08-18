// <copyright file="IFileChangeDetectionConfiguration.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Contracts
{
	/// <summary>
	/// Interface IFileChangeDetectionConfiguration
	/// </summary>
	public interface IFileChangeDetectionConfiguration
	{
		/// <summary>
		/// Gets or sets ConnectionString
		/// </summary>
		string ConnectionString { get; set; }

		/// <summary>
		/// Gets or sets ExchangeName
		/// </summary>
		string ExchangeName { get; set; }

		/// <summary>
		/// Gets or sets SubscriptionName
		/// </summary>
		string SubscriptionName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether AutoCompleteMessage
		/// </summary>
		bool AutoCompleteMessage { get; set; }
	}
}
