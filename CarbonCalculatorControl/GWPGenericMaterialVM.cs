using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CarbonMaterials;

namespace CarbonCalculator
{
    public class GWPGenericMaterialVM : ViewModelBase
    {
        GWPMaterial _material;

        public string Name => _material.Name;

        public string LinkReference => (_material as GWPGeneric).LinkReference;

        public string A1toA3 => string.Format("{0:0.0}", _material.A1toA3.Value);
        public double A4 => _material.A4.Value;
        public double A5 => _material.A5.Value;
        public double B1 => _material.B1.Value;
        public double B2 => _material.B2.Value;
        public double B3 => _material.B3.Value;
        public double B4 => _material.B4.Value;
        public double B5 => _material.B5.Value;
        public double B6 => _material.B6.Value;
        public double B7 => _material.B7.Value;
        public double C1 => _material.C1.Value;
        public double C2 => _material.C2.Value;
        public double C3 => _material.C3.Value;
        public double C4 => _material.C4.Value;

        public string TotalCarbon
        {
            get
            {
                return string.Format("{0:0.0}", _material.TotalAtoC) + "kg CO2e / m3";
            }
        }

        public List<string> EPDInfo
        {
            get
            {
                return GWPGeneric.EPDs;
            }
        }

        int _selectedEPDIndex;
        public int SelectedEPDIndex
        {
            get
            {
                return _selectedEPDIndex;
            }
            set
            {
                _selectedEPDIndex = value;
                var tempMat = GWPGeneric.getFromEPDLibrary(EPDInfo[_selectedEPDIndex]);
                _material.copyMaterialFrom(tempMat);
                updateAll();
            }
        }

        public string SelectedEPD
        {
            get
            {
                return EPDInfo[_selectedEPDIndex];
            }
        }

        void updateAll()
        {
            RaisePropertyChanged("");



        }

        public GWPGenericMaterialVM(GWPGeneric material)
        {
            _material = material;

        }

        bool _accepted = false;
        public bool Accepted
        {
            get
            {
                return _accepted;
            }
        }


        ICommand _acceptAndCloseCommand;

        public ICommand AcceptAndCloseCommand
        {
            get
            {
                return _acceptAndCloseCommand ?? (_acceptAndCloseCommand = new CommandHandlerWithParameter(new Action<object>(acceptAndClose), true));
            }
        }

        private void acceptAndClose(object obj)
        {
            var win = obj as Window;
            _accepted = true;
            win.Close();
        }

        ICommand _cancelAndCloseCommand;

        public ICommand CancelAndCloseCommand
        {
            get
            {
                return _cancelAndCloseCommand ?? (_cancelAndCloseCommand = new CommandHandlerWithParameter(new Action<object>(cancelAndClose), true));
            }
        }

        private void cancelAndClose(object obj)
        {
            var win = obj as Window;
            _accepted = false;
            win.Close();
        }
    }
}
