using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Globalization;
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
				.Where(d => d.Location != Location.None) // 有効なLocation値を持つ(ソースコードファイルに対する)結果のみを残す
				.Select(d => DiagnosticResult.CreateFrom(d))
				.ToImmutableArray();
		}
	}

	public class DiagnosticResult
	{
		public string Id { get; private set; }
		public LinePositionSpan Location { get; private set; }
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
				Location = diagnostic.Location.GetLineSpan().Span,
				Message = diagnostic.GetMessage(),
				WarningLevel = diagnostic.WarningLevel,
				Severity = diagnostic.Severity,
				Title = descriptor.Title.ToString(CultureInfo.CurrentCulture),
				Description = descriptor.Description.ToString(CultureInfo.CurrentCulture),
				HelpLinkUri = descriptor.HelpLinkUri,
				Category = descriptor.Category,
			};
		}
	}
}
