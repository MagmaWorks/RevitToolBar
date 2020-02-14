using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarbonMaterials;

namespace CarbonCalculator
{
    public class ElementVM : ViewModelBase
    {
        Element _element;
        ModelVM _parent;

        public GWPMaterialSet Material
        {
            get
            {
                return _element.Material;
            }
            set
            {
                _element.Material = value;
                RaisePropertyChanged(nameof(Material));
            }
        }

        public string Name => _element.Name;

        public Element Element => _element;

        bool _include = false;
        public bool Include
        {
            get
            {
                return _include;
            }
            set
            {
                _include = value;
                if (_include)
                {
                    _exclude = false;
                }
                else
                {
                    _exclude = false;
                }
                RaisePropertyChanged(nameof(Include));
                RaisePropertyChanged(nameof(Exclude));
                RaisePropertyChanged(nameof(AsFilter));
            }
        }
        bool _exclude = false;
        public bool Exclude
        {
            get
            {
                return _exclude;
            }
            set
            {
                _exclude = value;
                if (_exclude)
                {
                    _include = false;
                }
                else
                {
                    _include = false;
                }
                RaisePropertyChanged(nameof(Include));
                RaisePropertyChanged(nameof(Exclude));
                RaisePropertyChanged(nameof(AsFilter));
            }
        }

        public bool AsFilter
        {
            get
            {
                return !(_exclude || _include);
            }
        }

        public string UniqueID => _element.UniqueID;

        public double Volume
        {
            get
            {
                return _element.Volume;
            }
            set
            {
                _element.Volume = value;
                RaisePropertyChanged(nameof(Volume));
                RaisePropertyChanged(nameof(A1toA3));
            }
        }

        public string A1toA3 => string.Format("{0:0.0}", _element.A1toA3);
        public string A4 => string.Format("{0:0.0}", _element.A4);
        public string A5 => string.Format("{0:0.0}", _element.A5);
        public string B => string.Format("{0:0.0}", _element.TotalB);
        public string C1 => string.Format("{0:0.0}", _element.C1);
        public string C2 => string.Format("{0:0.0}", _element.C2);
        public string C3 => string.Format("{0:0.0}", _element.C3);
        public string C4 => string.Format("{0:0.0}", _element.C4);
        public string AtoC => string.Format("{0:0.0}", _element.TotalAtoC);

        public string ElementFilters
        {
            get
            {
                string returnStr = "";
                for (int i = 0; i < _element.Filters.Count(); i++)
                {
                    var filter = _element.Filters[i];
                    returnStr += _parent.FilterNames[i] + ": " + filter + Environment.NewLine;
                }
                return returnStr;
            }
        }

        public string[] FilterValues
        {
            get
            {
                return _element.Filters;
            }
        }

        public bool checkElementInFilterSet(List<FilterSelection> selection)
        {
            bool foundInEachSet = true;
            for (int i = 0; i < _parent.Filters.Count; i++)
            {
                bool foundInSet = false;
                var matching = selection.Where(a => a.FilterName == _parent.Filters[i].Name).ToList();
                foreach (var item in matching)
                {
                    if (_element.Filters[i] == item.FilterValue)
                        foundInSet = true;
                }
                if (matching.Count == 0)
                {
                    foundInSet = true;
                }
                if (!foundInSet)
                {
                    foundInEachSet = false;
                }
            }
            return foundInEachSet;
        }

        public ElementVM(Element element, ModelVM parent)
        {
            _element = element;
            _parent = parent;
        }

        public void UpdateAll()
        {
            RaisePropertyChanged("");
        }
    }
}
