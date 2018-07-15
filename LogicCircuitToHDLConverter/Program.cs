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
            Converter.ParseXMLDocument();
            Console.Write("Press any key to close the window.");
            Console.ReadKey();
        }
    }
}
