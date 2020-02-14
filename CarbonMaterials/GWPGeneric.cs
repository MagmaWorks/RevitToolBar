using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CarbonMaterials
{
    public class GWPGeneric : GWPMaterial
    {
        string _name;
        public override string Name => _name;

        public override double SequesteredCarbonDensity => 0;

        public override GWPMaterialType GWPMaterialType => GWPMaterialType.General;

        GWPValue _a1toa3 = new GWPValue();
        GWPValue _a4 = new GWPValue();
        GWPValue _a5 = new GWPValue();
        GWPValue _b1 = new GWPValue();
        GWPValue _b2 = new GWPValue();
        GWPValue _b3 = new GWPValue();
        GWPValue _b4 = new GWPValue();
        GWPValue _b5 = new GWPValue();
        GWPValue _b6 = new GWPValue();
        GWPValue _b7 = new GWPValue();
        GWPValue _c1 = new GWPValue();
        GWPValue _c2 = new GWPValue();
        GWPValue _c3 = new GWPValue();
        GWPValue _c4 = new GWPValue();

        public override GWPValue A1toA3 => _a1toa3;
        public override GWPValue A4 => _a4;
        public override GWPValue A5 => _a5;
        public override GWPValue B1 => _b1;
        public override GWPValue B2 => _b2;
        public override GWPValue B3 => _b3;
        public override GWPValue B4 => _b4;
        public override GWPValue B5 => _b5;
        public override GWPValue B6 => _b6;
        public override GWPValue B7 => _b7;
        public override GWPValue C1 => _c1;
        public override GWPValue C2 => _c2;
        public override GWPValue C3 => _c3;
        public override GWPValue C4 => _c4;

        string _linkReference;

        public string LinkReference => _linkReference;

        public GWPGeneric()
        {
            copyMaterialFrom(getFromEPDLibrary("KLH CLT"));
        }

        public GWPGeneric(string name, string link, double a1toa3, double a4, double a5, double b1, double c1, double c2, double c3, double c4)
        {
            _a1toa3 = new GWPValue(a1toa3);
            _a4 = new GWPValue(a4);
            _a5 = new GWPValue(a5);
            _b1 = new GWPValue(b1);
            _c1 = new GWPValue(c1);
            _c2 = new GWPValue(c2);
            _c3 = new GWPValue(c3);
            _c4 = new GWPValue(c4);
            _linkReference = link;
            _name = name;
        }

        public override void copyMaterialFrom(GWPMaterial material)
        {
            var m = material as GWPGeneric;
            _a1toa3 = m.A1toA3;
            _a4 = m.A4;
            _a5 = m.A5;
            _b1 = m.B1;
            _b2 = m.B2;
            _b3 = m.B3;
            _b4 = m.B4;
            _b5 = m.B5;
            _b6 = m.B6;
            _b7 = m.B7;
            _c1 = m.C1;
            _c2 = m.C2;
            _c3 = m.C3;
            _c4 = m.C4;
            _name = m.Name;
            _linkReference = m.LinkReference;
        }

        public override GWPMaterial getCopy()
        {
            return new GWPGeneric(this.Name, this.LinkReference, this.A1toA3.Value, this.A4.Value, this.A5.Value, this.B1.Value, this.C1.Value, this.C2.Value, this.C3.Value, this.C4.Value);
        }

        public static GWPGeneric getFromEPDLibrary(string product)
        {
            return EPDLibrary[product];
        }

        static List<string> epds;
        public static List<string> EPDs
        {
            get
            {
                if (epds == null)
                {
                    epds = EPDLibrary.Keys.ToList();
                }
                return epds;
            }
        }

        static Dictionary<string, GWPGeneric> EPDLibrary = new Dictionary<string, GWPGeneric>
        {
            { "KLH CLT", new GWPGeneric("KLH CLT", @"http://www.klhuk.com/media/33498/klh_component%20catalogue%20for%20environmental%20product%20declaration.pdf", -601.29, 70.38, 20.69, 0,9.28,4.02, 808.39, 0) },
            { "Generic Readymix", new GWPGeneric("Generic Ready Mix concrete", @"https://www.concretecentre.com/TCC/media/TCCMediaLibrary/PDF%20attachments/Generic-ready-mixed-concrete.pdf", 246, 2.01, 0.19, -19.9, -0.84, 8.27, -18.9, 1.98) }
        };
    }
}
