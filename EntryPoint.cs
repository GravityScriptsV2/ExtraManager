using Common;
using Common.API;
using ExtraManager.Data;
using Rage;
using Rage.Attributes;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

[assembly: Plugin("ExtraManager", Author = "Venoxity Development", PrefersSingleInstance = true, ShouldTickInPauseMenu = true, SupportUrl = "https://discord.gg/jCEdAF8AQz")]
namespace ExtraManager
{
    public class EntryPoint : CommonPlugin
    {
        #region Fields

        private static List<VehicleData> vehicleList = VehicleDataManager.LoadVehicles().VehicleList;
        private static HashSet<uint> processedVehicleHandles = new HashSet<uint>();
        private static ModkitSet modkitSet = VehicleDataManager.LoadModkits();

        #endregion

        #region Plugin Lifecycle

        public static void Main()
        {
            DependencyManager.AddDependency("RageNativeUI.dll", "1.9.2.0");
            if (!DependencyManager.CheckDependencies()) return;

            InitializePlugin();
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
