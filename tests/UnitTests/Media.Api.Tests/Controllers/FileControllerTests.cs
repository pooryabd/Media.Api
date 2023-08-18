using AutoFixture.Xunit2;
using FluentAssertions;
using Media.Api.Controllers;
using Media.Api.Models;
using Media.Common.Domain.Models.DTO;
using Media.Common.Domain.Services.File.Commands;
using Media.Common.Domain.Services.File.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Media.Api.Tests.Controllers
{
	public class FileControllerTests
	{
		[Fact]
		public async Task UploadFiles_WhenCalled_ShouldCallMediatorAsExpected()
		{
			// arrange
			var file = Mock.Of<IFormFile>();
			var request = new List<IFormFile>()
				{
					file
				};

			var sutFactory = new SutFactory();
			var sut = sutFactory.Create();

			// act
			await sut.UploadFiles(request);

			// assert
			sutFactory.Mediator.Verify(x =>
					x.Send(It.Is<UploadFileCommand>(uploadFileCommand => uploadFileCommand.FormFiles == request), default),
				Times.Once);
		}

		[Theory]
		[AutoData]
		public async Task GetFiles_WhenCalled_ShouldCallMediatorAsExpectedAndReturnsData(List<GetFilesDto> filesDtos)
		{
			// arrange
			var sutFactory = new SutFactory();
			var sut = sutFactory.Create();

			sutFactory.Mediator.Setup(x => x.Send(It.IsAny<GetFilesQuery>(), default)).ReturnsAsync(filesDtos);

			// act
			var actualResult = await sut.GetFiles();

			// assert
			actualResult.Should().NotBeNull();
			var objectResult = (ObjectResult)actualResult;
			objectResult.Value.Should().BeOfType<GetFileResponse>();
			var filesResponse = (GetFileResponse)objectResult!.Value;
			filesResponse.GetFilesDtos.Should().BeEquivalentTo(filesDtos);
		}
		public class SutFactory
		{
			public Mock<IMediator> Mediator { get; set; } = new Mock<IMediator>();

			public FileController Create()
			{
				return new FileController(Mediator.Object);
			}
		}
	}
}
