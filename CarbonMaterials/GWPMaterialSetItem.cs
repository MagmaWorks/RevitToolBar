using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonMaterials
{
    public class GWPMaterialSetItem
    {
        public double Proportion { get; set; }
        public GWPMaterial Material { get; set; }

        public double A1 => Material.A1.Value * Proportion;
        public double A2 => Material.A2.Value * Proportion;
        public double A3 => Material.A3.Value * Proportion;
        public double A4 => Material.A4.Value * Proportion;
        public double A5 => Material.A5.Value * Proportion;
        public double A1toA3 => Material.A1toA3.Value * Proportion;
        public double B1 => Material.B1.Value * Proportion;
        public double B2 => Material.B2.Value * Proportion;
        public double B3 => Material.B3.Value * Proportion;
        public double B4 => Material.B4.Value * Proportion;
        public double B5 => Material.B5.Value * Proportion;
        public double B6 => Material.B6.Value * Proportion;
        public double B7 => Material.B7.Value * Proportion;
        public double C1 => Material.C1.Value * Proportion;
        public double C2 => Material.C2.Value * Proportion;
        public double C3 => Material.C3.Value * Proportion;
        public double C4 => Material.C4.Value * Proportion;

        public double TotalA => A1toA3 + A4 + A5;
        public double TotalB => B1 + B2 + B3 + B4 + B5 + B6 + B7;
        public double TotalC => C1 + C2 + C3 + C4;
        public double TotalAtoC => TotalA + TotalB + TotalC;
    }
}
