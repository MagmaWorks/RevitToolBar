using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarbonCalculator
{
    public class ModelVM : ViewModelBase
    {
        ElementSet _elementSet;
        public ObservableCollection<ElementVM> Elements { get; }
        public List<FilterSetVM> Filters { get; }
        public ObservableCollection<SelectionSetVM> SelectionSets { get; }
        ObservableCollection<MaterialSetVM> _materials;
        int _selectedMaterialIndex;
        public int SelectedMaterialIndex
        {
            get
            {
                return _selectedMaterialIndex;
            }
            set
            {
                _selectedMaterialIndex = value;
                RaisePropertyChanged(nameof(SelectedMaterial));
            }
        }

        public double A1toA3
        {
            get
            {
                double returnDouble = 0;
                foreach (var elem in _elementSet.Elements)
                {
                    returnDouble += elem.A1toA3;
                }
                return returnDouble;
            }
        }

        public ObservableCollection<MaterialSetVM> Materials
        {
            get
            {
                return _materials;
            }
        }

        public MaterialSetVM SelectedMaterial
        {
            get
            {
                return _materials[_selectedMaterialIndex];
            }
        }

        ICommand _addMaterialSetCommand;

        public ICommand AddMaterialSetCommand
        {
            get
            {
                return _addMaterialSetCommand ?? (_addMaterialSetCommand = new CommandHandler(() => addMaterialSet(), true));
            }
        }

        void addMaterialSet()
        {
            _materials.Add(new MaterialSetVM("None", new CarbonMaterials.GWPMaterialSet("None"), this));
        }


        public ModelVM(ElementSet elementSet)
        {
            _materials = new ObservableCollection<MaterialSetVM>
            {
                new MaterialSetVM("None", new CarbonMaterials.GWPMaterialSet("None"), this),
                new MaterialSetVM("Concrete", CarbonMaterials.GWPMaterialSet.GetDefaultGWPConcreteSetSeparateSteel(), this),
                new MaterialSetVM("Concrete", CarbonMaterials.GWPMaterialSet.GetDefaultGWPConcreteSet(), this)

            };

            _elementSet = elementSet;
            Elements = new ObservableCollection<ElementVM>();
            foreach (var elem in elementSet.Elements)
            {
                elem.Material = _materials[0].GWPMaterial;
                Elements.Add(new ElementVM(elem, this));
            }
            Filters = new List<FilterSetVM>();
            for (int i = 0; i < elementSet.FilterNames.Count(); i++)
            {
                var filter = elementSet.FilterNames[i];
                var newFilterSet = new FilterSetVM(filter);
                var filterValues = elementSet.Elements.Select(a => a.Filters[i])
                    .Distinct()
                    .ToList();
                foreach (var item in filterValues)
                {
                    newFilterSet.FilterItems.Add(new FilterItemVM(item));
                }
                Filters.Add(newFilterSet);
            }

            SelectionSets = new ObservableCollection<SelectionSetVM>();
            updateCarbonVsCategoryChartValues();
        }

        ICommand _assignToSelectionSetCommand;

        public ICommand AssignToSelectionCommand
        {
            get
            {
                return _assignToSelectionSetCommand ?? (_assignToSelectionSetCommand = new CommandHandler(() => assignToSelection(), true));
            }
        }

        void assignToSelection()
        {
            SelectionSetVM newSet = new SelectionSetVM(_selectedMaterialIndex);
            foreach (var filter in Filters)
            {
                foreach (var item in filter.FilterItems)
                {
                    if (item.IsSelected)
                        newSet.SelectedFilterValues.Add(new FilterSelection(filter.Name, item.Name));
                }
            }
            foreach (var elem in Elements)
            {
                if (elem.Include)
                {
                    newSet.ElementsIdsToInclude.Add(elem.UniqueID);
                }
                if (elem.Exclude)
                {
                    newSet.ElementsIdsToExclude.Add(elem.UniqueID);
                }
            }
            SelectionSets.Add(newSet);
            processSelectionSet(newSet);
            UpdateAll();
        }

        void processSelectionSet(SelectionSetVM set)
        {
            foreach (var elem in Elements)
            {
                if (elem.checkElementInFilterSet(set.SelectedFilterValues) && !set.ElementsIdsToExclude.Contains(elem.UniqueID))
                {
                    elem.Material = Materials[set.SelectedMaterial].GWPMaterial;
                }
                else if (set.ElementsIdsToInclude.Contains(elem.UniqueID))
                {
                    elem.Material = Materials[set.SelectedMaterial].GWPMaterial;
                }
            }
        }

        ICommand _deleteSelectedCommand;

        public ICommand DeleteSelectedCommand
        {
            get
            {
                return _deleteSelectedCommand ?? (_deleteSelectedCommand = new CommandHandler(() => deleteSelected(), true));
            }
        }

        void deleteSelected()
        {
            var itemsToRemove = SelectionSets.Where(a => a.IsSelected).ToList();
            foreach (var item in itemsToRemove)
            {
                SelectionSets.Remove(item);

            }
            foreach (var elem in Elements)
            {
                elem.Material = Materials[0].GWPMaterial;
            }
            foreach (var set in SelectionSets)
            {
                processSelectionSet(set);
            }
        }

        public void UpdateAll()
        {
            updateCarbonVsCategoryChartValues();
            RaisePropertyChanged("");
            foreach (var elem in Elements)
            {
                elem.UpdateAll();
            }
        }

        SeriesCollection _carbonVsCategory;
        public SeriesCollection CarbonVsCategory
        {
            get
            {
                return _carbonVsCategory;
            }
        }

        void updateCarbonVsCategoryChartValues()
        {
            SeriesCollection series = new SeriesCollection();
            var CategoryFilters = Filters[0].FilterItems;
            foreach (var item in CategoryFilters)
            {
                double catCarbon = 0;
                foreach (var elem in _elementSet.Elements)
                {
                    if (elem.Filters[0] == item.Name)
                    {
                        catCarbon += elem.TotalAtoC;
                    }
                }
                series.Add(new PieSeries { Values = new ChartValues<double> { catCarbon }, Title = item.Name });
            }
            _carbonVsCategory = series;
        }

    }
}
