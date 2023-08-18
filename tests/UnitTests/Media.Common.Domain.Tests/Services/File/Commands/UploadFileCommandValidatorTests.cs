using Media.Common.Domain.Services.File.Commands;
using Moq;
using Media.Common.Contracts;
using FluentValidation.TestHelper;
using Media.Common.Domain.Constants.File;
using Microsoft.AspNetCore.Http;

namespace Media.Common.Domain.Tests.Services.File.Commands
{
	public class UploadFileCommandValidatorTests
	{
		[Fact]
		public async Task ValidateAsync_WhenFileListIsNull_ThrowsExpectedValidationException()
		{
			// arrange
			var sutFactory = new SutFactory();
			var validator = sutFactory.Create();
			var command = new UploadFileCommand();

			// act
			var result = await validator.TestValidateAsync(command);

			// assert
			result.ShouldHaveAnyValidationError()
				.WithErrorCode(ErrorCodes.InputFileListNullOrEmpty)
				.WithErrorMessage(ErrorMessages.InputFileListNullOrEmpty);
		}

		[Fact]
		public async Task ValidateAsync_WhenFileListIsEmpty_ThrowsExpectedValidationException()
		{
			// arrange
			var sutFactory = new SutFactory();
			var validator = sutFactory.Create();
			var command = new UploadFileCommand()
			{
				FormFiles = new List<IFormFile>()
			};

			// act
			var result = await validator.TestValidateAsync(command);

			// assert
			result.ShouldHaveAnyValidationError()
				.WithErrorCode(ErrorCodes.InputFileListNullOrEmpty)
				.WithErrorMessage(ErrorMessages.InputFileListNullOrEmpty);
		}

		[Fact]
		public async Task ValidateAsync_WhenAtLeastOneOfTheFileSizeIsInvalid_ThrowsExpectedValidationException()
		{
			// arrange
			var fileMock1 = new Mock<IFormFile>();
			fileMock1.Setup(f => f.Length).Returns(10000001);

			var fileMock2 = new Mock<IFormFile>();
			fileMock2.Setup(f => f.Length).Returns(10000000);

			var sutFactory = new SutFactory();
			var validator = sutFactory.Create();

			var command = new UploadFileCommand()
			{
				FormFiles = new List<IFormFile>()
				{
					fileMock1.Object,
					fileMock2.Object,
				}
			};

			var maxFileSize = 10;
			sutFactory.MediaApiConfiguration.SetupGet(x => x.MaxFileSizeInMB).Returns(maxFileSize);

			// act
			var result = await validator.TestValidateAsync(command);

			// assert
			result.ShouldHaveAnyValidationError()
				.WithErrorCode(ErrorCodes.MaxFileSizeError);
		}

		[Fact]
		public async Task ValidateAsync_WhenCalled_DoesNotThrow()
		{
			// arrange
			var fileMock1 = new Mock<IFormFile>();
			fileMock1.Setup(f => f.Length).Returns(9999999);

			var fileMock2 = new Mock<IFormFile>();
			fileMock2.Setup(f => f.Length).Returns(10000000);

			var sutFactory = new SutFactory();
			var validator = sutFactory.Create();

			var command = new UploadFileCommand()
			{
				FormFiles = new List<IFormFile>()
				{
					fileMock1.Object,
					fileMock2.Object,
				}
			};

			sutFactory.MediaApiConfiguration.Setup(x => x.MaxFileSizeInMB).Returns(10);

			// act
			var result = await validator.TestValidateAsync(command);

			// assert
			result.ShouldNotHaveAnyValidationErrors();
		}

		public class SutFactory
		{
			public Mock<IMediaApiConfiguration> MediaApiConfiguration { get; set; } = new Mock<IMediaApiConfiguration>();

			public UploadFileCommandValidator Create()
			{
				return new UploadFileCommandValidator(MediaApiConfiguration.Object);
			}
		}
	}
}
