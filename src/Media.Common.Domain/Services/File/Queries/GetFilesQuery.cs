// <copyright file="GetFilesQuery.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Domain.Services.File.Queries
{
	using Media.Common.Domain.Models.DTO;
	using MediatR;

	/// <summary>
	/// Class GetFilesQuery
	/// </summary>
	public class GetFilesQuery : IRequest<List<GetFilesDto>>
	{
	}
}
