using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MagmaWorksToolbar
{
    public class ImportOptionsVM : ViewModelBase
    {
        Window window;
        
        public bool ExplodeWalls { get; set; } = false;
        public bool ExplodeFloors { get; set; } = false;

        public ImportOptionsVM(Window window)
        {
            this.window = window;
        }

        ICommand _closeWindowCommand;

        public ICommand CloseWindowCommand
        {
            get
            {
                return _closeWindowCommand ?? (_closeWindowCommand = new CommandHandler(() => closeWindow(), true));
            }
        }

        void closeWindow()
        {
            window.Close();
        }
    }
}
