using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Windows;
using System.Windows.Data;

namespace CarbonCalculator
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ModelVM : ViewModelBase, IViewModelParent
    {
        [JsonProperty(PropertyName = "ElementSet")]
        ElementSet _elementSet;
        public ElementSet ElementSet { get => _elementSet; }

        public ObservableCollection<ElementVM> Elements { get; private set; }
        public ListCollectionView FilteredElements { get; private set; }

        public List<FilterSetVM> Filters { get; private set; }
        [JsonProperty(PropertyName = "SelectionSets")]
        public ObservableCollection<SelectionSetVM> SelectionSets { get; private set; }
        //ObservableCollection<MaterialSetVM> _materials = new ObservableCollection<MaterialSetVM>();

        bool _viewAll;
        public bool ViewAll
        {
            get
            {
                return _viewAll;
            }
            set
            {
                _viewAll = value;
                RaisePropertyChanged(nameof(ViewAll));
            }
        }


        string _filePath = "";
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                RaisePropertyChanged(nameof(FilePath));
            }
        }

        ObservableCollection<MenuItemVM> _materialSetNames = new ObservableCollection<MenuItemVM>();
        public ObservableCollection<MenuItemVM> MaterialSetNames
        {
            get
            {
                return _materialSetNames;
            }
        }

        void updateMatSetNames()
        {
            _materialSetNames.Clear();
            foreach (var matSet in _elementSet.MaterialSets)
            {
                _materialSetNames.Add(new MenuItemVM(matSet.Name));
            }
        }

        int _selectedMaterialSetIndex;
        public int SelectedMaterialSetIndex
        {
            get
            {
                return _selectedMaterialSetIndex;
            }
            set
            {
                _selectedMaterialSetIndex = value;
                RaisePropertyChanged(nameof(SelectedMaterialSet));
                updateMatNames();
                RaisePropertyChanged(nameof(SelectedMaterial));
                RaisePropertyChanged(nameof(SelectedMaterialSetName));
            }
        }

        MaterialSetVM _selectedMaterialSet;

        public MaterialSetVM SelectedMaterialSet
        {
            get
            {
                if (_selectedMaterialSetIndex >= 0)
                {
                    _selectedMaterialSet = new MaterialSetVM(_elementSet.MaterialSets[_selectedMaterialSetIndex], this);
                    return _selectedMaterialSet;
                }
                else
                {
                    return null;
                }
            }
        }

        ObservableCollection<MenuItemVM> _materialNames = new ObservableCollection<MenuItemVM>();
        public ObservableCollection<MenuItemVM> MaterialNames
        {
            get
            {
                return _materialNames;
            }
        }

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

        private void updateMatNames()
        {
            _materialNames.Clear();
            if (_selectedMaterialSetIndex >= 0)
            {
                foreach (var mat in _elementSet.MaterialSets[_selectedMaterialSetIndex].Materials)
                {
                    _materialNames.Add(new MenuItemVM(mat.Name));
                }
            }

            _selectedMaterialIndex = 0;
        }

        MaterialVM _selectedMaterial;

        public MaterialVM SelectedMaterial
        {
            get
            {
                if (_selectedMaterialSetIndex >=0 && _selectedMaterialIndex >= 0)
                {
                    var mat = _elementSet.MaterialSets[_selectedMaterialSetIndex].Materials[_selectedMaterialIndex];

                    _selectedMaterial = new MaterialVM(mat);
                    return _selectedMaterial;
                }
                else
                {
                    return null;
                }
            }
        }

        ICommand _deleteMaterialSetCommand;

        public ICommand DeleteMaterialSetCommand
        {
            get
            {
                return _deleteMaterialSetCommand ?? (_deleteMaterialSetCommand = new CommandHandlerWithParameter(new Action<object>(deleteMaterialSet), true));
            }
        }

        void deleteMaterialSet(object obj)
        {
            var matSet = obj as MenuItemVM;
            int itemToDelete = MaterialSetNames.IndexOf(matSet);
            int currentSel = _selectedMaterialSetIndex;
            _materialSetNames.Remove(matSet);
            _elementSet.MaterialSets.RemoveAt(itemToDelete);
            if (currentSel >= _elementSet.MaterialSets.Count)
                _selectedMaterialSetIndex = _elementSet.MaterialSets.Count - 1;
            else
                _selectedMaterialSetIndex = currentSel;
            updateMatNames();
            RaisePropertyChanged(nameof(MaterialSetNames));
            RaisePropertyChanged(nameof(SelectedMaterialSet));
            RaisePropertyChanged(nameof(SelectedMaterial));
        }

        ICommand _deleteMaterialCommand;

        public ICommand DeleteMaterialCommand
        {
            get
            {
                return _deleteMaterialCommand ?? (_deleteMaterialCommand = new CommandHandlerWithParameter(new Action<object>(deleteMaterial), true));
            }
        }

        void deleteMaterial(object obj)
        {
            var mat = obj as MenuItemVM;
            int itemToDelete = MaterialNames.IndexOf(mat);
            int currentSel = _selectedMaterialIndex;
            _materialNames.Remove(mat);
            _elementSet.MaterialSets[_selectedMaterialSetIndex].Materials.RemoveAt(itemToDelete);
            if (currentSel >= _elementSet.MaterialSets[_selectedMaterialSetIndex].Materials.Count)
                _selectedMaterialIndex = _elementSet.MaterialSets[_selectedMaterialSetIndex].Materials.Count - 1;
            else
                _selectedMaterialIndex = currentSel;
            updateMatNames();
            RaisePropertyChanged(nameof(MaterialSetNames));
            RaisePropertyChanged(nameof(SelectedMaterialSet));
            RaisePropertyChanged(nameof(SelectedMaterial));
        }

        public string SelectedMaterialSetName
        {
            get
            {
                if (_selectedMaterialSetIndex >= 0)
                {
                    return _elementSet.MaterialSets[_selectedMaterialSetIndex].Name;
                }
                else
                    return "";
               
            }
            set
            {
                _elementSet.MaterialSets[_selectedMaterialSetIndex].Name = value;
                int currentSel = _selectedMaterialSetIndex;
                updateMatSetNames();
                SelectedMaterialSetIndex = currentSel;
                RaisePropertyChanged(nameof(SelectedMaterialSetName));
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

        public double AtoC
        {
            get
            {
                double returnDouble = 0;
                foreach (var elem in _elementSet.Elements)
                {
                    returnDouble += elem.TotalAtoC;
                }
                return returnDouble;
            }
        }

        public string AtoCString
        {
            get
            {
                double total = AtoC / 1000;
                return string.Format("{0:0.00}", total) + "tonnes CO2e";
            }
        }

        [JsonProperty(PropertyName = "IncludeSequestration")]
        bool _includeSequestration = true;
        public bool IncludeSequestration
        {
            get
            {
                return _includeSequestration; 
            }
            set
            {
                _includeSequestration = value;
                foreach (var matSet in _elementSet.MaterialSets)
                {
                    foreach (var mat in matSet.Materials)
                    {
                        mat.IncludeSequesteredCarbon = value;
                    }
                }
                //foreach (var mat in _materials)
                //{
                //    foreach (var mat2 in mat.Materials)
                //    {
                //        mat2.Material.IncludeSequesteredCarbon = value;
                //    }
                //}
                UpdateAll();
            }
        }



        //public ObservableCollection<MaterialSetVM> Materials
        //{
        //    get
        //    {
        //        return _materials;
        //    }
        //}

        public List<CarbonMaterials.GWPMaterialSet> MaterialSets
        {
            get
            {
                return _elementSet.MaterialSets;
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
            var newSet = new CarbonMaterials.GWPMaterialSet("None");
            _elementSet.MaterialSets.Add(newSet);
            //_materials.Add(new MaterialSetVM("None", newSet, this));
            _materialSetNames.Add(new MenuItemVM(newSet.Name));
        }

 

        public ModelVM()
        {
        }

        public ModelVM(ElementSet elementSet)
        {
            _elementSet = elementSet;
            initialise();
            SelectionSets = new ObservableCollection<SelectionSetVM>();
            updateCarbonVsCategoryChartValues();
            UpdateFilteredElements();
            ResetChartColors();
        }

        public void initialise()
        {
            //_materials.Clear();
            _materialSetNames.Clear();
            foreach (var matSet in _elementSet.MaterialSets)
            {
                //_materials.Add(new MaterialSetVM(matSet, this));
                _materialSetNames.Add(new MenuItemVM(matSet.Name));
            }

            Elements = new ObservableCollection<ElementVM>();
            foreach (var elem in _elementSet.Elements)
            {
                Elements.Add(new ElementVM(elem, this));
            }
            initFilters();
            if (SelectionSets != null)
            {
                foreach (var set in SelectionSets)
                {
                    set.setParent(this);
                }
            }

            FilteredElements = new ListCollectionView(Elements);
            FilteredElements.Filter = new Predicate<object>(MatchesFilterSet);
        }

        public bool MatchesFilterSet (object o)
        {
            ElementVM elem =  o as ElementVM;
            return elem.Display;
        }

        public void initFilters()
        {
            Filters = new List<FilterSetVM>();
            for (int i = 0; i < _elementSet.FilterNames.Count(); i++)
            {
                var filter = _elementSet.FilterNames[i];
                var newFilterSet = new FilterSetVM(filter, this);
                var filterValues = _elementSet.Elements.Select(a => a.Filters[i])
                    .Distinct()
                    .ToList();
                foreach (var item in filterValues)
                {
                    newFilterSet.FilterItems.Add(new FilterItemVM(item, newFilterSet));
                }
                Filters.Add(newFilterSet);
            }
        }

        [JsonProperty(PropertyName = "FilterChartColors")]
        Dictionary<string, Dictionary<string, string>> FilterChartColors = new Dictionary<string, Dictionary<string, string>>();

        public void SetChartColors()
        {
            foreach (var filter in Filters)
            {
                if (FilterChartColors.ContainsKey(filter.Name))
                {
                    foreach (var item in filter.FilterItems)
                    {
                        if (FilterChartColors[filter.Name].ContainsKey(item.Name))
                        {
                            item.Color = FilterChartColors[filter.Name][item.Name];
                        }
                    }
                }

            }
        }

        public void SaveChartColors()
        {
            FilterChartColors.Clear();
            foreach (var filters in Filters)
            {
                Dictionary<string, string> newDict = new Dictionary<string, string>();
                foreach (var item in filters.FilterItems)
                {
                    newDict.Add(item.Name, item.Color);
                }
                FilterChartColors.Add(filters.Name, newDict);
            }
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
            SelectionSetVM newSet = new SelectionSetVM(_selectedMaterialSetIndex, this);
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

        public void MoveUp(SelectionSetVM item)
        {
            int index = SelectionSets.IndexOf(item);
            if (index > 0)
            {
                SelectionSets.Move(index, index - 1);
            }
            foreach (var elem in Elements)
            {
                elem.Material = _elementSet.MaterialSets[0];
            }
            foreach (var set in SelectionSets)
            {
                processSelectionSet(set);
            }
            UpdateAll();
        }

        public void MoveDown(SelectionSetVM item)
        {
            int index = SelectionSets.IndexOf(item);
            if (index >= 0 && index < SelectionSets.Count-1)
            {
                SelectionSets.Move(index, index + 1);
            }
            foreach (var elem in Elements)
            {
                elem.Material = _elementSet.MaterialSets[0];
            }
            foreach (var set in SelectionSets)
            {
                processSelectionSet(set);
            }
            UpdateAll();

        }

        void processSelectionSet(SelectionSetVM set)
        {
            foreach (var elem in Elements)
            {
                if (elem.checkElementInFilterSet(set.SelectedFilterValues) && !set.ElementsIdsToExclude.Contains(elem.UniqueID))
                {
                    elem.Material = _elementSet.MaterialSets[set.SelectedMaterial];
                }
                else if (set.ElementsIdsToInclude.Contains(elem.UniqueID))
                {
                    elem.Material = _elementSet.MaterialSets[set.SelectedMaterial];
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
                elem.Material = _elementSet.MaterialSets[0];
            }
            foreach (var set in SelectionSets)
            {
                processSelectionSet(set);
            }
            UpdateAll();
        }

        ICommand _updateAllCommand;

        public ICommand UpdateAllCommand
        {
            get
            {
                return _updateAllCommand ?? (_updateAllCommand = new CommandHandler(() => UpdateAll(), true));
            }
        }

        public void UpdateAll()
        {
            updateCarbonVsCategoryChartValues();
            updateWaterfall();
            RaisePropertyChanged("");
            UpdateFilteredElements();
            foreach (var elem in Elements)
            {
                elem.UpdateAll();
            }
        }

        public void UpdateFilteredElements()
        {
            SelectionSetVM currentSet = new SelectionSetVM(_selectedMaterialSetIndex, this);
            foreach (var filter in Filters)
            {
                foreach (var item in filter.FilterItems)
                {
                    if (item.IsSelected)
                        currentSet.SelectedFilterValues.Add(new FilterSelection(filter.Name, item.Name));
                }
            }
            foreach (var elem in Elements)
            {
                elem.UpdateAll();
                if ((elem.checkElementInFilterSet(currentSet.SelectedFilterValues) && !elem.Exclude) || elem.Include)
                {
                    elem.Display = true;
                }
                else
                    elem.Display = false;
            }
            FilteredElements.Filter = new Predicate<object>(MatchesFilterSet);

        }

        SeriesCollection _carbonVsCategory;
        public SeriesCollection CarbonVsCategory
        {
            get
            {
                return _carbonVsCategory;
            }
        }

        int _selectedFilterForCharts = 1;
        public int SelectedFilterForCharts
        {
            get
            {
                return _selectedFilterForCharts;
            }
            set
            {
                _selectedFilterForCharts = value;
                updateCarbonVsCategoryChartValues();
                RaisePropertyChanged(nameof(CarbonVsCategory));
                RaisePropertyChanged(nameof(SelectedFilterForCharts));
            }
        }
        public string[] FilterNames
        {
            get
            {
                return _elementSet.FilterNames;
            }
        }

        public string[] PropertiesToDisplay { get; } = new string[] { "Embodied Carbon", "Material Quantity" };

        string _propToDisplay = "Embodied Carbon";

        public string PropToDisplay
        {
            get
            {
                return _propToDisplay;
            }
            set
            {
                _propToDisplay = value;
                updateCarbonVsCategoryChartValues();
                RaisePropertyChanged(nameof(CarbonVsCategory));
                RaisePropertyChanged(nameof(PropToDisplay));
            }
        }

        public void updateCarbonVsCategoryChartValues()
        {
            foreach (var elem in Elements)
            {
                elem.Material = _elementSet.MaterialSets[0];
            }
            foreach (var set in SelectionSets)
            {
                processSelectionSet(set);
            }
            SeriesCollection series = new SeriesCollection();
            var CategoryFilters = Filters[_selectedFilterForCharts].FilterItems;
            foreach (var item in CategoryFilters)
            {
                double catCarbon = 0;
                foreach (var elem in _elementSet.Elements)
                {
                    if (elem.Filters[_selectedFilterForCharts] == item.Name)
                    {
                        if (_propToDisplay == "Embodied Carbon")
                        {
                            catCarbon += elem.TotalAtoC;
                        }
                        else
                        {
                            catCarbon += elem.Volume;
                        }
                    }
                }
                series.Add(new PieSeries { Values = new ChartValues<double> { catCarbon }, Title = item.Name, Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(item.Color)) });
            }
            _carbonVsCategory = series;
        }

        SeriesCollection _waterfall;
        public SeriesCollection Waterfall
        {
            get
            {
                return _waterfall;
            }
        }

        public void changeElementSet(ElementSet newSet)
        {
            _elementSet = newSet;
            Elements.Clear();
            foreach (var elem in _elementSet.Elements)
            {
                Elements.Add(new ElementVM(elem, this));
            }
            initFilters();
            foreach (var set in SelectionSets)
            {
                processSelectionSet(set);
            }
            UpdateAll();
        }

        public string[] Labels { get; } = { "", "A1 to A3", "A4", "A5", "B", "C1", "C2", "C3", "C4" };

        public void ResetChartColors()
        {
            List<string> defaultColors = new List<string> { "#FFF48B66", "#FFF5CC65", "#FFBEF565", "#FF64F67C", "#FF65BBF5", "#FF657DF5", "#FFA963F7" };
            int i = 0;
            foreach (var filters in Filters)
            {
                i = 0;
                foreach (var filterval in filters.FilterItems)
                {
                    if (i > defaultColors.Count - 1)
                    {
                        i = 0;
                    }
                    filterval.Color = defaultColors[i];
                    i++;
                }
            }
        }

        ICommand _resetColorsCommandCommand;

        public ICommand ResetColorsCommand
        {
            get
            {
                return _resetColorsCommandCommand ?? (_resetColorsCommandCommand = new CommandHandler(() => ResetChartColors(), true));
            }
        }

        void updateWaterfall()
        {
            SeriesCollection series = new SeriesCollection();
            double A1toA3 = 0;
            double A4 = 0;
            double A5 = 0;
            double B = 0;
            double C1 = 0;
            double C2 = 0;
            double C3 = 0;
            double C4 = 0;
            foreach (var elem in Elements)
            {
                A1toA3 += elem.Element.A1toA3;
                A4 += elem.Element.A4;
                A5 += elem.Element.A5;
                B += elem.Element.TotalB;
                C1 += elem.Element.C1;
                C2 += elem.Element.C2;
                C3 += elem.Element.C3;
                C4 += elem.Element.C4;
            }

            A1toA3 = (A1toA3) / 1000;
            A4 = A4 / 1000 + A1toA3;
            A5 = A5 / 1000 + A4;
            B = B / 1000 + A5;
            C1 = C1 / 1000 + B;
            C2 = C2 / 1000 + C1;
            C3 = C3 / 1000 + C2;
            C4 = C4 / 1000 + C3;


            series.Add(new LineSeries()
            {
                Values = new ChartValues<double> { 0, A1toA3, A4, A5, B, C1, C2, C3, C4 },
                LineSmoothness = 0
            });
            _waterfall = series;
        }
    }
}
