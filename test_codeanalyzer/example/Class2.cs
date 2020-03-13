using System;
using Foo.Bar;
using System.Threading.Tasks;

namespace Test
{
	public class Class2
	{
		private void TestException()
		{
			try
			{
				var a = 0;
			}
			finally
			{
				throw new Exception();
			}
		}

		[Flags]
		public enum TestFlag
		{
			AAAAA = 1,
			BBBBB = 2,
		}
	
}
