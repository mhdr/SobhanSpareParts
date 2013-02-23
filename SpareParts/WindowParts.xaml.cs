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
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace SpareParts
{
    /// <summary>
    /// Interaction logic for WindowParts.xaml
    /// </summary>
    public partial class WindowParts : Window
    {
        private SparePartsEntities _entities=new SparePartsEntities();

        public WindowParts()
        {
            StyleManager.ApplicationTheme = new Office_BlackTheme();
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private void WindowParts_OnLoaded(object sender, RoutedEventArgs e)
        {

            GridViewParts.ItemsSource = Entities.Parts;
        }

        private void RibbonButtonBrands_OnClick(object sender, RoutedEventArgs e)
        {
            WindowBrands windowBrands=new WindowBrands();
            windowBrands.Show();
        }
    }
}
