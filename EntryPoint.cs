using ExtraManager.Models;
using Rage;
using Rage.Attributes;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: Plugin("ExtraManager", Author = "Venoxity Development", PrefersSingleInstance = true, ShouldTickInPauseMenu = true, SupportUrl = "https://discord.gg/jCEdAF8AQz")]
namespace ExtraManager
{
    public class EntryPoint
    {
        #region Fields

        private static List<VehicleData> vehicleList = VehicleDataManager.LoadVehicles().VehicleList;
        private static HashSet<uint> processedVehicleHandles = new HashSet<uint>();

        #endregion

        public static void Main()
        {
            if (CheckDependencies())
            {
                InitializePlugin();
            }
            else
            {
                Game.DisplayNotification("new_editor", "warningtriangle", "ExtraManager", "~r~Initialization Failure", "~y~ExtraManager could not start. You are missing required libraries.");
            }
        }

        #region Initialization

        private static bool CheckDependencies()
        {
            foreach (var dependency in UtilityConstants.Dependencies)
            {
                if (!IsAssemblyAvailable(dependency.Name, dependency.Version))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsAssemblyAvailable(string assemblyName, string version)
        {
            try
            {
                AssemblyName assemblyName2 = AssemblyName.GetAssemblyName(AppDomain.CurrentDomain.BaseDirectory + "/" + assemblyName);
                if (assemblyName2.Version >= new Version(version))
                {
                    Game.LogTrivial($"ExtraManager dependency {assemblyName} is available ({assemblyName2.Version}).");
                    return true;
                }
                Game.LogTrivial($"ExtraManager dependency {assemblyName} does not meet minimum requirements ({assemblyName2.Version} < {version}).");
                return false;
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is BadImageFormatException)
            {
                Game.LogTrivial("ExtraManager dependency " + assemblyName + " is not available.");
                return false;
            }
        }

        private static void InitializePlugin()
        {
            GameFiber.StartNew(delegate
            {
                while (true)
                {
                    GameFiber.Yield();
                    GameFiber.Sleep(100);
                    Vehicle[] allVehicles = World.GetAllVehicles();
                    foreach (Vehicle vehicle in allVehicles)
                    {
                        GameFiber.Yield();
                        if (vehicle.IsValid())
                        {
                            uint modelHash = vehicle.Model.Hash;
                            IntPtr modelNamePtr = NativeFunction.CallByHash<IntPtr>(0xB215AAC32D25D019, modelHash);

                            if (modelNamePtr != IntPtr.Zero)
                            {
                                string modelName = Marshal.PtrToStringAnsi(modelNamePtr).ToUpper();

                                if (!string.IsNullOrEmpty(modelName))
                                {
                                    var vehicleData = vehicleList.FirstOrDefault(v => v.Name == modelName);
                                    if (vehicleData != null)
                                    {
                                        uint vehicleHandle = vehicle.Handle;

                                        if (processedVehicleHandles.Contains(vehicleHandle))
                                        {
                                            continue;
                                        }

                                        Game.LogTrivial($"Vehicle found: {modelName}");

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

                                        processedVehicleHandles.Add(vehicleHandle);
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        #endregion
    }
}
