using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Wire
    {
        public string Id;
        public string ParentId;
        public Coords Point1, Point2;

        public Wire(XmlNode node)
        {
            Point1 = new Coords();
            Point2 = new Coords();
            XmlNodeList children = node.ChildNodes;
            foreach (XmlNode child in children)
            {
                switch (child.Name)
                {
                    case "lc:WireId":
                        Id = child.InnerText;
                        break;
                    case "lc:LogicalCircuitId":
                        ParentId = child.InnerText;
                        break;
                    case "lc:X1":
                        Point1.x = int.Parse(child.InnerText);
                        break;
                    case "lc:X2":
                        Point2.x = int.Parse(child.InnerText);
                        break;
                    case "lc:Y1":
                        Point1.y = int.Parse(child.InnerText);
                        break;
                    case "lc:Y2":
                        Point2.y = int.Parse(child.InnerText);
                        break;
                    default:
                        Console.WriteLine("Wire Constructor: Unknown Element Type -- Ignoring");
                        break;
                }
            }
            if (Id == null || ParentId == null || Point1.x == -1000 || Point2.x == -1000 || Point1.y == -1000 || Point2.y == -1000)
            {
                throw new Exception("Wire Constructor: An invalid Pin element has been detected in the save file, please check your project and try again!");
            }
        }
    }
}
