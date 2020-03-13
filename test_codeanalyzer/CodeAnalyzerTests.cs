using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sider.CodeAnalyzers
{
	[TestClass]
	public class CodeAnalyzerTests
	{
		[TestMethod]
		[DeploymentItem(@"example\Class1.cs", @"example")]
		[DeploymentItem(@"..\..\..\packages\Microsoft.CodeQuality.Analyzers.2.9.8\analyzers\dotnet\cs\Microsoft.CodeQuality.Analyzers.dll")]
		[DeploymentItem(@"..\..\..\packages\Microsoft.CodeAnalysis.CSharp.Workspaces.3.4.0\lib\netstandard2.0\Microsoft.CodeAnalysis.CSharp.Workspaces.dll")]
		public void Test_Diagnose()
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

			var actual = CodeAnalyzer.Diagnose(
				new[] { "Microsoft.CodeQuality.Analyzers" },
				new[] { @"example\Class1.cs" });


			Assert.AreEqual(expected, actual);
		}
	}
}
