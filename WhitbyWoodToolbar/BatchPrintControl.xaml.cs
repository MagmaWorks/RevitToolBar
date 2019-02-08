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

namespace WhitbyWoodToolbar
{
    /// <summary>
    /// Interaction logic for BatchPrintControl.xaml
    /// </summary>
    public partial class BatchPrintControl : UserControl
    {
        public BatchPrintControl()
        {
            InitializeComponent();
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var box = sender as TextBox;
            (box.DataContext as sheetVM).Comment = box.Text;
            (box.DataContext as sheetVM).NamesCommand.Execute("Comment");
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            var box = sender as TextBox;
            (box.DataContext as sheetVM).Prefix = box.Text;
            (box.DataContext as sheetVM).NamesCommand.Execute("Prefix");
        }

        private void OnMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            ListBoxItem lbi = sender as ListViewItem;
            if (lbi != null)
            {
                if (lbi.IsSelected)
                {
                    lbi.IsSelected = false;
                    e.Handled = true;
                }
            }
        }
        
    }
}
