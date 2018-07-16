using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuitToHDLConverter
{
    public static class HDLWriter
    {
        public static void WriteDocument(List<LogicalCircuit> circuits)
        {
            string path = @"C:\Users\Catalyse\Desktop\HDLTest\";
            string fileString = "";

            foreach(var circuit in circuits)
            {
                fileString += "//This file was created with LogicCircuitToHDLConverter" + Environment.NewLine;
                fileString += "//Created by: Taylor May" + Environment.NewLine;
                fileString += "//GitHub: https://github.com/Catalyse" + Environment.NewLine;
                fileString += "//FileName: " + /*filename +*/ Environment.NewLine;
                fileString += "" + Environment.NewLine;
                fileString += "CHIP " + circuit.Notation + " { " + Environment.NewLine;
                //Write IN
                fileString = WriteINBlock(circuit, fileString);
                //Write OUT
                fileString = WriteOutBlock(circuit, fileString);
                //Write Parts
                fileString += "\tPARTS:" + Environment.NewLine;
                fileString = WriteParts(circuit, fileString);
                fileString += "" + Environment.NewLine;
                fileString += "" + Environment.NewLine;
                fileString += "" + Environment.NewLine;
            }
        }

        private static string WriteParts(LogicalCircuit circuit, string fileString)
        {
            throw new NotImplementedException();
        }

        private static string WriteINBlock(LogicalCircuit circuit, string fileString)
        {
            bool startedINBlock = false;
            foreach (var item in circuit.circuits)
            {
                if (item.GetType() == typeof(Pin))
                {
                    Pin pin = (Pin)item;
                    if (pin.Type == PinType.Input)
                    {
                        if (!startedINBlock)
                        {
                            startedINBlock = true;
                            fileString += "\tIN " + pin.Name;
                        }
                        else
                        {
                            fileString += ", " + pin.Name;
                        }
                    }
                }
            }
            if (startedINBlock)//As long as the block has been started we add a semicolon and a newline to close out the IN block;
            {
                fileString += ";" + Environment.NewLine;
            }
            return fileString;
        }

        private static string WriteOutBlock(LogicalCircuit circuit, string fileString)
        {
            bool startedOutBlock = false;
            foreach (var item in circuit.circuits)
            {
                if (item.GetType() == typeof(Pin))
                {
                    Pin pin = (Pin)item;
                    if (pin.Type == PinType.Output)
                    {
                        if (!startedOutBlock)
                        {
                            startedOutBlock = true;
                            fileString += "\tOUT " + pin.Name;
                        }
                        else
                        {
                            fileString += ", " + pin.Name;
                        }
                    }
                }
            }
            if (startedOutBlock)//As long as the block has been started we add a semicolon and a newline to close out the OUT block;
            {
                fileString += ";" + Environment.NewLine;
            }
            return fileString;
        }
    }
}
