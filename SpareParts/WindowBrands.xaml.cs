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

namespace SpareParts
{
    /// <summary>
    /// Interaction logic for WindowBrands.xaml
    /// </summary>
    public partial class WindowBrands : Window
    {
        private SparePartsEntities _entities=new SparePartsEntities();
        private Brand _currentBrand = null;

        public WindowBrands()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public Brand CurrentBrand
        {
            get { return _currentBrand; }
            set { _currentBrand = value; }
        }

        private void WindowBrands_OnLoaded(object sender, RoutedEventArgs e)
        {
            GridViewBrands.ItemsSource = Entities.Brands;
        }

        private void GridViewBrands_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            CurrentBrand = (Brand) GridViewBrands.SelectedItem;

            if (CurrentBrand == null)
            {
                RibbonButtonDelete.IsEnabled = false;
                RibbonButtonEdit.IsEnabled = false;
            }
            else
            {
                RibbonButtonDelete.IsEnabled = true;
                RibbonButtonEdit.IsEnabled = true;
            }
        }

        private void RibbonButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            Entities.Brands.Remove(CurrentBrand);
            Entities.SaveChanges();
            Entities=new SparePartsEntities();
            GridViewBrands.ItemsSource = Entities.Brands;
        }
    }
}
