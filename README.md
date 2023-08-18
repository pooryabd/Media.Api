# File Management API

This project is a .NET 7 API gateway that provides two routes for managing files:

1. Uploading a file
2. Retrieving a list of files

In addition, there is a background service that is responsible for detecting changes to the files.

## Technical Details

This project was developed using a Test-Driven Development (TDD) approach. The following tools and libraries were used:

- xUnit for unit testing
- Moq for mocking dependencies in tests
- FluentAssertions for more readable test assertions
- AutoFixture for generating test data

The API uses FluentValidation to validate incoming requests and follows the MediatR design pattern. Communication between the API and the background service is handled using RabbitMQ. The API also includes Swagger documentation, which can be accessed at the `~/swagger` path.

No authorization or authentication is required at this stage.

## Getting Started

To run this project, you will need to have .NET 7 installed on your machine. Once you have cloned the repository, navigate to the project directory and run the following command to start the API:

```
dotnet run
```

You can then use a tool like Postman or Swagger to send requests to the API’s routes and test its functionality.

In addition, you will need to run the `FileChangeDetectionService` to listen to the queue and write changes to the console window. To do this, navigate to the `FileChangeDetectionService` directory and run the following command:

```
dotnet run
```

Once both the API and the `FileChangeDetectionService` are running, you can test the system by uploading files through the API and observing the changes being written to the console window by the `FileChangeDetectionService`.