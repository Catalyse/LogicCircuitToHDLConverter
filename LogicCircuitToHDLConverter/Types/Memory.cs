using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Memory : CircuitBase
    {
        public bool Writable;
        public int AddressWidth = -1000;
        public int DataWidth = -1000;

        public Memory(XmlNode node)
        {
            XmlNodeList children = node.ChildNodes;
            foreach (XmlNode child in children)
            {
                switch (child.Name)
                {
                    case "lc:MemoryId":
                        Id = child.InnerText;
                        break;
                    case "lc:CircuitId":
                        ParentId = child.InnerText;
                        break;
                    case "lc:Writable":
                        if(child.InnerText == "True")
                        {
                            Writable = true;
                        }
                        else
                        {
                            Writable = false;
                        }
                        break;
                    case "lc:AddressBitWidth":
                        AddressWidth = int.Parse(child.InnerText);
                        break;
                    case "lc:DataBitWidth":
                        DataWidth = int.Parse(child.InnerText);
                        break;
                    default:
                        Console.WriteLine("Memory Constructor: Unknown Element Type -- Ignoring");
                        break;
                }
            }
            if (Id == null || AddressWidth == -1000 || DataWidth == -1000)
            {
                throw new Exception("Memory Constructor: An invalid Memory element has been detected in the save file, please check your project and try again!");
            }
        }
    }
}
