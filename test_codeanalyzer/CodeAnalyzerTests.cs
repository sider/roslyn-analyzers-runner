using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sider.CodeAnalyzers
{
	[TestClass]
	public class CodeAnalyzerTests
	{
		private const string MicrosoftCodeQualityAnalyzersDll = @"%global‑packages%/microsoft.codequality.analyzers/2.9.8/analyzers/dotnet/cs/Microsoft.CodeQuality.Analyzers.dll";
		private const string MicrosoftNetCoreAnalyzersDll = @"%global‑packages%/microsoft.netcore.analyzers/2.9.8/analyzers/dotnet/cs/Microsoft.NetCore.Analyzers.dll";

		[TestMethod]
		[ExpectedException(typeof(System.IO.FileNotFoundException))]
		public void TestCreateWithUnknownAnalyzer()
		{
			const string FooBarDll = @"%global‑packages%/microsoft.codequality.analyzers/2.9.8/analyzers/dotnet/cs/Foo.Bar.dll";

			CodeAnalyzer.Create(new[] { MicrosoftCodeQualityAnalyzersDll, FooBarDll });
		}

		[TestMethod]
		[DeploymentItem(@"example/Class1.cs", @"example")]
		public void TestDiagnose()
		{
			var expected = @"file: example/Class1.cs

id: CA2219
location: (89,4)-(89,26)
message: finally 句内から例外を発生させないでください。 

id: CA1031
location: (83,3)-(83,8)
message: '.ctor' を変更して特定の例外の種類をさらにキャッチするか、例外を再スローします。

id: CA1714
location: (13,13)-(13,17)
message: フラグ列挙型は、複数形の名前を含んでいなければなりません

id: CA1008
location: (19,13)-(19,21)
message: 提案された名前 'None' を伴う、値 0 を含む TestEnum にメンバーを追加します。

id: CA1711
location: (19,13)-(19,21)
message: 型名 'TestEnum' が 'Enum' で終わらないように変更します。

id: CA1812
location: (25,7)-(25,13)
message: Class1 は、インスタンス化されていない内部クラスです。その場合、コードをアセンブリから削除してください。このクラスが静的メンバーのみを含むことを意図している場合は、このクラスを static (Visual Basic の場合は Shared) にしてください。

id: CA1812
location: (10,16)-(10,18)
message: A2 は、インスタンス化されていない内部クラスです。その場合、コードをアセンブリから削除してください。このクラスが静的メンバーのみを含むことを意図している場合は、このクラスを static (Visual Basic の場合は Shared) にしてください。

";

			var actual = CodeAnalyzer.Create(new[] { MicrosoftCodeQualityAnalyzersDll })
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
message: finally 句内から例外を発生させないでください。 

id: CA1714
location: (25,14)-(25,22)
message: フラグ列挙型は、複数形の名前を含んでいなければなりません

id: CA1060
location: (7,14)-(7,20)
message: pinvoke をネイティブ メソッド クラスに移動します

file: example/Class3.cs

id: CA1008
location: (18,14)-(18,18)
message: 提案された名前 'None' を伴う、値 0 を含む Test にメンバーを追加します。

";

			var actual = CodeAnalyzer.Create(new[] { MicrosoftCodeQualityAnalyzersDll })
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

id: CA1823
location: (3,0)-(3,3)
message: 未使用のフィールド 'def'。

id: CA1823
location: (1,7)-(1,12)
message: 未使用のフィールド 'numpy'。

file: example/Class4.cs

id: CA1823
location: (3,0)-(3,3)
message: 未使用のフィールド 'def'。

id: CA1823
location: (1,7)-(1,12)
message: 未使用のフィールド 'numpy'。

";

			var actual = CodeAnalyzer.Create(new[] { MicrosoftCodeQualityAnalyzersDll })
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
message: メソッド SetWindowText で、P/Invoke に対して DefaultDllImportSearchPaths 属性が使用されませんでした。

id: CA1401
location: (10,28)-(10,41)
message: P/Invoke メソッド 'SetWindowText' は参照可能にすることはできません

id: CA2101
location: (9,3)-(9,56)
message: P/Invoke 文字列引数に対してマーシャリングを指定します

";

			var actual = CodeAnalyzer.Create(new[] { MicrosoftNetCoreAnalyzersDll })
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
message: finally 句内から例外を発生させないでください。 

id: CA1714
location: (25,14)-(25,22)
message: フラグ列挙型は、複数形の名前を含んでいなければなりません

id: CA1060
location: (7,14)-(7,20)
message: pinvoke をネイティブ メソッド クラスに移動します

id: CA5392
location: (10,28)-(10,41)
message: メソッド SetWindowText で、P/Invoke に対して DefaultDllImportSearchPaths 属性が使用されませんでした。

id: CA1401
location: (10,28)-(10,41)
message: P/Invoke メソッド 'SetWindowText' は参照可能にすることはできません

id: CA2101
location: (9,3)-(9,56)
message: P/Invoke 文字列引数に対してマーシャリングを指定します

";

			var actual = CodeAnalyzer.Create(new[] { MicrosoftCodeQualityAnalyzersDll, MicrosoftNetCoreAnalyzersDll })
				.Diagnose(new[] { @"example/Class2.cs" })
				.ToSimpleText();

			Assert.AreEqual(expected, actual);
		}
	}
}
