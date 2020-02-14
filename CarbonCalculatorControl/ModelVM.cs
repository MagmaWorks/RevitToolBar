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

namespace CarbonCalculator
{
    public class ModelVM : ViewModelBase, IViewModelParent
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
                foreach (var mat in _materials)
                {
                    foreach (var mat2 in mat.Materials)
                    {
                        mat2.Material.IncludeSequesteredCarbon = value;
                    }
                }
                UpdateAll();
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
                new MaterialSetVM(new CarbonMaterials.GWPMaterialSet("None"), this),
                new MaterialSetVM(CarbonMaterials.GWPMaterialSet.GetDefaultGWPConcreteSetSeparateSteel(), this),
                new MaterialSetVM(CarbonMaterials.GWPMaterialSet.GetDefaultGWPConcreteSet(), this),
                new MaterialSetVM(CarbonMaterials.GWPMaterialSet.GetDefaultFoundationRC(), this),
                new MaterialSetVM(CarbonMaterials.GWPMaterialSet.GetDefaultGlulam(), this),
                new MaterialSetVM(CarbonMaterials.GWPMaterialSet.GetDefaultStructuralSteel(), this),
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
                    newFilterSet.FilterItems.Add(new FilterItemVM(item, newFilterSet));
                }
                Filters.Add(newFilterSet);
            }

            SelectionSets = new ObservableCollection<SelectionSetVM>();
            updateCarbonVsCategoryChartValues();

            var serialiser = new JsonSerializer();
            serialiser.Formatting = Formatting.Indented;
            using (StreamWriter sw = new StreamWriter(@"C:\Users\Alex Baalham\Documents\test.json"))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serialiser.Serialize(writer, _materials[0].GWPMaterial);
                }
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
            SelectionSetVM newSet = new SelectionSetVM(_selectedMaterialIndex, this);
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
                elem.Material = Materials[0].GWPMaterial;
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
                elem.Material = Materials[0].GWPMaterial;
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
            UpdateAll();
        }

        ICommand _outputCSVCommand;

        public ICommand OutputCSVCommand
        {
            get
            {
                return _outputCSVCommand ?? (_outputCSVCommand = new CommandHandler(() => outputToCSV(), true));
            }
        }

        void outputToCSV()
        {
            var save = new Microsoft.Win32.SaveFileDialog();
            save.AddExtension = true;
            save.DefaultExt = "csv";
            save.Filter = @"CSV files | *.csv";
            if (save.ShowDialog() == true)
            {
                using (var w = new StreamWriter(save.FileName))
                {
                    w.WriteLine("ID, Name, Material, A1-A3, A4, A5, B1-B7, C1, C2, C3, C4");
                    foreach (var elem in _elementSet.Elements)
                    {
                        string newLine = "";
                        newLine += elem.UniqueID + ",";
                        newLine += elem.Name + ",";
                        newLine += elem.Material.Name + ",";
                        newLine += elem.A1toA3 + ",";
                        newLine += elem.A4 + ",";
                        newLine += elem.A5 + ",";
                        newLine += elem.TotalB + ",";
                        newLine += elem.C1 + ",";
                        newLine += elem.C2 + ",";
                        newLine += elem.C3 + ",";
                        newLine += elem.C4;
                        w.WriteLine(newLine);
                        w.Flush();
                    }
                }
            }
            
        }

        public void UpdateAll()
        {
            updateCarbonVsCategoryChartValues();
            updateWaterfall();
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


        void updateCarbonVsCategoryChartValues()
        {
            SeriesCollection series = new SeriesCollection();
            var CategoryFilters = Filters[_selectedFilterForCharts].FilterItems;
            foreach (var item in CategoryFilters)
            {
                double catCarbon = 0;
                foreach (var elem in _elementSet.Elements)
                {
                    if (elem.Filters[_selectedFilterForCharts] == item.Name)
                    {
                        catCarbon += elem.TotalAtoC;
                    }
                }
                series.Add(new PieSeries { Values = new ChartValues<double> { catCarbon }, Title = item.Name });
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

        public string[] Labels { get; } = { "", "A1 to A3", "A4", "A5", "B", "C1", "C2", "C3", "C4" };

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
