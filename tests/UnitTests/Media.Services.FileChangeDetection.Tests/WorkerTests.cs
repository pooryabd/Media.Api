using System.Reflection;
using AutoFixture.AutoMoq;
using AutoFixture;
using Media.Common.Contracts;
using Media.Common.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Media.Services.FileChangeDetection.Tests
{
	public class WorkerTests
	{
		private readonly IFixture _fixture;

		public WorkerTests()
		{
			_fixture = new Fixture().Customize(new AutoMoqCustomization());
		}

		[Fact]
		public async Task ExecuteAsync_SubscribesToRabbitMqWrapper()
		{
			// Arrange
			var loggerMock = _fixture.Freeze<Mock<ILogger<Worker>>>();
			var rabbitMqWrapperMock = _fixture.Freeze<Mock<IRabbitMqWrapper>>();
			var worker = _fixture.Create<Worker>();
			var cancellationTokenSource = new CancellationTokenSource();

			// Act
			await worker.StartAsync(cancellationTokenSource.Token);
			await worker.StopAsync(cancellationTokenSource.Token);

			// Assert
			rabbitMqWrapperMock.Verify(x => x.Subscribe(It.IsAny<Func<FileMessage, Task>>()), Times.Once);
			rabbitMqWrapperMock.Verify(x => x.DisposeAsync(), Times.Once);
		}
	}
}
