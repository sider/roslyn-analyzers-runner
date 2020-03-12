﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sider.CodeAnalyzers
{
	public class CodeAnalyzer
	{
		public static string Diagnose(IEnumerable<string> diagnosticAnalyzerAssemblyNames, IEnumerable<string> sourceCodeFilePaths)
		{
			var analyzers = ActivateAnalyzers(diagnosticAnalyzerAssemblyNames);
			var compilationOptions = CreateCompilationOptions(analyzers);

			var results = new StringBuilder();

			foreach (var sourceCodeFilePath in sourceCodeFilePaths)
			{
				results.AppendLine($"file: {sourceCodeFilePath}");
				results.AppendLine();

				var solution = CreateAdhocSolutionFromFile(sourceCodeFilePath);
				var compilation = solution.Projects.First().GetCompilationAsync().Result
					.WithOptions(compilationOptions)
					.WithAnalyzers(analyzers);

				foreach (var diagnostic in compilation.GetAnalyzerDiagnosticsAsync().Result)
				{
					results.AppendLine($"id: {diagnostic.Id}");
					results.AppendLine($"location: {diagnostic.Location}");
					results.AppendLine($"message: {diagnostic.GetMessage()}");
					results.AppendLine();
				}
			}

			return results.ToString();
		}

		private static ImmutableArray<DiagnosticAnalyzer> ActivateAnalyzers(IEnumerable<string> diagnosticAnalyzerAssemblyNames)
		{
			var analyzerAssemblies = diagnosticAnalyzerAssemblyNames.Select(n => Assembly.Load(n));

			var analyzers = analyzerAssemblies
				.SelectMany(a => a.GetTypes())
				.Where(t => t.IsSubclassOf(typeof(DiagnosticAnalyzer)))
				.Where(t => HasTargetLanguages(t, LanguageNames.CSharp))
				.Select(t => (DiagnosticAnalyzer)Activator.CreateInstance(t))
				.ToArray();

			return ImmutableArray.Create(analyzers);
		}

		private static bool HasTargetLanguages(Type analyzerType, string language)
		{
			var attribute = analyzerType.GetCustomAttributes<DiagnosticAnalyzerAttribute>().FirstOrDefault();
			if (attribute == null)
			{
				return false;
			}

			return attribute.Languages.Contains(language);
		}

		private static CompilationOptions CreateCompilationOptions(ImmutableArray<DiagnosticAnalyzer> analyzers)
		{
			var compilationOptions = new Microsoft.CodeAnalysis.CSharp.CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
				.WithSpecificDiagnosticOptions(
					analyzers.SelectMany(a => a.SupportedDiagnostics)
					.Select(d => new KeyValuePair<string, ReportDiagnostic>(d.Id, ReportDiagnostic.Warn))); // すべての診断を警告(有効)にしている。既定では無効のものも存在しているので。
			return compilationOptions;
		}

		private static Solution CreateAdhocSolutionFromFile(string filePath)
		{
			const string PrjName = "adhoc";
			var RefTypes = new[] { typeof(object) };

			var workspace = new AdhocWorkspace();
			var project = workspace.CurrentSolution
				.AddProject(PrjName, PrjName, LanguageNames.CSharp)
				.AddMetadataReferences(RefTypes.Select(t => MetadataReference.CreateFromFile(t.Assembly.Location)));

			var file = new FileInfo(filePath);
			using (var fileStream = File.Open(file.FullName, FileMode.Open, FileAccess.Read)) // TODO: エンコーディングを考慮せよ。とりあえず、TextReaderではなくて生のFileStreamで開く。
			{
				var document = project.AddDocument(file.Name, SourceText.From(fileStream));
				project = document.Project;
				return project.Solution;
			}
		}
	}
}
