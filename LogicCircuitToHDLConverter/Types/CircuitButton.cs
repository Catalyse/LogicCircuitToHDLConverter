using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class CircuitButton : CircuitBase
    {
        public string Notation;
        public bool IsToggle = false;//IsToggle is false by default, and the element is not saved unless true.
        public int Height = 2;//2 is the smallest it can be, and the implied size unless specified.
        public int Width = 2;//2 is the smallest it can be, and the implied size unless specified.
        public PinSide Side = PinSide.Right;//Right is the default for the CircuitButton, and the implied side unless specified otherwise.

        public CircuitButton(XmlNode node)
        {
            XmlNodeList children = node.ChildNodes;
            foreach (XmlNode child in children)
            {
                switch (child.Name)
                {
                    case "lc:CircuitButtonId":
                        Id = child.InnerText;
                        break;
                    case "lc:CircuitId":
                        ParentId = child.InnerText;
                        break;
                    case "lc:Notation":
                        Notation = child.InnerText;
                        break;
                    case "lc:IsToggle":
                        if(child.InnerText == "True")
                        {
                            IsToggle = true;
                        }
                        else
                        {
                            IsToggle = false;
                        }
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
                            throw new Exception("CircuitButton Constructor: An invalid PinSide element has been detected in the save file, please check your project and try again!");
                        }
                        break;
                    case "lc:Width":
                        Width = int.Parse(child.InnerText);
                        break;
                    case "lc:Height":
                        Height = int.Parse(child.InnerText);
                        break;
                    default:
                        Console.WriteLine("CircuitButton Constructor: Unknown Element Type -- Ignoring");
                        break;
                }
            }
            if (Id == null)
            {
                throw new Exception("CircuitButton Constructor: An invalid CircuitButton element has been detected in the save file, please check your project and try again!");
            }
        }
    }
}
