using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarbonMaterials;

namespace CarbonCalculator
{
    public class TransportVM : ViewModelBase
    {
        MaterialTransport _transport;
        IViewModelParent _parent;

        public double TransportDistance
        {
            get
            {
                return _transport.Distance;
            }
            set
            {
                _transport.Distance = value;
                RaisePropertyChanged(nameof(TransportDistance));
                RaisePropertyChanged(nameof(CarbonPerKG));
                _parent.UpdateAll();
            }
        }

        public TransportVM(MaterialTransport transport, IViewModelParent parent)
        {
            _transport = transport;
            _parent = parent;
            _label1 = transport.Definition.Level1;
            _label2 = transport.Definition.Level2;
            _label3 = transport.Definition.Level3;
            _label4 = transport.Definition.Level4;
        }

        public double CarbonPerKG
        {
            get
            {
                return _transport.CarbonPerKG;
            }
        }

        string _label1;
        public string Label1
        {
            get
            {
                return _label1;
            }
            set
            {
                _label1 = value;
                _label2 = Label2Options[0];
                _label3 = Label3Options[0];
                _label4 = Label4Options[0];
                _transport.Definition = TransportDefinition.GetDefinition(_label1, _label2, _label3, _label4);
                RaisePropertyChanged(nameof(Label1));
                RaisePropertyChanged(nameof(Label2));
                RaisePropertyChanged(nameof(Label3));
                RaisePropertyChanged(nameof(Label4));
                RaisePropertyChanged(nameof(Label2Options));
                RaisePropertyChanged(nameof(Label3Options));
                RaisePropertyChanged(nameof(Label4Options));
                RaisePropertyChanged(nameof(CarbonPerKG));
                _parent.UpdateAll();

            }
        }
        public List<string> Label1Options
        {
            get
            {
                return TransportDefinition.Level1Names();
            }
        }

        string _label2;
        public string Label2
        {
            get
            {
                return _label2;
            }
            set
            {
                _label2 = value;
                _label3 = Label3Options[0];
                _label4 = Label4Options[0];
                _transport.Definition = TransportDefinition.GetDefinition(_label1, _label2, _label3, _label4);
                RaisePropertyChanged(nameof(Label2));
                RaisePropertyChanged(nameof(Label3));
                RaisePropertyChanged(nameof(Label4));
                RaisePropertyChanged(nameof(Label3Options));
                RaisePropertyChanged(nameof(Label4Options));
                RaisePropertyChanged(nameof(CarbonPerKG));
                _parent.UpdateAll();

            }
        }
        public List<string> Label2Options
        {
            get
            {
                return TransportDefinition.Level2Names(_label1);
            }
        }

        string _label3;
        public string Label3
        {
            get
            {
                return _label3;
            }
            set
            {
                _label3 = value;
                _label4 = Label4Options[0];
                _transport.Definition = TransportDefinition.GetDefinition(_label1, _label2, _label3, _label4);
                RaisePropertyChanged(nameof(Label3));
                RaisePropertyChanged(nameof(Label4));
                RaisePropertyChanged(nameof(Label4Options));
                RaisePropertyChanged(nameof(CarbonPerKG));
                _parent.UpdateAll();

            }
        }
        public List<string> Label3Options
        {
            get
            {
                return TransportDefinition.Level3Names(_label1, _label2);
            }
        }

        string _label4;
        public string Label4
        {
            get
            {
                return _label4;
            }
            set
            {
                _label4 = value;
                _transport.Definition = TransportDefinition.GetDefinition(_label1, _label2, _label3, _label4);
                RaisePropertyChanged(nameof(Label4));
                RaisePropertyChanged(nameof(CarbonPerKG));
                _parent.UpdateAll();

            }
        }
        public List<string> Label4Options
        {
            get
            {
                return TransportDefinition.Level4Names(_label1, _label2, _label3);
            }
        }
    }
}
