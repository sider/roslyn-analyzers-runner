using System;

namespace Test
{
	public static class HelloWorldSjis
	{
		public static void こんにちは()
		{
			try
			{
				Console.WriteLine("こんにちは 世界! shift-jis");
			}
			finally
			{
				throw;
			}
		}
	}
}
