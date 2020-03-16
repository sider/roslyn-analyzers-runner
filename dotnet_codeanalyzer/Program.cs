using CommandLine;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Sider.CodeAnalyzers
{
	public static class Program
	{
		private static string[] Analyzers = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(ConfigurationManager.AppSettings["analyzers"]);

		private class Options
		{
			[Option()]
			public IEnumerable<string> Targets { get; set; }
		}

		public static int Main(string[] args)
		{
			Tuple<int, string> exitCode;

			var parseResult = Parser.Default.ParseArguments<Options>(args);

			if (parseResult.Tag == ParserResultType.NotParsed)
			{
				exitCode = Tuple.Create(-1, string.Empty);
				goto Exit;
			}

			try
			{
				var parsed = (Parsed<Options>)parseResult;
				var results = CodeAnalyzer
					.Create(Analyzers)
					.Diagnose(parsed.Value.Targets)
					.ToJsonString();
				exitCode = Tuple.Create(0, results);
			}
			catch (Exception e)
			{
				exitCode = Tuple.Create(1, e.ToString());
			}

		Exit:
			var con = exitCode.Item1 == 0 ? Console.Out : Console.Error;
			con.WriteLine(exitCode.Item2);
			return exitCode.Item1;
		}
	}
}
