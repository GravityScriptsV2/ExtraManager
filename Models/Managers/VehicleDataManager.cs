using System;
using System.IO;
using System.Xml.Serialization;

namespace ExtraManager.Models
{
    public class VehicleDataManager
    {
        #region File Paths

        private static readonly string VehicleXmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/plugins/ExtraManager", "VehicleExtrasConfig.xml");
        private static readonly string ModkitsXmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/plugins/ExtraManager", "VehicleModkitsConfig.xml");

        #endregion

        #region Vehicle Data Methods

        public static VehicleSet LoadVehicles()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(VehicleSet));
            VehicleSet vehicles;

            using (FileStream fs = new FileStream(VehicleXmlFilePath, FileMode.Open))
            {
                vehicles = (VehicleSet)serializer.Deserialize(fs);
            }

            return vehicles;
        }

        #endregion

        #region Modkit Data Methods

        public static ModkitSet LoadModkits()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ModkitSet));
            ModkitSet modkits;

            using (FileStream fs = new FileStream(ModkitsXmlFilePath, FileMode.Open))
            {
                modkits = (ModkitSet)serializer.Deserialize(fs);
            }

            return modkits;
        }

        #endregion
    }
}
