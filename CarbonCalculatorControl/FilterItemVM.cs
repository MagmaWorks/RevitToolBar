using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;

namespace CarbonCalculator
{
    public class FilterItemVM :ViewModelBase
    {
        [JsonProperty]
        public string Name { get; set; }

        bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }

        public FilterItemVM(string name, FilterSetVM parent)
        {
            Name = name;

            _parent = parent;
        }

        FilterSetVM _parent;
    }
}
