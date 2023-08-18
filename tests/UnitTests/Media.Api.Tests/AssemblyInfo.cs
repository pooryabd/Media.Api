using System.Diagnostics.CodeAnalysis;

[assembly: TestFramework("Meziantou.Xunit.ParallelTestFramework", "Media.Api.Tests")]
[assembly: AssemblyTrait("Category", "UnitTest")]
[assembly: ExcludeFromCodeCoverage]