using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts.Wpf;
using LiveCharts;

namespace MagmaWorksToolbar
{
    public class MaterialsVM : ViewModelBase
    {
        public List<ElementCarbonVM> Elements { get; private set; }
        ObservableCollection<ElementCarbonVM> _selectedElements;
        public ObservableCollection<ElementCarbonVM> SelectedElements
        {
            get
            {
                if (_selectedElements == null)
                {
                    _selectedElements = new ObservableCollection<ElementCarbonVM>();
                    foreach (var elem in Elements)
                    {
                        if (elem.IsSelected)
                        {
                            _selectedElements.Add(elem);
                        }
                    }
                    return _selectedElements;
                }
                else
                    return _selectedElements;
            }
        }
        public ObservableCollection<RevitCategory> Categories { get; private set; }
        public ObservableCollection<RevitMaterial> RevitMaterials { get; private set; }

        SeriesCollection _carbonVsCategory;
        public SeriesCollection CarbonVsCategory
        {
            get
            {
                if (_carbonVsCategory == null)
                {
                    updateCarbonVsCategoryChartValues();
                    return _carbonVsCategory;
                }
                else
                    return _carbonVsCategory;
            }
        }

        SeriesCollection _carbonVsMaterial;
        public SeriesCollection CarbonVsMaterial
        {
            get
            {
                if (_carbonVsMaterial == null)
                {
                    updateCarbonVsMaterialChartValues();
                    return _carbonVsMaterial;
                }
                else
                    return _carbonVsMaterial;
            }
        }

        SeriesCollection _volumeVsCategory;
        public SeriesCollection VolumeVsCategory
        {
            get
            {
                if (_volumeVsCategory == null)
                {
                    updateVolumeVsCategoryChartValues();
                    return _volumeVsCategory;
                }
                else
                    return _volumeVsCategory;
            }
        }

        SeriesCollection _volumeVsMaterial;
        public SeriesCollection VolumeVsMaterial
        {
            get
            {
                if (_volumeVsMaterial == null)
                {
                    updateVolumeVsMaterialChartValues();
                    return _volumeVsMaterial;
                }
                else
                    return _volumeVsMaterial;
            }
        }

        SeriesCollection _volumeVsAssignedMaterial;
        public SeriesCollection VolumeVsAssignedMaterial
        {
            get
            {
                if (_volumeVsAssignedMaterial == null)
                {
                    updateVolumeVsAssignedMaterialChartValues();
                    return _volumeVsAssignedMaterial;
                }
                else
                    return _volumeVsAssignedMaterial;
            }
        }

        public MaterialsVM()
        {
            Elements = new List<ElementCarbonVM>();
            Categories = new ObservableCollection<RevitCategory>();
            RevitMaterials = new ObservableCollection<RevitMaterial>();

            updateCarbonVsCategoryChartValues();
            updateCarbonVsMaterialChartValues();
            updateVolumeVsAssignedMaterialChartValues();
            updateVolumeVsCategoryChartValues();
            updateVolumeVsMaterialChartValues();

        }

        void updateCarbonVsCategoryChartValues()
        {
            SeriesCollection series = new SeriesCollection();
            foreach (var item in Categories)
            {
                double total = 0;
                foreach (var elem in Elements)
                {
                    if (elem.Category.Name == item.Name)
                        total += elem.EmbodiedCarbon;
                }
                series.Add(new PieSeries { Values = new ChartValues<double> { total }, Title = item.Name });
            }
            _carbonVsCategory = series;
            //RaisePropertyChanged(nameof(CarbonVsCategory));
        }

        void updateCarbonVsMaterialChartValues()
        {
            SeriesCollection series = new SeriesCollection();
            foreach (var item in RevitMaterials)
            {
                double total = 0;
                foreach (var elem in Elements)
                {
                    if (elem.RevitMaterial.Name == item.Name)
                        total += elem.EmbodiedCarbon;
                }
                series.Add(new PieSeries { Values = new ChartValues<double> { total }, Title = item.Name });
            }
            _carbonVsMaterial = series;
        }

        void updateVolumeVsCategoryChartValues()
        {
            SeriesCollection series = new SeriesCollection();
            foreach (var item in Categories)
            {
                double total = 0;
                foreach (var elem in Elements)
                {
                    if (elem.Category.Name == item.Name)
                        total += elem.Volume;
                }
                series.Add(new PieSeries { Values = new ChartValues<double> { total }, Title = item.Name });
            }
            _volumeVsCategory = series;
        }

