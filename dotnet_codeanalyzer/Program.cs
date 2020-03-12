﻿using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace Sider.CodeAnalyzers
{
	public static class Program
	{
		private class Options
		{
			[Option()]
			public IEnumerable<string> Analyzers { get; set; }

			[Option()]
			public IEnumerable<string> Targets { get; set; }
		}

		public static int Main(string[] args)
		{
			int exitCode;

			var parseResult = (ParserResult<Options>)Parser.Default.ParseArguments<Options>(args);

			if (parseResult.Tag == ParserResultType.NotParsed)
			{
				var helpText = HelpText.AutoBuild(parseResult);
				Console.Error.WriteLine(helpText);
				exitCode = -1;
				goto Exit;
			}

			try
			{
				var parsed = (Parsed<Options>)parseResult;
				CodeAnalyzer.Diagnose(parsed.Value.Analyzers, parsed.Value.Targets);
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
