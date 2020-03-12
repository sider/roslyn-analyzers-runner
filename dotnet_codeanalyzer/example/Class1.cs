using System;
using System.Collections.Generic;
using System.Linq;
using Hoge.Foo.Bar;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Project1
{
	internal class A2 { }

	[Flags]
	public enum Test
	{
		AAAAA = 1,
		BBBBB = 2,
	}

	public enum TestEnum
	{
		AAA = 1,
		BBB = 2,
	}

	class Class1
	{
		public static void Main()
		{
                    AStaticClass.AMember = 123;
                    
			var a = 123;
			var b = 321;

			NewString();
			Mul(1, 3);
			var sub = Sub(a, 3);
			var add = Add(a, 3);

			System.Console.WriteLine($"sub; {sub}");
			System.Console.WriteLine($"add: {add}");

			TestEnum testEnum;
		}


		static string NewString()
		{
			return "aaaa";
		}
		static int Mul(int a, int b)
		{
			return a * b;
		}

		private void Add(int a, int b)
		{


			return a + b;


		}

		public static int Sub(int a, int b)
		{
			double c = (float)3.14;

			return a - b;



		}


		IllegalSyntax

		private BadThrow()
		{
			try
			{
				System.Console.WriteLine("test throw");
			}
			catch(Exception e)
			{
				System.Console.WriteLine(e);
			}
			finally
			{
				throw new Exception();

			}
		}
	}
}
