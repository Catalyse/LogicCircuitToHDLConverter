using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LogicCircuitToHDLConverter
{
    public class Sensor : CircuitBase
    {
        public Sensor(XmlNode node)
        {
            XmlNodeList children = node.ChildNodes;
            foreach (XmlNode child in children)
            {
                switch (child.Name)
                {
                    case "lc:SensorId":
                        Id = child.InnerText;
                        break;
                    case "lc:CircuitId":
                        ParentId = child.InnerText;
                        break;
                    default:
                        Console.WriteLine("Sensor Constructor: Unknown Element Type -- Ignoring");
                        break;
                }
            }
            if (Id == null)
            {
                throw new Exception(@"Sensor Constructor: An invalid Constant element has been detected in the save file, please check your project and try again!
                                        The converter does NOT support Sensors to HDL, so removing them is recommended as they are not parsed");
            }
        }
    }
}
