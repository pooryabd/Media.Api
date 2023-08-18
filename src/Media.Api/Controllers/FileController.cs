// <copyright file="FileController.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Api.Controllers
{
	using System.Net;
	using Media.Api.Constants;
	using Media.Api.Models;
	using Media.Common.Domain.Services.File.Commands;
	using Media.Common.Domain.Services.File.Queries;
	using MediatR;
	using Microsoft.AspNetCore.Mvc;
	using Swashbuckle.AspNetCore.Annotations;

	/// <summary>
	/// Controller FileController
	/// </summary>
	[ApiController]
	public class FileController : Controller
	{
		private readonly IMediator _mediator;

		/// <summary>
		/// Initializes a new instance of the <see cref="FileController"/> class.
		/// </summary>
		/// <param name="mediator">The mediator</param>
		public FileController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Upload files
		/// </summary>
		/// <param name="formFiles">The formFiles</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		[HttpPost]
		[Route(ApiConstants.UploadFilesRoute)]
		[SwaggerOperation(summary: ApiConstants.UploadFileDescription, Tags = new[] { ApiConstants.FilesTag })]
		public async Task<IActionResult> UploadFiles(List<IFormFile> formFiles)
		{
			var command = new UploadFileCommand()
			{
				FormFiles = formFiles
			};

			await _mediator.Send(command);

			return StatusCode((int)HttpStatusCode.Created);
		}

		/// <summary>
		/// Upload files
		/// </summary>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		[HttpGet]
		[Route(ApiConstants.UploadFilesRoute)]
		[SwaggerOperation(summary: ApiConstants.GetFileDescription, Tags = new[] { ApiConstants.FilesTag })]
		public async Task<IActionResult> GetFiles()
		{
			var query = new GetFilesQuery();

			var fileListDto = await _mediator.Send(query);

			var response = new GetFileResponse()
			{
				GetFilesDtos = fileListDto
			};

			return StatusCode((int)HttpStatusCode.OK, response);
		}
	}
}
