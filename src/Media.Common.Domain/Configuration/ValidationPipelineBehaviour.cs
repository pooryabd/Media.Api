// <copyright file="ValidationPipelineBehaviour.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Domain.Configuration
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using FluentValidation;
	using MediatR;

	/// <summary>
	/// Mediator validation pipeline
	/// </summary>
	/// <typeparam name="TRequest">Request</typeparam>
	/// <typeparam name="TResponse">Response</typeparam>
	public sealed class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
			where TRequest : IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator> _validators;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidationPipelineBehaviour{TRequest, TResponse}"/> class.
		/// </summary>
		/// <param name="validators">Fluent Validators</param>
		public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		/// <inheritdoc />
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var validatorTasks = _validators.Select(x => x.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken));
			var validationResult = await Task.WhenAll(validatorTasks);
			var failures = validationResult.SelectMany(x => x.Errors).Where(x => x != null).ToList();

			if (failures.Any())
			{
				throw new ValidationException(failures);
			}

			return await next();
		}
	}
}