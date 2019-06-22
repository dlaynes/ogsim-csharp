using System;

namespace OgSim.Misc
{
	public class Debugger
	{
		public Debugger()
		{
        }

        public static void ConsoleLog(object item) {
            var serialized = ObjectDumper.Dump(item);

            Console.WriteLine(serialized);
        }
	}
}
