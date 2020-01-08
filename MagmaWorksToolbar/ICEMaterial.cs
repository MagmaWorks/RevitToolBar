using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaWorksToolbar
{
    public abstract class ICEMaterial : ViewModelBase
    {
        public abstract string Name { get; }
        public abstract double CarbonDensity { get;  }
        public abstract double SequesteredCarbonDensity { get;}
        public ICECategory Category { get; protected set; }
        public ElementCarbonVM Parent { get; protected set; }
        public string Info
        {
            get
            {
                return ICENotes[Category];
            }
        }

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

        public static Dictionary<ICECategory, string> ICENotes = new Dictionary<ICECategory, string>
        {
            { ICECategory.None,""},
            {
                ICECategory.Steel,
                "Please read the recycling methodology guide (Annex on recycling methods) before using this data, " +
                "which also contains guidance on end of life issues for steel. The above data is 'cradle to gate', " +
                "which excludes the important end of life stage (see Annex on recycling methods). The majority of this " +
                "data has been derived from the World Steel Association (Formerly International Iron & Steel Institute " +
                "[IISI]) life cycle inventory (LCI) data, which is the most complete and detailed steel LCI to date " +
                "and can be obtained free of charge from the IISI website (www.worldsteel.org).  Some of the IISI " +
                "data has been modified to fit within the ICE framework and methodology (e.g. converted to Gross " +
                "Calorific Value). It should be noted that the data for 'primary steel' is a purely hypothetical 100% " +
                "primary steel, this enables the recycled content approach to be easily implemented. In practise all " +
                "steel contains at least a small recycled content, even if sourced from a 'primary production route' " +
                "(Blast Furnace), on average blast furnace steel has a recycled content of approx 13% (e.g. general " +
                "steel @13% recycled content = BF route = 31 MJ/kg). On the other hand a 100% recycled steel is " +
                "realistic. Only steel PRODUCTION WITHIN the EU 27 countries may apply the EU 27 3-year average " +
                "recycled content of 59%. If applying this recycled content a 'rest of the world' recycled content " +
                "should be applied to non-EU 27 steel (for consistency within the same project), the 3-year average " +
                "ROTW recycled content is 35.5%. Alternatively the 3-year world average recycled content of 39% may " +
                "be applied for all steel products, but this cannot be mixed with the EU 27 average within the same " +
                "project. For further guidance please see Annex on recycling methods. There is now new data from " +
                "Worldsteel, which updates the LCI study to 2010. This data was not used here because we were not " +
                "able to process the data in time (and the Worldsteel methodology report was still being finished). " +
                "Readers with a strong interest in steel are advised to look at the detailed data from Worldsteel, " +
                "which is available through their website."
            },
            {
                ICECategory.Concrete,
                "The values of embodied carbon all exclude the re-carbonation of concrete in use, which is " +
                "application dependent. The majority of these concrete values were taken from the University of " +
                "Bath's ICE Cement, Mortar and Concrete Model.  It operates using the quantities of constituent " +
                "material inputs and an additional energy requirement of plant operations, transport of constituents " +
                "and a small allowance for mixing waste.  As a result these values are dependent upon the selected " +
                "coefficients of embodied energy and carbon of cement, sand and aggregates, which are the main " +
                "constituent materials for concrete. It may appear that concrete has a confusing array of options, " +
                "but it is worth determining the strength class or preferably mix of concrete (particularly cement " +
                "content) used within a project. Even for a specified strength class of concrete the cement content " +
                "can vary significantly.  If none of the descriptions or comments above help you may wish to apply " +
                "the above 'general' value, which is for a typical concrete mix.  But in doing so (and in an extreme " +
                "case) you may inadvertently add up to +/-50% additional error bars to your concrete results. Note: " +
                "the suggested possible uses of each strength class of concrete is a rough guide only."
            },
            {
                ICECategory.Timber,
                "Of all the major building materials timber still presents the most difficulties to the ICE database, " +
                "there are a number of reasons for this. These include a lack of high quality and detailed studies on " +
                "timber within the UK and EU. The highest quality studies are possibly the CORRIM studies (see references " +
                "88, 150) but these are North American studies and the UK consumes very little timber from this region. " +
                "Other factors include a high element of natural variation, which may include, for example, variations " +
                "in the moisture content of the trees, variations in the consumption of total energy to manufacture the " +
                "same timber product, and variations in the fuel mix. The latter is particularly significant to timber " +
                "manufacturing. Timber off-cuts are often burnt in a furnace to provide energy (normally to dry the timber " +
                "in a kiln). Large variations in the proportion of embodied energy from this biomass fuel give the data a " +
                "larger uncertainty than normal. This is also significant because the use of biomass as a fuel may " +
                "sometimes be assumed to be carbon neutral. The newest ICE data separates the embodied carbon emissions " +
                "derived from fossil fuels and that from biomass. The two numbers together give the total carbon released " +
                "if the biomass cannot be considered to be carbon neutral (i.e. if the timber is not from a sustainably " +
                "managed forest). If the timber is from a sustainably managed forest it is easier to justify the carbon " +
                "neutrality of burning biomass fuels. In this case the embodied carbon may be taken as the fossil fuel " +
                "derived carbon only (i.e. the first carbon number). None of the ICE data include the effects of carbon " +
                "sequestration during the growing of the trees or the biogenic carbon storage within the timber itself. " +
                "The inclusion or exclusion of sequestered carbon is a complex discussion.  The present authors do not " +
                "believe that this data should be included in the data for cradle to gate. Without including the end of " +
                "life stage it is difficult to justify."
            }
        };
    }

    public enum ICECategory
    {
        None,
        Steel,
        Concrete,
        Timber
    }
}
