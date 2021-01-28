using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;

namespace CarbonCalculator
{
    public class AppVM : ViewModelBase
    {
        ModelVM vm;
        public ModelVM Model
        {
            get
            {
                return vm;
            }
        }
           

        public AppVM(ElementSet elementSet)
        {
            vm = new ModelVM(elementSet);
        }


        ICommand _saveToFileCommand;

        public ICommand SaveToFileCommand
        {
            get
            {
                return _saveToFileCommand ?? (_saveToFileCommand = new CommandHandler(() => saveToFile(), true));
            }
        }

        void saveToFile()
        {
            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.TypeNameHandling = TypeNameHandling.Auto;
            vm.SaveChartColors();
            var output = JsonConvert.SerializeObject(vm, settings);

            string filePath = "";
            try
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = @"JSON files |*.JSON";
                if (vm.FilePath == "")
                {
                    saveDialog.FileName = "New file" + @".JSON";
                    saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    if (saveDialog.ShowDialog() != DialogResult.OK) return;
                    filePath = saveDialog.FileName;
                }
                else
                    filePath = vm.FilePath;

                using (var writer = File.CreateText(filePath))
                {
                    writer.Write(output);
                }
                vm.FilePath = filePath;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Oops..." + Environment.NewLine + ex.Message);
                return;
            }
        }

        ICommand _saveAsToFileCommand;

        public ICommand SaveAsToFileCommand
        {
            get
            {
                return _saveAsToFileCommand ?? (_saveAsToFileCommand = new CommandHandler(() => saveAsToFile(), true));
            }
        }

        void saveAsToFile()
        {
            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.TypeNameHandling = TypeNameHandling.Auto;
            vm.SaveChartColors();
            var output = JsonConvert.SerializeObject(vm, settings);

            string filePath = "";
            try
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.Filter = @"JSON files |*.JSON";
                if (vm.FilePath == "")
                {
                    saveDialog.FileName = "New file" + @".JSON";
                    saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                else
                {
                    saveDialog.FileName = Path.GetFileName(vm.FilePath);
                    saveDialog.InitialDirectory = Path.GetDirectoryName(vm.FilePath);
                }

                if (saveDialog.ShowDialog() != DialogResult.OK) return;
                filePath = saveDialog.FileName;
                using (var writer = File.CreateText(filePath))
                {
                    writer.Write(output);
                }
                vm.FilePath = filePath;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Oops..." + Environment.NewLine + ex.Message);
                return;
            }
        }

        ICommand _openFileCommand;

