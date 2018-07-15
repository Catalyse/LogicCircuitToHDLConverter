using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Splitter : CircuitBase
    {
        public int BitWidth;
        public PinSide SourcePinSide = PinSide.Right;//Right is the default for the SourcePinSide, and the implied side unless specified otherwise.

        public Splitter(XmlNode node)
        {
            XmlNodeList children = node.ChildNodes;
            foreach (XmlNode child in children)
            {
                switch (child.Name)
                {
                    case "lc:SplitterId":
                        Id = child.InnerText;
                        break;
                    case "lc:CircuitId":
                        ParentId = child.InnerText;
                        break;
                    case "lc:BitWidth":
                        BitWidth = int.Parse(child.InnerText);
                        break;
                    case "lc:Clockwise":
                        if(child.InnerText == "True")
                        {
                            SourcePinSide = PinSide.Left;
                        }
                        else
                        {
                            SourcePinSide = PinSide.Right;
                        }
                        break;
                    default:
                        Console.WriteLine("Splitter Constructor: Unknown Element Type -- Ignoring");
                        break;
                }
            }
            if (Id == null)
            {
                throw new Exception("Splitter Constructor: An invalid Splitter element has been detected in the save file, please check your project and try again!");
            }
        }
    }
}
