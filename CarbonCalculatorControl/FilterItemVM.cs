using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json;

namespace CarbonCalculator
{
    public class FilterItemVM :ViewModelBase
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        string _color = "#FFDFD991";

        public string Color
        {
            get
            {
                return _color;
            }
            set
            {
                try
                {
                    ColorConverter.ConvertFromString(value);
                    _color = value;
                }
                catch (Exception)
                {
                }               
                RaisePropertyChanged(nameof(Color));
                RaisePropertyChanged(nameof(FillColor));
                _parent.FilterUpdated();
            }
        }

        public Brush FillColor
        {
            get
            {
                var col =  (Color)ColorConverter.ConvertFromString(_color);
                return new SolidColorBrush(col);
            }
        }

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
                _parent.FilterUpdated();
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
