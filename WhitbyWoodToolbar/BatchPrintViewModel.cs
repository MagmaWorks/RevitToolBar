using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace WhitbyWoodToolbar
{
    public class BatchPrintViewModel : ViewModelBase
    {
        ExternalCommandData commandData;

        public ObservableCollection<sheetVM> SheetInfo { get; set; }

        ICommand _printCommand;

        public ICommand PrintCommand
        {
            get
            {
                return _printCommand ?? (_printCommand = new CommandHandler(() => batchPrint(), true));
            }
        }

        ICommand _filterCommand;

        public ICommand FilterCommand
        {
            get
            {
                return _filterCommand ?? (_filterCommand = new CommandHandler(() => filter(), true));
            }
        }

        bool _includeRev = false;
        public bool IncludeRev
        {
            get
            {
                return _includeRev;
            }
            set
            {
                _includeRev = value;
                foreach (var sheet in SheetInfo)
                {
                    sheet.IncludeRev = value;
                }
                RaisePropertyChanged(nameof(IncludeRev));
            }
        }

        public string Comment { get; set; }

        ICommand _addCommentCommand;

        public ICommand AddCommentCommand
        {
            get
            {
                return _addCommentCommand ?? (_addCommentCommand = new CommandHandler(() => addComment(), true));
            }
        }

        void addComment()
        {
            foreach (var sheet in SheetInfo)
            {
                sheet.Comment = Comment;
            }
        }

        public string Prefix { get; set; }

        ICommand _addPrefixCommand;

        public ICommand AddPrefixCommand
        {
            get
            {
                return _addPrefixCommand ?? (_addPrefixCommand = new CommandHandler(() => addPrefix(), true));
            }
        }

        private void addPrefix()
        {
            foreach (var sheet in SheetInfo)
            {
                sheet.Prefix = Prefix;
            }
        }

        bool _includeName = false;
        public bool IncludeName
        {
            get
            {
                return _includeName;
            }
            set
            {
                _includeName = value;
                foreach (var sheet in SheetInfo)
                {
                    sheet.IncludeName = value;
                }
                RaisePropertyChanged(nameof(IncludeName));
            }
        }

        bool _copyToFolder = false;
        public bool CopyToFolder
        {
            get
            {
                return _copyToFolder;
            }
            set
            {
                _copyToFolder = value;
                RaisePropertyChanged(nameof(CopyToFolder));
            }
        }

        bool _selectAll = true;
        public bool SelectAll
        {
            get
            {
                return _selectAll;
            }
            set
            {
                _selectAll = value;
                foreach (var sheet in SheetInfo)
                {
                    sheet.ToPrint = _selectAll;
                }
                RaisePropertyChanged(nameof(SelectAll));
            }
        }

        string _filterBy = "";
        public string FilterBy
        {
            get
            {
                return _filterBy;
            }
            set
            {
                _filterBy = value;
                RaisePropertyChanged(nameof(FilterBy));
            }
        }


        public BatchPrintViewModel(ExternalCommandData commandData)
        {
            this.commandData = commandData;

            var doc = commandData.Application.ActiveUIDocument.Document;
            List<Element> mySheets = new List<Element>();
            FilteredElementCollector sheets = new FilteredElementCollector(doc);
            mySheets.AddRange(sheets.OfClass(typeof(ViewSheet)).ToElements());
            string output = "Sheets printed: " + Environment.NewLine;
            SheetInfo = new ObservableCollection<sheetVM>();

            foreach (var sheet in mySheets)
            {
                string sheetName = sheet.get_Parameter(BuiltInParameter.SHEET_NAME).AsString();
                string sheetNumber = sheet.get_Parameter(BuiltInParameter.SHEET_NUMBER).AsString();
                string currentRev = sheet.get_Parameter(BuiltInParameter.SHEET_CURRENT_REVISION).AsString();
                string currentRevDate = sheet.get_Parameter(BuiltInParameter.SHEET_CURRENT_REVISION_DATE).AsString();
                string currentRevDescript = sheet.get_Parameter(BuiltInParameter.SHEET_CURRENT_REVISION_DESCRIPTION).AsString();
                SheetInfo.Add(new sheetVM { Name = sheetName, Number = sheetNumber, Rev = currentRev, RevDate = currentRevDate, RevDescript = currentRevDescript, Sheet = sheet as ViewSheet, IncludeName=false, IncludeRev=false });
            }
        }

        private void filter()
        {
            foreach (var sheet in SheetInfo)
            {
                if (sheet.Number.Contains(FilterBy))
                {
                    sheet.ToPrint = true;
                }
                else
                {
                    sheet.ToPrint = false;
                }
            };
        }

        void batchPrint()
        {
            Transaction trans = new Transaction(commandData.Application.ActiveUIDocument.Document, "WW_PDF");
            trans.Start();

            string printSetting = "A1 TO A1";

            var doc = commandData.Application.ActiveUIDocument.Document;
            string filePath = @"C:\Users\Alex Baalham\Documents\";

            var printM = commandData.Application.ActiveUIDocument.Document.PrintManager;
            printM.SelectNewPrintDriver("Bluebeam PDF");

            var ps = new FilteredElementCollector(doc);
            List<Element> myPrintSettings = ps.OfClass(typeof(PrintSetting)).ToList();
            foreach (var setting in myPrintSettings)
            {
                if (setting.Name == printSetting)
                {
                    printM.PrintSetup.CurrentPrintSetting = setting as PrintSetting;
                }
            }

            printM.PrintToFile = true;

            string dest = "";
            if (CopyToFolder)
            {
                FolderBrowserDialog folderDlg = new FolderBrowserDialog();
                folderDlg.ShowNewFolderButton = true;
                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    dest = folderDlg.SelectedPath;
                }
            }


            //List<Element> mySheets = new List<Element>();
            //FilteredElementCollector sheets = new FilteredElementCollector(doc);
            //mySheets.AddRange(sheets.OfClass(typeof(ViewSheet)).ToElements());
            string output = "Sheets printed: " + Environment.NewLine;
            List<string> printedFileNames = new List<string>();

            foreach (var sheet in SheetInfo)
            {
                if (sheet.ToPrint)
                {
                    output += sheet.Number + " " + sheet.Name + Environment.NewLine;
                    var fp = sheet.FilePath;
                    printM.PrintToFileName = fp;
                    printedFileNames.Add(fp);
                    printM.SubmitPrint(sheet.Sheet);
                }
            }
            //System.Windows.MessageBox.Show(output);

            if (CopyToFolder)
            {
                foreach (var fileName in printedFileNames)
                {
                    int waitCycles = 0;
                    CannotFindFileVM vm = new CannotFindFileVM();
                    while (!File.Exists(fileName))
                    {
                        Thread.Sleep(1000);
                        waitCycles++;
                        if (waitCycles > 15)
                        {
                            var skipFile = new Window();
                            var userControl = new ContinueLooking(vm);
                            var window = new Window()
                            {
                                Content = userControl
                            };
                            window.ShowDialog();
                        }
                        if (vm.SkipAll == true || vm.SkipOne == true)
                        {
                            break;
                        }
                    }
                    if (vm.SkipOne == false && vm.SkipAll == false)
                    {
                        string file = Path.GetFileName(fileName);
                        File.Copy(fileName, dest + @"\" + file, true);
                    }
                    else
                    {
                        if (vm.SkipAll == true)
                        {
                            break;
                        }
                    }

                }
            }
            trans.Commit();
        }
    }
    
    public class sheetVM : ViewModelBase
    {
        public ViewSheet Sheet { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Rev { get; set; }
        public string RevDate { get; set; }
        public string RevDescript { get; set; }

        string _comment;
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
                RaisePropertyChanged(nameof(Comment));
                RaisePropertyChanged(nameof(FilePath));
            }
        }

        string _prefix;
        public string Prefix
        {
            get
            {
                return _prefix;
            }
            set
            {
                _prefix = value;
                RaisePropertyChanged(nameof(Prefix));
                RaisePropertyChanged(nameof(FilePath));
            }
        }

        bool _includeRev;
        public bool IncludeRev
        { get
            {
                return _includeRev;
            }
            set
            {
                _includeRev = value;
                RaisePropertyChanged(nameof(IncludeRev));
                RaisePropertyChanged(nameof(FilePath));
            }
        }
        bool _includeName;
        public bool IncludeName
        {
            get
            {
                return _includeName;
            }
            set
            {
                _includeName = value;
                RaisePropertyChanged(nameof(IncludeName));
                RaisePropertyChanged(nameof(FilePath));
            }
        }
        bool _toPrint = true;
        public bool ToPrint
        {
            get
            {
                return _toPrint;
            }
            set
            {
                _toPrint = value;
                RaisePropertyChanged(nameof(ToPrint));
            }
        }
        public string FilePath
        {
            get
            {
                string fp = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\";
                fp += this.Prefix;
                fp += this.Number;
                if (IncludeRev)
                {
                    fp += "-" + this.Rev;
                }
                if (IncludeName)
                {
                    fp += "-" + this.Name;
                }
                fp += Comment;
                fp += @".pdf";
                return fp;
            }
        }
    }
}
