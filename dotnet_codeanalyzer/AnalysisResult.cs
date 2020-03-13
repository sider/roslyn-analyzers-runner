using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;

namespace Sider.CodeAnalyzers
{
	public class AnalysisResult
	{
		public string SourceCodeFilePath { get; private set; }
		public ImmutableArray<DiagnosticResult> Diagnostics { get; private set; }

		public AnalysisResult(string sourceCodeFilePath, ImmutableArray<Diagnostic> diagnostics)
		{
			this.SourceCodeFilePath = sourceCodeFilePath;
			this.Diagnostics = diagnostics
				.Select(d => DiagnosticResult.CreateFrom(d))
				.ToImmutableArray();
		}
	}

	public class DiagnosticResult
	{
		public string Id { get; private set; }
		public Location Location { get; private set; }
		public string Message { get; private set; }
		public int WarningLevel { get; private set; }
		public DiagnosticSeverity Severity { get; private set; }
		public string Title { get; private set; }
		public string Description { get; private set; }
		public string HelpLinkUri { get; private set; }
		public string Category { get; private set; }


		public static DiagnosticResult CreateFrom(Diagnostic diagnostic)
		{
			var descriptor = diagnostic.Descriptor;

			return new DiagnosticResult()
			{
				Id = diagnostic.Id,
				Location = diagnostic.Location,
				Message = diagnostic.GetMessage(),
				WarningLevel = diagnostic.WarningLevel,
				Severity = diagnostic.Severity,
				Title = descriptor.Title.ToString(),
				Description = descriptor.Description.ToString(),
				HelpLinkUri = descriptor.HelpLinkUri,
				Category = descriptor.Category,
			};
		}
	}
}
