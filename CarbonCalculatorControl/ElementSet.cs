﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CarbonCalculator
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ElementSet
    {
        [JsonProperty(PropertyName ="Elements")]
        List<Element> _elements;

        public List<Element> Elements
        {
            get
            {
                return _elements;
            }
        }

        [JsonProperty]
        public string[] FilterNames { get; private set; }

        [JsonProperty(Order = -2)]
        public List<CarbonMaterials.GWPMaterialSet> MaterialSets { get; private set; }

        public ElementSet(params string[] filters)
        {
            FilterNames = filters;
            _elements = new List<Element>();
            MaterialSets = new List<CarbonMaterials.GWPMaterialSet>
            {
                new CarbonMaterials.GWPMaterialSet("None"), 
                CarbonMaterials.GWPMaterialSet.GetDefaultGWPConcreteSetSeparateSteel(), 
                CarbonMaterials.GWPMaterialSet.GetDefaultGWPConcreteSet(), 
                CarbonMaterials.GWPMaterialSet.GetDefaultFoundationRC(), 
                CarbonMaterials.GWPMaterialSet.GetDefaultGlulam(), 
                CarbonMaterials.GWPMaterialSet.GetDefaultStructuralSteel()
            };
        }

        public void AddElement(Element elem)
        {
            if (elem.Material == null)
            {
                elem.Material = MaterialSets[0];
            }
            _elements.Add(elem);
        }
    }
}