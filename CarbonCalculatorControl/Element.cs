using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CarbonMaterials;
using Newtonsoft;
using Newtonsoft.Json;

namespace CarbonCalculator
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Element
    {
        [JsonProperty(PropertyName = "Quantity")]
        double _tempQuant { set { _volume = value; } }

        [JsonProperty(PropertyName = "Volume")]
        double _volume;
        double Volume { get { return _volume; }  set { _volume = value; } }

        [JsonProperty(PropertyName = "Area")]
        double _area;
        double Area { get { return _area; } set { _area = value; } }

        [JsonProperty(PropertyName = "Length")]
        double _length;
        double Length { get { return _length; } set { _length = value; } }

        public double Quantity
        {
            get
            {
                switch (SpatialDimensions)
                {
                    case Measurement.Volume:
                        return _volume;
                    case Measurement.Area:
                        return _area;
                    case Measurement.Length:
                        return _length;
                    case Measurement.Item:
                        return 1;
                    default:
                        return double.NaN;
                }
            }

            set
            {
                if (!VolumeIsReadOnly)
                    switch (SpatialDimensions)
                    {
                        case Measurement.Volume:
                            _volume = value;
                            break;
                        case Measurement.Area:
                            _area = value;
                            break;
                        case Measurement.Length:
                            _length = value;
                            break;
                        case Measurement.Item:
                            break;
                        default:
                            break;
                    }
            }
        }

        [JsonProperty]
        public bool VolumeIsReadOnly { get; private set; }

        [JsonProperty(PropertyName = "Filters")]
        string[] _filters; 

        public string[] Filters { get { return _filters; } }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string UniqueID { get; private set; }

        [JsonProperty]
        public GWPMaterialSet Material { get; set; }

        [JsonProperty]
        public Measurement SpatialDimensions { get; set; } = Measurement.Volume;

        [JsonConstructor]
        private Element()
        {

        }

        public Element(string name, double vol, string uniqueID, params string[] filters)
        {
            _volume = vol;
            SpatialDimensions = Measurement.Volume;
            Name = name;
            _filters = filters;
            VolumeIsReadOnly = false;
            UniqueID = uniqueID;
        }

        public Element(string name, double vol, double area, string uniqueID, params string[] filters)
        {
            _volume = vol;
            _area = area;
            SpatialDimensions = Measurement.Volume;
            Name = name;
            _filters = filters;
            VolumeIsReadOnly = false;
            UniqueID = uniqueID;
        }

        //public Element ElementFromRevit(string name, double vol, string uniqueID, params string[] filters)
        //{
        //    Element returnElement = new Element(name, vol, uniqueID, filters);
        //    returnElement.VolumeIsReadOnly = true;
        //    return returnElement;
        //}

        public double A1toA3 => Material.A1toA3 * Quantity;
        public double A4 => Material.A4 * Quantity;
        public double A5 => Material.A5 * Quantity;
        public double B1 => Material.B1 * Quantity;
        public double B2 => Material.B2 * Quantity;
        public double B3 => Material.B3 * Quantity;
        public double B4 => Material.B4 * Quantity;
        public double B5 => Material.B5 * Quantity;
        public double B6 => Material.B6 * Quantity;
        public double B7 => Material.B7 * Quantity;
        public double C1 => Material.C1 * Quantity;
        public double C2 => Material.C2 * Quantity;
        public double C3 => Material.C3 * Quantity;
        public double C4 => Material.C4 * Quantity;        
        public double TotalAtoC => Material.TotalAtoC * Quantity;
        public double TotalA => Material.TotalA * Quantity;
        public double TotalB => Material.TotalB* Quantity;
        public double TotalC => Material.TotalC * Quantity;
    }
}
