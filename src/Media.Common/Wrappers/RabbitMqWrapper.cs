// <copyright file="RabbitMqWrapper.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Services.FileChangeDetection.Wrappers
{
	using System.Diagnostics.CodeAnalysis;
	using System.Text;
	using Media.Common.Contracts;
	using Media.Common.Models;
	using Newtonsoft.Json;
	using RabbitMQ.Client;
	using RabbitMQ.Client.Events;

	/// <summary>
	/// Class RabbitMqWrapper
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class RabbitMqWrapper : IRabbitMqWrapper
	{
		private readonly IFileChangeDetectionConfiguration _fileChangeDetectionConfiguration;
		private readonly IConnection _connection;
		private readonly IModel _channel;

		/// <summary>
		/// Initializes a new instance of the <see cref="RabbitMqWrapper"/> class.
		/// </summary>
		/// <param name="fileChangeDetectionConfiguration">The fileChangeDetectionConfiguration</param>
		public RabbitMqWrapper(IFileChangeDetectionConfiguration fileChangeDetectionConfiguration)
		{
			_fileChangeDetectionConfiguration = fileChangeDetectionConfiguration;

			var factory = new ConnectionFactory
			{
				HostName = _fileChangeDetectionConfiguration.ConnectionString,
				DispatchConsumersAsync = true
			};

			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();

			_channel.QueueDeclare(
				queue: _fileChangeDetectionConfiguration.ExchangeName,
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null);
		}

		/// <summary>
		/// Subscribe
		/// </summary>
		/// <param name="handler">The handler</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		public async Task Subscribe(Func<FileMessage, Task> handler)
		{
			var consumer = new AsyncEventingBasicConsumer(_channel);
			consumer.Received += async (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var messageStr = Encoding.UTF8.GetString(body);

				var message = JsonConvert.DeserializeObject<FileMessage>(messageStr);

				await handler(message);
			};

			_channel.BasicConsume(
				queue: _fileChangeDetectionConfiguration.ExchangeName,
				autoAck: true,
				consumer: consumer);

			await Task.CompletedTask;
		}

		/// <summary>
		/// Publish
		/// </summary>
		/// <param name="fileMessage">The fileMessage</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		public async Task Publish(FileMessage fileMessage)
		{
			var messageStr = JsonConvert.SerializeObject(fileMessage);

			var body = Encoding.UTF8.GetBytes(messageStr);

			_channel.BasicPublish(
				exchange: string.Empty,
				routingKey: _fileChangeDetectionConfiguration.ExchangeName,
				basicProperties: null,
				body: body);

			await Task.CompletedTask;
		}

		/// <inheritdoc />
		public ValueTask DisposeAsync()
		{
			_channel.Close();
			_connection.Close();
			return ValueTask.CompletedTask;
		}
	}
}
