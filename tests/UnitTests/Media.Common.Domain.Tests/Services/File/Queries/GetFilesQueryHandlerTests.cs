using AutoFixture.Xunit2;
using FluentAssertions;
using Media.Common.Contracts;
using Media.Common.Domain.Contracts;
using Media.Common.Domain.Models.DTO;
using Media.Common.Domain.Services.File.Queries;
using Media.Common.Enumerations;
using Media.Common.Models;
using Moq;

namespace Media.Common.Domain.Tests.Services.File.Queries
{
	public class GetFilesQueryHandlerTests
	{
		[Theory]
		[AutoData]
		public async Task Handle_WhenCalled_ShouldCallDiskFileServiceGetFilesMethod(GetFilesQuery getFilesQuery, List<GetFilesDto> getFilesDtos, CancellationToken cancellationToken)
		{
			// arrange
			var sutFactory = new SutFactory();
			var sut = sutFactory.Create();

			sutFactory.FileSaver.Setup(x => x.GetFiles()).ReturnsAsync(getFilesDtos);

			// act
			var actualResult = await sut.Handle(getFilesQuery, cancellationToken);

			// assert
			actualResult.Should().BeEquivalentTo(getFilesDtos);
		}

		[Theory]
		[AutoData]
		public async Task Handle_WhenCalled_ShouldSendReadMessageToChangeDetectionQueue(GetFilesQuery getFilesQuery, GetFilesDto getFilesDto, CancellationToken cancellationToken)
		{
			// arrange
			var sutFactory = new SutFactory();
			var sut = sutFactory.Create();

			var getFilesDtos = new List<GetFilesDto>()
			{
				getFilesDto
			};

			sutFactory.FileSaver.Setup(x => x.GetFiles()).ReturnsAsync(getFilesDtos);

			// act
			var actualResult = await sut.Handle(getFilesQuery, cancellationToken);

			// assert
			sutFactory.RabbitMqWrapper.Verify(x => x.Publish(It.Is<FileMessage>(fileMessage =>
				fileMessage.FileName == getFilesDto.FileName && fileMessage.FileOperation == FileOperation.Read)));
		}
		public class SutFactory
		{
			public Mock<IFileService> FileSaver { get; set; } = new Mock<IFileService>();

			public Mock<IRabbitMqWrapper> RabbitMqWrapper { get; set; } = new Mock<IRabbitMqWrapper>();

			public GetFilesQueryHandler Create()
			{
				return new GetFilesQueryHandler(FileSaver.Object, RabbitMqWrapper.Object);
			}
		}
	}
}
