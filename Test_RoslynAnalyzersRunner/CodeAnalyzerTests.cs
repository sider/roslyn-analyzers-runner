using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Threading;

namespace Sider.RoslynAnalyzersRunner
{
	[TestClass]
	public class CodeAnalyzerTests
	{
		private const Language CSharp = Language.CSharp;

		private const string MicrosoftCodeQualityAnalyzersDll = @"%global-packages%/microsoft.codequality.analyzers/3.0.0/analyzers/dotnet/cs/Microsoft.CodeQuality.Analyzers.dll";
		private const string MicrosoftNetCoreAnalyzersDll = @"%global-packages%/microsoft.netcore.analyzers/3.0.0/analyzers/dotnet/cs/Microsoft.NetCore.Analyzers.dll";

		[TestInitialize]
		public void TestInitialize()
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
		}

		[TestMethod]
		[ExpectedException(typeof(System.IO.FileNotFoundException))]
		public void TestCreateWithUnknownAnalyzer()
		{
			const string FooBarDll = @"%global‑packages%/microsoft.codequality.analyzers/3.0.0/analyzers/dotnet/cs/Foo.Bar.dll";

			CodeAnalyzer.Create(CSharp, new[] { MicrosoftCodeQualityAnalyzersDll, FooBarDll });
		}

