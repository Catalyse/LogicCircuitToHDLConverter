using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Constant : CircuitBase
    {
        public int BitWidth = 1;//1 is the default and implied value unless specified.
        public int Value = 0;//0 is the default and implied value unless specified.
        public PinSide Side = PinSide.Right;//Right is the default for the CircuitButton, and the implied side unless specified otherwise.

        public Constant(XmlNode node)
        {
            XmlNodeList children = node.ChildNodes;
            foreach (XmlNode child in children)
            {
                switch (child.Name)
                {
                    case "lc:ConstantId":
                        Id = child.InnerText;
                        break;
                    case "lc:CircuitId":
                        ParentId = child.InnerText;
                        break;
                    case "lc:BitWidth":
                        BitWidth = int.Parse(child.InnerText);
                        break;
                    case "lc:Value":
                        Value = int.Parse(child.InnerText);
                        break;
                    case "lc:PinSide":
                        if (child.InnerText == "Left")
                        {
                            Side = PinSide.Left;
                        }
                        else if (child.InnerText == "Top")
                        {
                            Side = PinSide.Top;
                        }
                        else if (child.InnerText == "Right")
                        {
                            Side = PinSide.Right;
                        }
                        else if (child.InnerText == "Bottom")
                        {
                            Side = PinSide.Bottom;
                        }
                        else
                        {
                            throw new Exception("Constant Constructor: An invalid PinSide element has been detected in the save file, please check your project and try again!");
                        }
                        break;
                    default:
                        Console.WriteLine("Constant Constructor: Unknown Element Type -- Ignoring");
                        break;
                }
            }
            if (Id == null)
            {
                throw new Exception("Constant Constructor: An invalid Constant element has been detected in the save file, please check your project and try again!");
            }
        }
    }
}
