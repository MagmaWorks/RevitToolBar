using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarbonCalculator
{
    public class MenuItemVM : ViewModelBase
    {
        public string Name { get; }

        public MenuItemVM(string name)
        {
            Name = name;
        }
    }
}