		[TestMethod]
		[DeploymentItem(@"example/Class1.cs", @"example")]
		public void TestDiagnose()
		{
			var expected = @"file: example/Class1.cs

id: CA2219
location: (89,4)-(89,26)
severity: Warning
message: Do not raise an exception from within a finally clause. 

id: CA1822
location: (55,15)-(55,18)
severity: Warning
message: Member 'Add' does not access instance data and can be marked as static

id: CA1031
location: (83,3)-(83,8)
severity: Warning
message: Modify '.ctor' to catch a more specific allowed exception type, or rethrow the exception.

id: CA1714
location: (13,13)-(13,17)
severity: Warning
message: Flags enums should have plural names

id: CA1008
location: (19,13)-(19,21)
severity: Warning
message: Add a member to TestEnum that has a value of zero with a suggested name of 'None'.

id: CA1711
location: (19,13)-(19,21)
severity: Warning
message: Rename type name TestEnum so that it does not end in 'Enum'.

id: CA1812
location: (10,16)-(10,18)
severity: Warning
message: A2 is an internal class that is apparently never instantiated. If so, remove the code from the assembly. If this class is intended to contain only static members, make it static (Shared in Visual Basic).

";

			var actual = CodeAnalyzer.Create(CSharp, new[] { MicrosoftCodeQualityAnalyzersDll })
				.Diagnose(new[] { @"example/Class1.cs" })
				.ToSimpleText();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[DeploymentItem(@"example/Class2.cs", @"example")]
		[DeploymentItem(@"example/Class3.cs", @"example")]
		public void TestDiagnoseMultipleFiles()
		{
			var expected = @"file: example/Class2.cs

id: CA2219
location: (20,4)-(20,26)
severity: Warning
message: Do not raise an exception from within a finally clause. 

id: CA1822
location: (12,15)-(12,28)
severity: Warning
message: Member 'TestException' does not access instance data and can be marked as static

id: CA1714
location: (25,14)-(25,22)
severity: Warning
message: Flags enums should have plural names

id: CA1060
location: (7,14)-(7,20)
severity: Warning
message: Move pinvokes to native methods class

file: example/Class3.cs

id: CA1822
location: (10,14)-(10,18)
severity: Warning
message: Member 'Test' does not access instance data and can be marked as static

id: CA1008
location: (18,14)-(18,18)
severity: Warning
message: Add a member to Test that has a value of zero with a suggested name of 'None'.

";

			var actual = CodeAnalyzer.Create(CSharp, new[] { MicrosoftCodeQualityAnalyzersDll })
				.Diagnose(new[] { @"example/Class2.cs", @"example/Class3.cs" })
				.ToSimpleText();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[DeploymentItem(@"example/TestPy.py", @"example")]
		[DeploymentItem(@"example/Class4.cs", @"example")]
		public void TestDiagnoseIllegalFiles()
		{
			var expected = @"file: example/TestPy.py

file: example/Class4.cs

";

			var actual = CodeAnalyzer.Create(CSharp, new[] { MicrosoftCodeQualityAnalyzersDll })
				.Diagnose(new[] { @"example/TestPy.py", @"example/Class4.cs" })
				.ToSimpleText();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[DeploymentItem(@"example/Class2.cs", @"example")]
		public void TestDiagnoseNetCoreAnalyzers()
		{
			var expected = @"file: example/Class2.cs

id: CA5392
location: (10,28)-(10,41)
severity: Warning
message: The method SetWindowText didn't use DefaultDllImportSearchPaths attribute for P/Invokes.

id: CA2201
location: (20,10)-(20,25)
severity: Warning
message: Exception type System.Exception is not sufficiently specific.

id: CA1401
location: (10,28)-(10,41)
severity: Warning
message: P/Invoke method 'SetWindowText' should not be visible

id: CA2101
location: (9,3)-(9,56)
severity: Warning
message: Specify marshaling for P/Invoke string arguments

";

			var actual = CodeAnalyzer.Create(CSharp, new[] { MicrosoftNetCoreAnalyzersDll })
				.Diagnose(new[] { @"example/Class2.cs" })
				.ToSimpleText();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[DeploymentItem(@"example/Class2.cs", @"example")]
		public void TestDiagnoseMultipleAnalyzers()
		{
			var expected = @"file: example/Class2.cs

id: CA2219
location: (20,4)-(20,26)
severity: Warning
message: Do not raise an exception from within a finally clause. 

id: CA1822
location: (12,15)-(12,28)
severity: Warning
message: Member 'TestException' does not access instance data and can be marked as static

id: CA1714
location: (25,14)-(25,22)
severity: Warning
message: Flags enums should have plural names

id: CA1060
location: (7,14)-(7,20)
severity: Warning
message: Move pinvokes to native methods class

id: CA5392
location: (10,28)-(10,41)
severity: Warning
message: The method SetWindowText didn't use DefaultDllImportSearchPaths attribute for P/Invokes.

id: CA2201
location: (20,10)-(20,25)
severity: Warning
message: Exception type System.Exception is not sufficiently specific.

id: CA1401
location: (10,28)-(10,41)
severity: Warning
message: P/Invoke method 'SetWindowText' should not be visible

id: CA2101
location: (9,3)-(9,56)
severity: Warning
message: Specify marshaling for P/Invoke string arguments

";

			var actual = CodeAnalyzer.Create(CSharp, new[] { MicrosoftCodeQualityAnalyzersDll, MicrosoftNetCoreAnalyzersDll })
				.Diagnose(new[] { @"example/Class2.cs" })
				.ToSimpleText();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[DeploymentItem(@"example/HelloWorldUtf8.cs", @"example")]
		public void TestUtf8File()
		{
			var expected = @"file: example/HelloWorldUtf8.cs

id: CA2219
location: (14,4)-(14,10)
severity: Warning
message: Do not raise an exception from within a finally clause. 

";

			var actual = CodeAnalyzer.Create(CSharp, new[] { MicrosoftCodeQualityAnalyzersDll })
				.Diagnose(new[] { @"example/HelloWorldUtf8.cs" })
				.ToSimpleText();

			Assert.AreEqual(expected, actual);
		}

		[Ignore]
		[TestMethod]
		[DeploymentItem(@"example/HelloWorldSjis.cs", @"example")]
		public void TestSjisFile()
		{
			var expected = @"file: example/HelloWorldSjis.cs

id: CA2219
location: (14,4)-(14,10)
severity: Warning
message: Do not raise an exception from within a finally clause. 

";

			var actual = CodeAnalyzer.Create(CSharp, new[] { MicrosoftCodeQualityAnalyzersDll })
				.Diagnose(new[] { @"example/HelloWorldSjis.cs" })
				.ToSimpleText();

			Assert.AreEqual(expected, actual);
		}
	}
}
