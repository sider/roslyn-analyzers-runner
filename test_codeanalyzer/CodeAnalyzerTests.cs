using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sider.CodeAnalyzers
{
	[TestClass]
	public class CodeAnalyzerTests
	{
		private const string MicrosoftCodeQualityAnalyzersDll = @"..\..\..\packages\Microsoft.CodeQuality.Analyzers.2.9.8\analyzers\dotnet\cs\Microsoft.CodeQuality.Analyzers.dll";
		private const string MicrosoftCodeAnalysisCSharpWorkspacesDll = @"..\..\..\packages\Microsoft.CodeAnalysis.CSharp.Workspaces.3.4.0\lib\netstandard2.0\Microsoft.CodeAnalysis.CSharp.Workspaces.dll";

		private static CodeAnalyzer codeAnalyzer;

		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			codeAnalyzer = CodeAnalyzer.Create(new[] { "Microsoft.CodeQuality.Analyzers" });
		}

		[TestMethod]
		[ExpectedException(typeof(System.IO.FileNotFoundException))]
		public void TestCreateWithUnknownAnalyzer()
		{
			CodeAnalyzer.Create(new[] { "Microsoft.CodeQuality.Analyzers", "Foo.Bar" });
		}

		[TestMethod]
		[DeploymentItem(MicrosoftCodeQualityAnalyzersDll)]
		[DeploymentItem(MicrosoftCodeAnalysisCSharpWorkspacesDll)]
		[DeploymentItem(@"example\Class1.cs", @"example")]
		public void TestDiagnose()
		{
			var expected = @"file: example\Class1.cs

id: CA2219
location: SourceFile(Class1.cs[1215..1237))
message: finally 句内から例外を発生させないでください。 

id: CA1031
location: SourceFile(Class1.cs[1127..1132))
message: '.ctor' を変更して特定の例外の種類をさらにキャッチするか、例外を再スローします。

id: CA1714
location: SourceFile(Class1.cs[245..249))
message: フラグ列挙型は、複数形の名前を含んでいなければなりません

id: CA1008
location: SourceFile(Class1.cs[302..310))
message: 提案された名前 'None' を伴う、値 0 を含む TestEnum にメンバーを追加します。

id: CA1711
location: SourceFile(Class1.cs[302..310))
message: 型名 'TestEnum' が 'Enum' で終わらないように変更します。

id: CA1812
location: SourceFile(Class1.cs[212..214))
message: A2 は、インスタンス化されていない内部クラスです。その場合、コードをアセンブリから削除してください。このクラスが静的メンバーのみを含むことを意図している場合は、このクラスを static (Visual Basic の場合は Shared) にしてください。

id: CA1812
location: SourceFile(Class1.cs[353..359))
message: Class1 は、インスタンス化されていない内部クラスです。その場合、コードをアセンブリから削除してください。このクラスが静的メンバーのみを含むことを意図している場合は、このクラスを static (Visual Basic の場合は Shared) にしてください。

id: CA1016
location: None
message: アセンブリにアセンブリ バージョンを設定します

id: CA1014
location: None
message: アセンブリに CLSCompliant を設定します

";

			var actual = codeAnalyzer.Diagnose(new[] { @"example\Class1.cs" }).ToSimpleText();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[DeploymentItem(MicrosoftCodeQualityAnalyzersDll)]
		[DeploymentItem(MicrosoftCodeAnalysisCSharpWorkspacesDll)]
		[DeploymentItem(@"example\Class2.cs", @"example")]
		[DeploymentItem(@"example\Class3.cs", @"example")]
		public void TestDiagnoseMultipleFiles()
		{
			var expected = @"file: example\Class2.cs

id: CA2219
location: SourceFile(Class2.cs[204..226))
message: finally 句内から例外を発生させないでください。 

id: CA1714
location: SourceFile(Class2.cs[266..274))
message: フラグ列挙型は、複数形の名前を含んでいなければなりません

id: CA1016
location: None
message: アセンブリにアセンブリ バージョンを設定します

id: CA1014
location: None
message: アセンブリに CLSCompliant を設定します

file: example\Class3.cs

id: CA1008
location: SourceFile(Class3.cs[270..274))
message: 提案された名前 'None' を伴う、値 0 を含む Test にメンバーを追加します。

id: CA1016
location: None
message: アセンブリにアセンブリ バージョンを設定します

id: CA1014
location: None
message: アセンブリに CLSCompliant を設定します

";

			var actual = codeAnalyzer.Diagnose(new[] { @"example\Class2.cs", @"example\Class3.cs" }).ToSimpleText();
			
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[DeploymentItem(MicrosoftCodeQualityAnalyzersDll)]
		[DeploymentItem(MicrosoftCodeAnalysisCSharpWorkspacesDll)]
		[DeploymentItem(@"example\TestPy.py", @"example")]
		[DeploymentItem(@"example\Class4.cs", @"example")]
		public void TestDiagnoseIllegalFiles()
		{
			var expected = @"file: example\TestPy.py

id: CA1823
location: SourceFile(TestPy.py[47..50))
message: 未使用のフィールド 'def'。

id: CA1823
location: SourceFile(TestPy.py[32..37))
message: 未使用のフィールド 'numpy'。

id: CA1016
location: None
message: アセンブリにアセンブリ バージョンを設定します

id: CA1014
location: None
message: アセンブリに CLSCompliant を設定します

file: example\Class4.cs

id: CA1823
location: SourceFile(Class4.cs[47..50))
message: 未使用のフィールド 'def'。

id: CA1823
location: SourceFile(Class4.cs[32..37))
message: 未使用のフィールド 'numpy'。

id: CA1016
location: None
message: アセンブリにアセンブリ バージョンを設定します

id: CA1014
location: None
message: アセンブリに CLSCompliant を設定します

";

			var actual = codeAnalyzer.Diagnose(new[] { @"example\TestPy.py", @"example\Class4.cs" }).ToSimpleText();

			Assert.AreEqual(expected, actual);
		}
	}
}
