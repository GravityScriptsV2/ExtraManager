﻿namespace ExtraManager.Engine.Data
{
    [XmlRoot("Vehicles")]
    public class VehicleSet
    {
        [XmlElement("Vehicle")]
        public List<VehicleData> VehicleList { get; set; }
    }
}
