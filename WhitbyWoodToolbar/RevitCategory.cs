using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhitbyWoodToolbar
{
    public class RevitCategory:ViewModelBase
    {
        Category revitCat;

        public string Name
        {
            get
            {
                return revitCat.Name;
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

        public RevitCategory(Document doc, BuiltInCategory cat, MaterialsVM parent)
        {
            revitCat = Category.GetCategory(doc, cat);
            _parent = parent;
        }
    }
}
