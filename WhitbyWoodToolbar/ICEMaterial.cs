using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhitbyWoodToolbar
{
    public abstract class ICEMaterial : ViewModelBase
    {
        public abstract string Name { get; }
        public abstract double CarbonDensity { get;  }
        public ICECategory Category { get; protected set; }
        public ElementCarbonVM Parent { get; protected set; }

        public override string ToString()
        {
            return Name + ": " + CarbonDensity;
        }

        public static List<string> MaterialTypes = new List<string> { "None", "Concrete", "Steel", "Timber" };

        public static ICEMaterial CreateMaterial(string material, ElementCarbonVM parent)
        {
            switch (material)
            {
                case "None":
                    return new ICEMaterialNone(parent);
                case "Concrete":
                    return new ICEMaterialConcrete("RC32/40", "zero", 150, parent);
                case "Steel":
                    return new ICEMaterialSteel("General Steel", parent);
                case "Timber":
                    return new ICEMaterialTimber("General", true, parent);
                default:
                    return new ICEMaterialNone(parent);
            }
        }

        public static ICEMaterial CopyMaterial(ICEMaterial material, ElementCarbonVM parent)
        {
            switch (material.Category)
            {
                case ICECategory.None:
                    return new ICEMaterialNone(parent) ;
                case ICECategory.Steel:
                    var thisMat3 = material as ICEMaterialSteel;
                    return new ICEMaterialSteel(thisMat3.SteelMaterial, parent) ;
                case ICECategory.Concrete:
                    var thisMat = material as ICEMaterialConcrete;
                    return new ICEMaterialConcrete(thisMat.Grade, thisMat.Replacement, thisMat.ReinforcementDensity, parent) ;
                    break;
                case ICECategory.Timber:
                    var thisMat2 = material as ICEMaterialTimber;
                    return new ICEMaterialTimber(thisMat2.TimberMaterial, thisMat2.IncludeCarbonFromBiomass, parent) ;
                    break;
                default:
                    return new ICEMaterialNone(parent);
            }
        }
    }

    public enum ICECategory
    {
        None,
        Steel,
        Concrete,
        Timber
    }
}
