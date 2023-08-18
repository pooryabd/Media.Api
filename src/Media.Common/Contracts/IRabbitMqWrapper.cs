// <copyright file="IRabbitMqWrapper.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Contracts
{
	using Media.Common.Models;

	/// <summary>
	/// Interface IRabbitMqWrapper
	/// </summary>
	public interface IRabbitMqWrapper : IAsyncDisposable
	{
		/// <summary>
		/// Subscribe to queue
		/// </summary>
		/// <param name="handler">The handler</param>
		/// <returns>FileMessage</returns>
		public Task Subscribe(Func<FileMessage, Task> handler);

		/// <summary>
		/// Publish to queue
		/// </summary>
		/// <param name="fileMessage">The fileMessage</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		public Task Publish(FileMessage fileMessage);
	}
}
