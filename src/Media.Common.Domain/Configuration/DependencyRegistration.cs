// <copyright file="DependencyRegistration.cs" company="Visual Art - Poorya Bahadori Code Practice Media API">
// Copyright by Visual Art - Poorya Bahadori Code Practice Media API. All rights reserved.
// </copyright>

namespace Media.Common.Domain.Configuration
{
	using System.Diagnostics.CodeAnalysis;
	using LightInject;
	using MediatR;
	using MediatR.Pipeline;

	/// <summary>
	/// Class DependencyRegistration
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class DependencyRegistration
	{
		/// <summary>
		/// Adds the services registrations to light inject container
		/// </summary>
		/// <param name="serviceContainer">light inject container.</param>
		/// <returns>light inject container with registration.</returns>
		public static IServiceContainer RegisterApplicationServices(this IServiceContainer serviceContainer)
		{
			RegisterFluentValidator(serviceContainer);
			RegisterMediatrAndPipelines(serviceContainer);
			return serviceContainer;
		}

		private static void RegisterFluentValidator(IServiceContainer serviceContainer)
		{
			// Add fluent validation
			FluentValidation
				.AssemblyScanner
				.FindValidatorsInAssembly(typeof(DependencyRegistration).Assembly, true)
				.ForEach(result => { serviceContainer.Register(result.InterfaceType, result.ValidatorType, new PerScopeLifetime()); });
		}

		private static void RegisterMediatrAndPipelines(IServiceContainer serviceContainer)
		{
			serviceContainer.Register<IMediator, Mediator>();
			serviceContainer
				.RegisterAssembly(typeof(DependencyRegistration).Assembly, (serviceType, implementingType) =>
					serviceType.IsConstructedGenericType &&
					(
						serviceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
						serviceType.GetGenericTypeDefinition() == typeof(INotificationHandler<>)));

			Type[] implementingTypes = new[]
			{
				typeof(RequestPreProcessorBehavior<,>), // Built in pre-processor behaviour
				typeof(RequestPostProcessorBehavior<,>), // Built in post processor behaviour
				typeof(ValidationPipelineBehaviour<,>) // Fluent validation behaviour
			};
			serviceContainer.RegisterOrdered(typeof(IPipelineBehavior<,>), implementingTypes, type => null);
		}
	}
}
