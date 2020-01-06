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

namespace MagmaWorksToolbar
{
    /// <summary>
    /// Interaction logic for ContinueLooking.xaml
    /// </summary>
    public partial class ContinueLooking : UserControl
    {
        CannotFindFileVM vm;

        public ContinueLooking(CannotFindFileVM vm)
        {
            this.vm = vm;
            this.DataContext = vm;
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            vm.SkipAll = false;
            vm.SkipOne = false;
            var window = Window.GetWindow(this);
            window.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            vm.SkipAll = false;
            vm.SkipOne = true;
            var window = Window.GetWindow(this);
            window.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            vm.SkipAll = true;
            vm.SkipOne = false;
            var window = Window.GetWindow(this);
            window.Close();
        }
    }
}
