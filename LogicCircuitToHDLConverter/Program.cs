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
            Console.WriteLine("Logic Circuit to HDL Converter");
            Console.WriteLine("By Taylor May");
            Console.Write("Please enter the full path of your LogicCircuit save file: ");
            List<LogicalCircuit> test = Converter.ParseXMLDocument(Console.ReadLine());
            HDLWriter.WriteDocument(test);
            Console.Write("Press any key to close the window.");
            Console.ReadKey();
        }
    }
}
