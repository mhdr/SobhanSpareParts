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
        private SparePartsEntities _entities = new SparePartsEntities();

        public WindowBrands()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private void WindowBrands_OnLoaded(object sender, RoutedEventArgs e)
        {
            BindGirdViewBrands();
        }

        private void BindGirdViewBrands()
        {
            GridViewBrands.ItemsSource = Entities.Brands.ToList();
        }

        private void GridViewBrands_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (GridViewBrands.SelectedItem==null)
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
            Entities.Brands.Remove((Brand) GridViewBrands.SelectedItem);
            Entities.SaveChanges();
            BindGirdViewBrands();
        }

        private void RibbonButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            WindowInsertBrand windowInsertBrand=new WindowInsertBrand();
            windowInsertBrand.DataBaseUpdated += windowInsertBrand_DataBaseUpdated;
            windowInsertBrand.Show();
        }

        void windowInsertBrand_DataBaseUpdated(object sender, EventArgs e)
        {
            BindGirdViewBrands();
        }

        private void RibbonButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            WindowEditBrand windowEditBrand=new WindowEditBrand();
            windowEditBrand.BrandToEdit = (Brand) GridViewBrands.SelectedItem;
            windowEditBrand.DataBaseUpdated += windowEditBrand_DataBaseUpdated;
            windowEditBrand.Show();
        }

        void windowEditBrand_DataBaseUpdated(object sender, EventArgs e)
        {
            Entities = new SparePartsEntities();
            BindGirdViewBrands();
        }
    }
}
