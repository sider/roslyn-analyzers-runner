using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Sider.CodeAnalyzers
{
	public enum Language
	{
		CSharp,
	}

	public static class LanguageExtensions
	{
		private static readonly Dictionary<Language, Tuple<string, CompilationOptions>> languages
			= new Dictionary<Language, Tuple<string, CompilationOptions>>()
			{
			{ Language.CSharp,
					Tuple.Create<string, CompilationOptions>(
						LanguageNames.CSharp,
						new Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))},
			};

		public static string ToName(this Language language)
		{
			return languages[language].Item1;
		}

		public static CompilationOptions ToCompilationOptions(this Language language)
		{
			return languages[language].Item2;
		}
	}
}
