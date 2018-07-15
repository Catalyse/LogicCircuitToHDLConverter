using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public enum PinType
    {
        Invalid = 0,
        Output = 1,
        Input = 2
    }

    public enum PinSide
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3
    }

    public class Pin : CircuitBase
    {
        public string Name;
        public PinType Type = PinType.Invalid;
        public PinSide Side = PinSide.Left;//Left is the default.  If there is no PinSide element the implication is that it is on the left side.

        public Pin(XmlNode node)
        {
            XmlNodeList children = node.ChildNodes;
            foreach (XmlNode child in children)
            {
                switch (child.Name)
                {
                    case "lc:PinId":
                        Id = child.InnerText;
                        break;
                    case "lc:CircuitId":
                        ParentId = child.InnerText;
                        break;
                    case "lc:PinType":
                        if(child.InnerText == "Output")
                        {
                            Type = PinType.Output;
                        }
                        else
                        {
                            Type = PinType.Input;
                        }
                        break;
                    case "lc:PinSide":
                        if(child.InnerText == "Left")
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
                        else if(child.InnerText == "Bottom")
                        {
                            Side = PinSide.Bottom;
                        }
                        else
                        {
                            throw new Exception("Pin Constructor: An invalid PinSide element has been detected in the save file, please check your project and try again!");
                        }
                        break;
                    case "lc:Name":
                        Name = child.InnerText;
                        break;
                    default:
                        Console.WriteLine("Pin Constructor: Unknown Element Type -- Ignoring");
                        break;
                }
            }
            if(Type == PinType.Invalid)
            {
                Type = PinType.Input;
            }
            if (Name == null || Id == null)
            {
                throw new Exception("Pin Constructor: An invalid Pin element has been detected in the save file, please check your project and try again!");
            }
        }
    }
}
