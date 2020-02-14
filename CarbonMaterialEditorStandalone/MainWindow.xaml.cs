using CarbonCalculator;
using CarbonMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarbonMaterialEditorStandalone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewModelParent
    {
        public MainWindow()
        {
            var mySet = GWPMaterialSet.GetDefaultGWPConcreteSetSeparateSteel();
            MaterialSetVM vm = new MaterialSetVM(mySet, this);
            DataContext = vm;
            InitializeComponent();
        }

        public void UpdateAll()
        {
            
        }
    }
}
