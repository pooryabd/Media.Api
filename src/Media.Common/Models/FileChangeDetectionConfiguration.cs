// <copyright file="FileChangeDetectionConfiguration.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Models
{
	using System.Diagnostics.CodeAnalysis;
	using Media.Common.Contracts;

	/// <summary>
	/// Class FileChangeDetectionConfiguration
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class FileChangeDetectionConfiguration : IFileChangeDetectionConfiguration
	{
		/// <inheritdoc />
		public string ConnectionString { get; set; }

		/// <inheritdoc />
		public string ExchangeName { get; set; }

		/// <inheritdoc />
		public string SubscriptionName { get; set; }

		/// <inheritdoc />
		public bool AutoCompleteMessage { get; set; }
	}
}