        public ICommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ?? (_openFileCommand = new CommandHandler(() => openFile(), true));
            }
        }

        void openFile()
        {
            bool outputOK = false;
            ModelVM output = new ModelVM();
            string filePath = vm.FilePath;

            try
            {
                var openDialog = new OpenFileDialog();
                openDialog.Filter = @"Carbon model files |*.JSON";
                openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openDialog.Multiselect = false;
                if (openDialog.ShowDialog() != DialogResult.OK) return;
                filePath = openDialog.FileName;
                using (var reader = File.OpenText(openDialog.FileName))
                {
                    var settings = new JsonSerializerSettings();
                    settings.Formatting = Formatting.Indented;
                    settings.TypeNameHandling = TypeNameHandling.Auto;
                    output = JsonConvert.DeserializeObject<ModelVM>(reader.ReadToEnd(), settings);
                }
                output.initialise();
                output.SetChartColors();
                output.updateCarbonVsCategoryChartValues();
                outputOK = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("oops..." + Environment.NewLine + ex.Message);
            }
            if (outputOK)
            {
                vm = output;
                vm.FilePath = filePath;
                vm.UpdateAll();
                RaisePropertyChanged(nameof(Model));
            }
        }

        ICommand _addElementCommand;

        public ICommand AddElementCommand
        {
            get
            {
                return _addElementCommand ?? (_addElementCommand = new CommandHandler(() => addElement(), true));
            }
        }

        void addElement()
        {
            string[] filterVals = new string[Model.Filters.Count];
            for (int i = 0; i < Model.Filters.Count; i++)
            {
                filterVals[i] = "";
            }
            Model.ElementSet.AddElement(new Element("User element", 0, "User" + _userelem.ToString("0000"), filterVals));
            Model.Elements.Add(new ElementVM(Model.ElementSet.Elements.Last(), Model));
            _userelem++;
            Model.UpdateAll();
        }

        ICommand _deleteElementCommand;

        public ICommand DeleteElementCommand
        {
            get
            {
                return _deleteElementCommand ?? (_deleteElementCommand = new CommandHandler(() => deleteElement(), true));
            }
        }

        void deleteElement()
        {
            List<int> indicesToDelete = new List<int>();
            for (int i = 0; i < Model.Elements.Count; i++)
            {
                if (Model.Elements[i].IsSelected)
                {
                    indicesToDelete.Add(i);
                }
            }
            indicesToDelete.Reverse();

            foreach (var item in indicesToDelete)
            {
                Model.ElementSet.Elements.Remove(Model.Elements[item].Element);
                Model.Elements.RemoveAt(item);

            }
            Model.UpdateAll();

        }

        ICommand _changeToVolumeCommand;

        public ICommand ChangeToVolumeCommand
        {
            get
            {
                return _changeToVolumeCommand ?? (_changeToVolumeCommand = new CommandHandler(() => changeToVolume(), true));
            }
        }

        void changeToVolume()
        {
            foreach (var elem in Model.Elements)
            {
                if (elem.IsSelected)
                {
                    elem.Element.SpatialDimensions = CarbonMaterials.Measurement.Volume;
                }
            }

            Model.UpdateAll();
        }

        ICommand _changeToAreaCommand;

        public ICommand ChangeToAreaCommand
        {
            get
            {
                return _changeToAreaCommand ?? (_changeToAreaCommand = new CommandHandler(() => changeToArea(), true));
            }
        }

        void changeToArea()
        {
            foreach (var elem in Model.Elements)
            {
                if (elem.IsSelected)
                {
                    elem.Element.SpatialDimensions = CarbonMaterials.Measurement.Area;
                }
            }

            Model.UpdateAll();
        }

        int _userelem = 0;

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
            try
            {
                var save = new SaveFileDialog();
                save.AddExtension = true;
                save.DefaultExt = "csv";
                save.Filter = @"CSV files | *.csv";
                string filterNames = "";
                foreach (var item in Model.ElementSet.FilterNames)
                {
                    filterNames += "," + item;
                }
                if (save.ShowDialog() == DialogResult.OK)
                {
                    using (var w = new StreamWriter(save.FileName))
                    {
                        w.WriteLine("ID, Name, Material, A1-A3, A4, A5, B1-B7, C1, C2, C3, C4" + filterNames);
                        foreach (var elem in vm.ElementSet.Elements)
                        {
                            string newLine = "";
                            newLine += elem.UniqueID + ",";
                            newLine += "\"" + elem.Name + "\"" + ",";
                            newLine += "\"" + elem.Material.Name + "\"" + ",";
                            newLine += elem.A1toA3 + ",";
                            newLine += elem.A4 + ",";
                            newLine += elem.A5 + ",";
                            newLine += elem.TotalB + ",";
                            newLine += elem.C1 + ",";
                            newLine += elem.C2 + ",";
                            newLine += elem.C3 + ",";
                            newLine += elem.C4;
                            string filterValues = "";
                            foreach (var item in elem.Filters)
                            {
                                filterValues += "," + "\"" + item + "\"";
                            }
                            newLine += filterValues;
                            w.WriteLine(newLine);
                            w.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("oops..." + Environment.NewLine + ex.Message);
            }


        }

        ICommand _outputSummaryCSVCommand;

        public ICommand OutputSummaryCSVCommand
        {
            get
            {
                return _outputSummaryCSVCommand ?? (_outputSummaryCSVCommand = new CommandHandler(() => outputSummaryToCSV(), true));
            }
        }

        void outputSummaryToCSV()
        {
            try
            {
                var save = new SaveFileDialog();
                save.AddExtension = true;
                save.DefaultExt = "csv";
                save.Filter = @"CSV files | *.csv";
                string filterNames = "";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    using (var w = new StreamWriter(save.FileName))
                    {
                        for (int i = 0; i < Model.ElementSet.FilterNames.Count(); i++)
                        {
                            string filterName = Model.ElementSet.FilterNames[i];
                            string[] uniqueFilterValues = vm.ElementSet.Elements.Select(a => a.Filters[i]).Distinct().ToArray();

                            string valueHeaders = "";
                            string valuesA = "A1-A5";
                            string valuesBC = "B1-C4";
                            foreach (var item in uniqueFilterValues)
                            {
                                valueHeaders += ",";
                                valueHeaders += item;

                                double totalA = Model.ElementSet.Elements.Where(a => a.Filters[i] == item).Sum(a => a.TotalA);
                                valuesA += "," + totalA;
                                double totalBC = Model.ElementSet.Elements.Where(a => a.Filters[i] == item).Sum(a => a.TotalB + a.TotalC);
                                valuesBC += "," + totalBC;

                            }
                            w.WriteLine("Results by " + filterName);
                            w.WriteLine(valueHeaders);
                            w.WriteLine(valuesA);
                            w.WriteLine(valuesBC);
                            w.Flush();
                        }
                    }
                }


           
            }
            catch (Exception ex)
            {
                MessageBox.Show("oops..." + Environment.NewLine + ex.Message);
            }


        }


        ICommand _importMaterialsCommand;

        public ICommand ImportMaterialsCommand
        {
            get
            {
                return _importMaterialsCommand ?? (_importMaterialsCommand = new CommandHandler(() => importMaterials(), true));
            }
        }

        void importMaterials()
        {
            bool outputOK = false;
            ModelVM output = new ModelVM();
            string filePath = vm.FilePath;

            try
            {
                var openDialog = new OpenFileDialog();
                openDialog.Filter = @"Carbon model files |*.JSON";
                openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openDialog.Multiselect = false;
                if (openDialog.ShowDialog() != DialogResult.OK) return;
                filePath = openDialog.FileName;
                using (var reader = File.OpenText(openDialog.FileName))
                {
                    var settings = new JsonSerializerSettings();
                    settings.Formatting = Formatting.Indented;
                    settings.TypeNameHandling = TypeNameHandling.Auto;
                    output = JsonConvert.DeserializeObject<ModelVM>(reader.ReadToEnd(), settings);
                }
                output.initialise();
                output.SetChartColors();
                output.updateCarbonVsCategoryChartValues();
                outputOK = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("oops..." + Environment.NewLine + ex.Message);
            }
            if (outputOK)
            {
                foreach (var item in output.MaterialSets)
                {
                    vm.ElementSet.MaterialSets.Add(item);
                }
                vm.initialise();
                vm.UpdateAll();
                RaisePropertyChanged(nameof(Model));
            }
        }
    }
}
