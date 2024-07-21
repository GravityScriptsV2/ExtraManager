using ExtraManager.Handlers;
using System.IO;
using System.Xml.Serialization;

namespace ExtraManager.Models
{
    public class VehicleDataManager
    {
        private static readonly string XmlFilePath = Path.Combine(ConfigHandler.PluginPath, "VehicleExtrasConfig.xml");

        public static VehicleSet LoadVehicles()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(VehicleSet));
            VehicleSet vehicles;

            using (FileStream fs = new FileStream(XmlFilePath, FileMode.Open))
            {
                vehicles = (VehicleSet)serializer.Deserialize(fs);
            }

            return vehicles;
        }
    }
}
