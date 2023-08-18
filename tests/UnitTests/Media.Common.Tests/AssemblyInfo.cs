using System.Diagnostics.CodeAnalysis;

[assembly: TestFramework("Meziantou.Xunit.ParallelTestFramework", "Media.Common.Tests")]
[assembly: AssemblyTrait("Category", "UnitTest")]
[assembly: ExcludeFromCodeCoverage]