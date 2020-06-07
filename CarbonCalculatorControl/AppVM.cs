using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Linq;

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
                if (save.ShowDialog() == DialogResult.OK)
                {
                    using (var w = new StreamWriter(save.FileName))
                    {
                        w.WriteLine("ID, Name, Material, A1-A3, A4, A5, B1-B7, C1, C2, C3, C4");
                        foreach (var elem in vm.ElementSet.Elements)
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
            catch (Exception ex)
            {
                MessageBox.Show("oops..." + Environment.NewLine + ex.Message);
            }


        }
    }
}
