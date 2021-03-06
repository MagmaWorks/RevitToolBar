﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarbonMaterials;

namespace CarbonCalculator
{
    public class Element
    {
        double _volume;
        public double Volume
        {
            get
            {
                return _volume;
            }

            set
            {
                if (!VolumeIsReadOnly)
                    _volume = value;
            }
        }
        public bool VolumeIsReadOnly { get; private set; }
        public string[] Filters { get; }
        public string Name { get; }
        public string UniqueID { get; }
        public GWPMaterialSet Material { get; set; }
        public Element(string name, double vol, string uniqueID, params string[] filters)
        {
            _volume = vol;
            Name = name;
            Filters = filters;
            VolumeIsReadOnly = false;
            UniqueID = uniqueID;
        }
        public Element ElementFromRevit(string name, double vol, string uniqueID, params string[] filters)
        {
            Element returnElement = new Element(name, vol, uniqueID, filters);
            returnElement.VolumeIsReadOnly = true;
            return returnElement;
        }

        public double A1toA3 => Material.A1toA3 * Volume;
        public double A4 => Material.A4 * Volume;
        public double A5 => Material.A5 * Volume;
        public double B1 => Material.B1 * Volume;
        public double B2 => Material.B2 * Volume;
        public double B3 => Material.B3 * Volume;
        public double B4 => Material.B4 * Volume;
        public double B5 => Material.B5 * Volume;
        public double B6 => Material.B6 * Volume;
        public double B7 => Material.B7 * Volume;
        public double C1 => Material.C1 * Volume;
        public double C2 => Material.C2 * Volume;
        public double C3 => Material.C3 * Volume;
        public double C4 => Material.C4 * Volume;        
        public double TotalAtoC => Material.TotalAtoC * Volume;
        public double TotalA => Material.TotalA * Volume;
        public double TotalB => Material.TotalB* Volume;
        public double TotalC => Material.TotalC * Volume;



    }
}
