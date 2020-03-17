﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Sider.CodeAnalyzers
{
	public static class Program
	{
		private static readonly string[] Analyzers =
			Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(ConfigurationManager.AppSettings["analyzers"]);

		private class Options
		{
			public const string Stdout = "-";

			[Option(Default = Stdout)]
			public string OutputFile { get; set; }

			[Option()]
			public IEnumerable<string> Targets { get; set; }
		}

		public static int Main(string[] args)
		{
			int exitCode;

			var parseResult = Parser.Default.ParseArguments<Options>(args);

			if (parseResult.Tag == ParserResultType.NotParsed)
			{
				exitCode = -1;
				goto Exit;
			}

			try
			{
				var parsed = (Parsed<Options>)parseResult;
				var outputFile = parsed.Value.OutputFile;
				using var writer = outputFile == Options.Stdout ? Console.Out : File.CreateText(outputFile);

				CodeAnalyzer
					.Create(Analyzers)
					.Diagnose(parsed.Value.Targets)
					.DumpJsonStringTo(writer);
				exitCode = 0;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
				exitCode = 1;
			}

		Exit:
			return exitCode;
		}
	}
}
