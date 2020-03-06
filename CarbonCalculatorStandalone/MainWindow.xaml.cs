using CarbonCalculator;
using System.Windows;


namespace CarbonCalculatorStandalone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var myelemset = new ElementSet("Category", "Material", "Levels");
            myelemset.AddElement(new Element("Elem 1", 120d, "Revit123", "Foundation", "Concrete", "Level 1"));
            myelemset.AddElement(new Element("Elem 2", 109.4d, "Revit124", "Foundation", "Concrete", "Level2"));
            myelemset.AddElement(new Element("Elem 3", 24.2d, "Revit125", "Walls", "RC40", "Level 1"));

            //ModelVM vm = new ModelVM(myelemset);
            var vm = new AppVM(myelemset);

            DataContext = vm;
            InitializeComponent();
        }
    }
}
