using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarbonCalculator
{
    public class ElementFilterVM
    {
        ElementVM _elem;
        ModelVM _modelVM;
        int ind;

        public string Name => _modelVM.FilterNames[ind];

        public string Value
        {
            get
            {
                return _elem.Element.Filters[ind];
            }
            set
            {
                _elem.Element.Filters[ind] = value;
                _modelVM.initFilters();
                _modelVM.ResetChartColors();
                _modelVM.UpdateAll();
                _modelVM.updateCarbonVsCategoryChartValues();
            }
        }

        public ElementFilterVM(ElementVM elem, ModelVM model, int ind)
        {
            this.ind = ind;
            _elem = elem;
            _modelVM = model;
        }
    }
}
