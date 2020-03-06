using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CarbonCalculator
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FilterSetVM : ViewModelBase
    {
        [JsonProperty]
        public string Name { get; private set; }
        [JsonProperty]
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