        void updateVolumeVsMaterialChartValues()
        {
            SeriesCollection series = new SeriesCollection();
            foreach (var item in RevitMaterials)
            {
                double total = 0;
                foreach (var elem in Elements)
                {
                    if (elem.RevitMaterial.Name == item.Name)
                        total += elem.Volume;
                }
                series.Add(new PieSeries { Values = new ChartValues<double> { total }, Title = item.Name });
            }
            _volumeVsMaterial = series;
        }

        void updateVolumeVsAssignedMaterialChartValues()
        {
            SeriesCollection series = new SeriesCollection();
            foreach (var item in ICEMaterial.MaterialTypes)
            {
                double total = 0;
                foreach (var elem in Elements)
                {
                    if (elem.MaterialType == item)
                        total += elem.Volume;
                }
                series.Add(new PieSeries { Values = new ChartValues<double> { total }, Title = item });
            }
            _volumeVsAssignedMaterial = series;
        }

        public void AddElement(string name, double vol, string revitMaterial, Document doc, BuiltInCategory cat)
        {
            RevitMaterial matToAssign = null;
            string materialToMatch = revitMaterial;
            var revMat = RevitMaterials.Where(a => a.Name == materialToMatch);
            if (revMat.Count() == 0)
            {
                matToAssign = new RevitMaterial(revitMaterial, this);
                RevitMaterials.Add(matToAssign);
            }
            else
            {
                matToAssign = revMat.First();
            }

            RevitCategory catToAssign = null;
            string nameToMatch = Category.GetCategory(doc, cat).Name;
            var revCat = Categories.Where(a => a.Name == nameToMatch);
            if (revCat.Count() == 0)
            {
                catToAssign = new RevitCategory(doc, cat, this);
                Categories.Add(catToAssign);
            }
            else
            {
                catToAssign = revCat.First();
            }

            Elements.Add(new ElementCarbonVM(name, vol, matToAssign, catToAssign, this));
        }

        public void selectionChanged()
        {
            _selectedElements.Clear();
            foreach (var elem in Elements)
            {
                if (elem.IsSelected)
                {
                    _selectedElements.Add(elem);
                }
            }
            RaisePropertyChanged(nameof(SelectedElements));
            //foreach (var item in Elements)
            //{
            //    item.selectionUpdated();
            //}
        }

        public void ChangeICEMaterial(ICEMaterial mat)
        {
            foreach (var item in Elements)
            {
                if (item.IsSelected && item.ElementSelected)
                {
                    item.UpdateICEMaterial(ICEMaterial.CopyMaterial(mat, item));
                }
            }
            Updated();
        }

        public void Updated()
        {
            RaisePropertyChanged(nameof(TotalCarbon));
            RaisePropertyChanged(nameof(TotalCarbonDisplay));
            _carbonVsCategory = null;
            RaisePropertyChanged(nameof(CarbonVsCategory));
            _carbonVsMaterial = null;
            RaisePropertyChanged(nameof(CarbonVsMaterial));
            _volumeVsCategory = null;
            RaisePropertyChanged(nameof(VolumeVsCategory));
            _volumeVsMaterial = null;
            RaisePropertyChanged(nameof(VolumeVsMaterial));
            _volumeVsAssignedMaterial = null;
            RaisePropertyChanged(nameof(VolumeVsAssignedMaterial));
        }

        public double TotalCarbon
        {
            get
            {
                double returnVal = 0;
                foreach (var elem in Elements)
                {
                    returnVal += elem.Volume * elem.ICEMaterial.CarbonDensity;
                }
                return returnVal / 1000;
            }
        }

        public string TotalCarbonDisplay
        {
            get
            {
                double returnVal = 0;
                foreach (var elem in Elements)
                {
                    returnVal += elem.Volume * elem.ICEMaterial.CarbonDensity;
                }
                return string.Format("{0:0.0}", returnVal / 1000) + " tonnes";
            }
        }

        public int ElementsImported
        {
            get
            {
                return Elements.Count;
            }
        }

        string _totalVolumeDisplay = "";
        public string TotalVolumeDisplay
        {
            get
            {
                double totalVol = 0;
                foreach (var elem in Elements)
                {
                    totalVol += elem.Volume;
                }
                _totalVolumeDisplay = string.Format("{0:0.0}", totalVol) + "m" + '\u00B3';
                return _totalVolumeDisplay; 
            }
        }


        public List<string> MaterialTypes
        {
            get
            {
                return ICEMaterial.MaterialTypes;
            }
        }
    }
}