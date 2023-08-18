using AutoFixture;
using Media.Common.Contracts;
using Media.Common.Domain.Constants;
using Media.Common.Domain.Contracts;
using Media.Common.Domain.Services.File.Commands;
using Media.Common.Enumerations;
using Media.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Media.Common.Domain.Tests.Services.File.Commands
{
	public class UploadFileCommandHandlerTests
	{
		private readonly IFixture _fixture;

		public UploadFileCommandHandlerTests()
		{
			_fixture = new Fixture();
		}

		[Fact]
		public async Task Handle_WhenFileShouldBeSaved_ShouldSaveFile()
		{
			// Arrange
			var sutFactory = new SutFactory();
			var sut = sutFactory.Create();

			var formFile = Mock.Of<IFormFile>();
			var formFiles = new List<IFormFile> { formFile };

			var request = _fixture.Build<UploadFileCommand>()
				.With(x => x.FormFiles, formFiles)
				.Create();

			sutFactory.FileSaver.Setup(x => x.ShouldSaveFile(formFile)).ReturnsAsync(true);

			// Act
			await sut.Handle(request, CancellationToken.None);

			// Assert
			sutFactory.FileSaver.Verify(x => x.Save(formFile), Times.Once);

			sutFactory.Logger.Verify(x => x.Log(
				LogLevel.Debug,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, t) => v.ToString() == string.Format(LoggingConstants.FileAlreadyExistsLogMessage, formFile.FileName)),
				It.IsAny<System.Exception>(),
				It.Is<Func<It.IsAnyType, System.Exception, string>>((v, t) => true)), Times.Never);
		}

		[Fact]
		public async Task Handle_WhenFileShouldBeSaved_ShouldSendMessageToChangeDetectionQueue()
		{
			// Arrange
			var sutFactory = new SutFactory();
			var sut = sutFactory.Create();

			var formFile = Mock.Of<IFormFile>();
			var formFiles = new List<IFormFile> { formFile };

			var request = _fixture.Build<UploadFileCommand>()
				.With(x => x.FormFiles, formFiles)
				.Create();

			sutFactory.FileSaver.Setup(x => x.ShouldSaveFile(formFile)).ReturnsAsync(true);

			// Act
			await sut.Handle(request, CancellationToken.None);

			// Assert
			sutFactory.RabbitMqWrapper.Verify(x => x.Publish(It.Is<FileMessage>(fileMessage =>
				fileMessage.FileName == formFile.FileName && fileMessage.FileOperation == FileOperation.Add)));
		}

		[Fact]
		public async Task Handle_WhenFileShouldNotBeSaved_ShouldNotSaveFile()
		{
			// Arrange
			var sutFactory = new SutFactory();
			var sut = sutFactory.Create();

			var formFile = Mock.Of<IFormFile>();
			var formFiles = new List<IFormFile> { formFile };

			var request = _fixture.Build<UploadFileCommand>()
				.With(x => x.FormFiles, formFiles)
				.Create();

			sutFactory.FileSaver.Setup(x => x.ShouldSaveFile(formFile)).ReturnsAsync(false);

			// Act
			await sut.Handle(request, CancellationToken.None);

			// Assert
			sutFactory.FileSaver.Verify(x => x.Save(formFile), Times.Never);

			sutFactory.Logger.Verify(x => x.Log(
				LogLevel.Debug,
				It.IsAny<EventId>(),
				It.Is<It.IsAnyType>((v, t) => v.ToString() == string.Format(LoggingConstants.FileAlreadyExistsLogMessage, formFile.FileName)),
				It.IsAny<System.Exception>(),
				It.Is<Func<It.IsAnyType, System.Exception, string>>((v, t) => true)), Times.Once);
		}

		public class SutFactory
		{
			public Mock<ILogger<UploadFileCommandHandler>> Logger { get; set; } = new Mock<ILogger<UploadFileCommandHandler>>();
			public Mock<IFileService> FileSaver { get; set; } = new Mock<IFileService>();
			public Mock<IRabbitMqWrapper> RabbitMqWrapper { get; set; } = new Mock<IRabbitMqWrapper>();

			public UploadFileCommandHandler Create()
			{
				return new UploadFileCommandHandler(Logger.Object, FileSaver.Object, RabbitMqWrapper.Object);
			}
		}
	}
}
