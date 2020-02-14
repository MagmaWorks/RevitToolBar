using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarbonCalculator
{
    public class FilterSetVM : ViewModelBase
    {
        public string Name { get; }
        public ObservableCollection<FilterItemVM> FilterItems { get; }

        public FilterSetVM(string name, params string[] filters)
        {
            FilterItems = new ObservableCollection<FilterItemVM>();
            foreach (var item in filters)
            {
                FilterItems.Add(new FilterItemVM(item, this));
            }
            Name = name;
        }

        public FilterSetVM(string name, IEnumerable<string> filters)
        {
            FilterItems = new ObservableCollection<FilterItemVM>();
            foreach (var item in filters)
            {
                FilterItems.Add(new FilterItemVM(item, this));
            }
            Name = name;
        }
    }
}
