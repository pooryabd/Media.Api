using FluentAssertions;
using Media.Common.Extensions;

namespace Media.Common.Tests.Extensions
{
	public class FileExtensionsTests
	{
		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		public void ToMegabytes_WhenCalledWithZeroOrLowerThatZero_ShouldReturnZero(long valueInBytes)
		{
			// arrange and act
			var actualResult = valueInBytes.ToMegabytes();

			// assert
			actualResult.Should().Be(0);
		}

		[Theory]
		[InlineData(1000000, 1)]
		[InlineData(1500000, 1.5)]
		[InlineData(1900000, 1.9)]
		[InlineData(1100000, 1.1)]
		public void ToMegabytes_WhenCalled_ShouldReturnAsExpected(long valueInBytes, decimal expectedResultInMb)
		{
			// arrange and act
			var actualResult = valueInBytes.ToMegabytes();

			// assert
			actualResult.Should().Be(expectedResultInMb);
		}
	}
}
