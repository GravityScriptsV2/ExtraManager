using System.IO;

namespace ExtraManager.Engine.Data
{
    public class VehicleDataManager
    {
        #region File Paths

        private static readonly string VehicleXmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/plugins/ExtraManager", "VehicleExtrasConfig.xml");
        private static readonly string ModkitsXmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/plugins/ExtraManager", "VehicleModkitsConfig.xml");

        #endregion

        #region Vehicle Data Loading

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

        #region Modkit Data Loading

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

        #region Vehicle Customization

        public static void ApplyVehicleCustomization(Vehicle vehicle, Modkit data)
        {
            int tintValue = data.WindowTint.HasValue ? data.WindowTint.Value : 0;

            NativeFunction.CallByHash<int>(0x57C51E6BAD752696, vehicle, tintValue); // SET_VEHICLE_WINDOW_TINT
        }

        public static void ConfigureModkit(Vehicle vehicle, VehicleData vehicleData, ModkitSet modkitSet)
        {
            var modkitConfig = vehicleData.ModKit.HasValue
                ? modkitSet.Modkits.FirstOrDefault(m => m.Id == vehicleData.ModKit.Value)
                : null;

            if (modkitConfig != null)
            {
                if (NativeFunction.CallByHash<int>(0x33F2E3FE70EAAE1D, vehicle) > 0) // GET_NUM_MOD_KITS
                {
                    NativeFunction.CallByHash<int>(0x1F2AA07F00B3217A, vehicle, 0); // SET_VEHICLE_MOD_KIT
                }
                ApplyVehicleCustomization(vehicle, modkitConfig);
            }
        }

        public static void ConfigureExtras(Vehicle vehicle, VehicleData vehicleData)
        {
            for (int i = 1; i <= 14; i++)
            {
                bool isExtraEnabled = NativeFunction.CallByHash<bool>(0xD2E6822DBFD6C8BD, vehicle, i);

                // Check if the extra is in the XML configuration
                var extraConfig = vehicleData.Extras.FirstOrDefault(ec => ec.Id == i);
                bool shouldEnable = extraConfig != null ? extraConfig.Enabled : false;

                // If the current state doesn't match the configuration, update it
                if (isExtraEnabled != shouldEnable)
                {
                    NativeFunction.CallByHash<int>(0x7EE3A3C5E4A40CC9, vehicle, i, !shouldEnable);
                }
            }
        }

        #endregion
    }
}
