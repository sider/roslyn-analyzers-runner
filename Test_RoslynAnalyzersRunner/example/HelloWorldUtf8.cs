using System;

namespace Test
{
	public static class HelloWorldUtf8
	{
		public static void こんにちは()
		{
			try
			{
				Console.WriteLine("こんにちは 世界! utf-8");
			}
			finally
			{
				throw;
			}
		}
	}
}