using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuitToHDLConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            List<LogicalCircuit> test = Converter.ParseXMLDocument();
            List<string> inputs = new List<string>
            {
                "a",
                "b",
                "c",
                "d"
            };
            var testString = test[0].gates[0].WriteGateHDL(inputs, "testOutput");
            Console.WriteLine(testString);
            Console.Write("Press any key to close the window.");
            Console.ReadKey();
        }
    }
}
