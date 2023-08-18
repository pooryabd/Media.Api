using AutoFixture.Xunit2;
using FluentAssertions;
using Media.Common.Domain.Extensions;
using Media.Common.Models;

namespace Media.Common.Domain.Tests.Extensions
{
	public class MediaApiConfigurationExtensionsTests
	{
		[Theory]
		[AutoData]
		public void GetFullFilePath_WhenCalled_ReturnsExpected(MediaApiConfiguration configuration, string fileName)
		{
			// arrange
			var expectedPath = Path.Combine(configuration.PathToSaveFiles, fileName);

			// act
			var actualPath = configuration.GetFullFilePath(fileName);

			// assert
			actualPath.Should().Be(expectedPath);
		}
	}
}
