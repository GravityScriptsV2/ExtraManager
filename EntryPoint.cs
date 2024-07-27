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
        private static ModkitSet modkitSet = VehicleDataManager.LoadModkits();

        #endregion

        #region Plugin Lifecycle

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

        #endregion

        #region Dependency Check

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

        #endregion

        #region Plugin Initialization

        private static void InitializePlugin()
        {
            GameFiber.StartNew(delegate
            {
                while (true)
                {
                    GameFiber.Yield();
                    GameFiber.Sleep(100);
                    ProcessVehicles();
                }
            });
        }

        private static void ProcessVehicles()
        {
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

                                VehicleDataManager.ConfigureModkit(vehicle, vehicleData, modkitSet);
                                VehicleDataManager.ConfigureExtras(vehicle, vehicleData);

                                processedVehicleHandles.Add(vehicleHandle);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Console Commands

        [ConsoleCommand]
        private static void Command_GetCurrentVehicleName()
        {
            Vehicle currentVehicle = Game.LocalPlayer.Character.CurrentVehicle;

            if (currentVehicle != null)
            {
                uint modelHash = (uint)currentVehicle.Model.Hash;
                IntPtr modelNamePtr = NativeFunction.CallByHash<IntPtr>(0xB215AAC32D25D019, modelHash);

                if (modelNamePtr != IntPtr.Zero)
                {
                    string modelName = Marshal.PtrToStringAnsi(modelNamePtr).ToUpper();

                    if (!string.IsNullOrEmpty(modelName))
                    {
                        Game.LogTrivial("Vehicle Model Name: " + modelName);
                    }
                }
            }
        }

        #endregion
    }
}
