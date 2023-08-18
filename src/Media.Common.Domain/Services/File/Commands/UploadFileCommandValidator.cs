// <copyright file="UploadFileCommandValidator.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Domain.Services.File.Commands
{
	using FluentValidation;
	using Media.Common.Contracts;
	using Media.Common.Domain.Constants.File;
	using Media.Common.Extensions;
	using Microsoft.AspNetCore.Http;

	/// <summary>
	/// Class UploadFileCommandValidator
	/// </summary>
	public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
	{
		private readonly IMediaApiConfiguration _mediaApiConfiguration;

		/// <summary>
		/// Initializes a new instance of the <see cref="UploadFileCommandValidator"/> class.
		/// </summary>
		/// <param name="mediaApiConfiguration">The mediaApiConfiguration</param>
		public UploadFileCommandValidator(IMediaApiConfiguration mediaApiConfiguration)
		{
			_mediaApiConfiguration = mediaApiConfiguration;

			ClassLevelCascadeMode = CascadeMode.Stop;
			RuleLevelCascadeMode = CascadeMode.Stop;

			SetupRules();
		}

		private void SetupRules()
		{
			RuleFor(uploadFileCommand => uploadFileCommand.FormFiles)
				.Must(FilesIsNotNullOrEmpty)
				.WithErrorCode(ErrorCodes.InputFileListNullOrEmpty)
				.WithMessage(ErrorMessages.InputFileListNullOrEmpty);

			RuleForEach(uploadFileCommand => uploadFileCommand.FormFiles)
				.Must(FileSizeIsValid)
				.WithErrorCode(ErrorCodes.MaxFileSizeError)
				.WithMessage(string.Format(ErrorMessages.MaxFileSizeError, _mediaApiConfiguration.MaxFileSizeInMB));
		}

		private bool FileSizeIsValid(IFormFile formFile)
		{
			return formFile.Length.ToMegabytes() <= _mediaApiConfiguration.MaxFileSizeInMB;
		}

		private bool FilesIsNotNullOrEmpty(List<IFormFile> files)
		{
			return files != null && files.Any();
		}
	}
}
