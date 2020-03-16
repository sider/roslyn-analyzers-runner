using System;
using Foo.Bar;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Test
{
	public class Class2
	{
		[DllImport("User32.Dll", EntryPoint = "SetWindowText")]
		public static extern void SetWindowText(int hwnd, String text);

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
