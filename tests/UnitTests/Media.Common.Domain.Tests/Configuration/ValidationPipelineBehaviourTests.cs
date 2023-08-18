using FluentAssertions;
using FluentValidation;
using Media.Common.Domain.Configuration;
using MediatR;
using ValidationException = FluentValidation.ValidationException;

namespace Media.Common.Domain.Tests.Configuration
{
	public class ValidationPipelineBehaviourTests
	{
		[Fact]
		public async Task Handle_WhenCancellationTokenIsCancelled_ShouldThrowOperationCancelledException()
		{
			var cancellationTokenSource = new CancellationTokenSource();

			cancellationTokenSource.Cancel();

			var validators = new List<IValidator<TestRequest>> { new TestRequestValidator() };
			var validation = new ValidationPipelineBehaviour<TestRequest, Unit>(validators);

			var func = async () => await validation.Handle(new TestRequest { Number = 0 }, null, cancellationTokenSource.Token);
			await func.Should().ThrowAsync<OperationCanceledException>();
		}

		[Fact]
		public async Task Handle_WhenInputIsInvalid_ShouldThrowValidationException()
		{
			var cancellationToken = default(CancellationToken);

			var validators = new List<IValidator<TestRequest>> { new TestRequestValidator() };
			var validation = new ValidationPipelineBehaviour<TestRequest, Unit>(validators);

			var func = async () => await validation.Handle(new TestRequest { Number = 0 }, null, cancellationToken);
			await func.Should().ThrowAsync<ValidationException>();
		}

		[Fact]
		public async Task Handle_WhenInputIsValid_ShouldNotThrowException()
		{
			var cancellationToken = default(CancellationToken);
			var validators = new List<IValidator<TestRequest>> { new TestRequestValidator() };
			var validation = new ValidationPipelineBehaviour<TestRequest, Unit>(validators);

			await validation.Handle(new TestRequest { Number = 1 }, () => Unit.Task, cancellationToken);
		}
	}

	public class TestRequest : IRequest<Unit>
	{
		public int Number { get; set; }
	}

	public class TestRequestValidator : AbstractValidator<TestRequest>
	{
		public TestRequestValidator()
		{
			RuleFor(x => x.Number)
				.GreaterThanOrEqualTo(1)
				.WithMessage("Invalid Number");
		}
	}
}
