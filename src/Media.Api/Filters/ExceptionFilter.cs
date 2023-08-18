// <copyright file="ExceptionFilter.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Api
{
	using System.Diagnostics.CodeAnalysis;
	using System.Net;
	using FluentValidation;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.Filters;
	using Models;

	/// <summary>
	/// Class ExceptionFilter
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class ExceptionFilter : IExceptionFilter
	{
		private readonly ILogger<ExceptionFilter> _loggingService;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExceptionFilter"/> class.
		/// </summary>
		/// <param name="loggingService">The loggingService</param>
		public ExceptionFilter(ILogger<ExceptionFilter> loggingService)
		{
			_loggingService = loggingService;
		}

		/// <inheritdoc/>
		public void OnException(ExceptionContext context)
		{
			// is exception handled
			if (context.Result != null)
			{
				return;
			}

			var exception = context.Exception;
			var errors = new List<ErrorModel>();
			HttpStatusCode statusCode;
			switch (exception)
			{
				case ValidationException validationException:
					var validationError = validationException.Errors.First();
					errors.Add(new ErrorModel()
					{
						ErrorCode = validationError.ErrorCode,
						ErrorMessage = validationError.ErrorMessage,
					});

					statusCode = HttpStatusCode.BadRequest;
					break;

				default:
					_loggingService.LogDebug(exception.Message);

					errors.Add(new ErrorModel()
					{
						ErrorCode = "Unknown error",
						ErrorMessage = exception.Message,
					});

					statusCode = HttpStatusCode.BadRequest;
					break;
			}

			var response = new BaseResponse();
			response.Errors ??= new List<ErrorModel>();
			foreach (var error in errors)
			{
				response.Errors.Add(error);
			}

			context.Result = new ObjectResult(response)
			{
				StatusCode = (int)statusCode
			};
		}
	}
}
