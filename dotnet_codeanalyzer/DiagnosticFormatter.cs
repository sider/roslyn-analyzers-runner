using System.Collections.Immutable;
using System.Text;

namespace Sider.CodeAnalyzers
{
	public static class DiagnosticFormatter
	{
		public static string ToSimpleText(this ImmutableArray<AnalysisResult> diagnosticResults)
		{
			var results = new StringBuilder();

			foreach (var result in diagnosticResults)
			{
				results.AppendLine($"file: {result.SourceCodeFilePath}");
				results.AppendLine();

				foreach (var diagnostic in result.Diagnostics)
				{
					results.AppendLine($"id: {diagnostic.Id}");
					results.AppendLine($"location: {diagnostic.Location}");
					results.AppendLine($"message: {diagnostic.Message}");
					results.AppendLine();
				}
			}

			return results.ToString();
		}

		public static string ToJsonString(this ImmutableArray<AnalysisResult> diagnosticResults)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(diagnosticResults);
		}
	}
}
