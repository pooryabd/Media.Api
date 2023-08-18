// <copyright file="UploadFileCommand.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Domain.Services.File.Commands
{
	using MediatR;
	using Microsoft.AspNetCore.Http;

	/// <summary>
	/// Class UploadFileCommand
	/// </summary>
	public class UploadFileCommand : IRequest<bool>
	{
		/// <summary>
		/// Gets or sets FormFiles
		/// </summary>
		public List<IFormFile> FormFiles { get; set; }
	}
}
