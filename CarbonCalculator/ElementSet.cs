using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarbonCalculator
{
    public class ElementSet
    {
        public List<Element> Elements { get; }
        public string[] FilterNames { get; }

        public ElementSet(params string[] filters)
        {
            FilterNames = filters;
            Elements = new List<Element>();
        }
    }
}
