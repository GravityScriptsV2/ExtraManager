﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExtraManager.Models
{
    [XmlRoot("Vehicles")]
    public class VehicleSet
    {
        [XmlElement("Vehicle")]
        public List<VehicleData> VehicleList { get; set; }
    }
}