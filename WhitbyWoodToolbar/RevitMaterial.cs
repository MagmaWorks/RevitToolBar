using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagmaWorksToolbar
{
    public class RevitMaterial : ViewModelBase
    {
        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        bool _isSelected = true;
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
                _parent.selectionChanged();
            }
        }

        MaterialsVM _parent;

        public RevitMaterial(string name, MaterialsVM parent)
        {
            _name = name;
            _parent = parent;
        }
    }
}
