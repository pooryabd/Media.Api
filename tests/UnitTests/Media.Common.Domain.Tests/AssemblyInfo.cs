using System.Diagnostics.CodeAnalysis;

[assembly: TestFramework("Meziantou.Xunit.ParallelTestFramework", "Media.Common.Domain.Tests")]
[assembly: AssemblyTrait("Category", "UnitTest")]
[assembly: ExcludeFromCodeCoverage]