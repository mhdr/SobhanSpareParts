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
using SpareParts.Lib;

namespace SpareParts
{
    /// <summary>
    /// Interaction logic for WindowBrands.xaml
    /// </summary>
    public partial class WindowBrands : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();
        private BrandsCollection _brandsCollection;
        private ListCollectionView _view;

        public WindowBrands()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public BrandsCollection BrandsCollection
        {
            get { return _brandsCollection; }
            set { _brandsCollection = value; }
        }

        public ListCollectionView View
        {
            get { return _view; }
            set { _view = value; }
        }

        private void WindowBrands_OnLoaded(object sender, RoutedEventArgs e)
        {
            BindGirdViewBrands();
        }

        private void BindGirdViewBrands()
        {
            BrandsCollection brandsCollection = new BrandsCollection(Entities.Brands, Entities);
            var brandsSource = (CollectionViewSource)FindResource("BrandsSource");
            brandsSource.Source = brandsCollection;
            View = (ListCollectionView)brandsSource.View;
        }

        private void RibbonButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (GridViewBrands.SelectedItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            var selectedBrand = GridViewBrands.SelectedItem as Brand;
            if (Entities.Parts.Any(x => x.BrandId == selectedBrand.BrandId))
            {
                ClearStatusbar();
                ShowMessageInStatusbar("This brand is in use");
                return;
            }

            Entities.Brands.Remove((Brand) GridViewBrands.SelectedItem);
            if (Entities.SaveChanges() > 0)
            {
                BindGirdViewBrands();
                ClearStatusbar();
                ShowMessageInStatusbar("Barnd removed");
                //NotifyOpenWindows();
            }
            else
            {
                ClearStatusbar();
                ShowMessageInStatusbar("Failed");
            }
            
        }

        private void RibbonButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            WindowInsertBrand windowInsertBrand=new WindowInsertBrand();
            windowInsertBrand.Entities = Entities;
            windowInsertBrand.BrandsCollection = BrandsCollection;
            windowInsertBrand.View = View;
            windowInsertBrand.Show();
        }

        private void RibbonButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (GridViewBrands.SelectedItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            WindowEditBrand windowEditBrand=new WindowEditBrand();
            windowEditBrand.BrandToEdit = (Brand) GridViewBrands.SelectedItem;
            windowEditBrand.DataBaseUpdated += windowEditBrand_DataBaseUpdated;
            windowEditBrand.Show();
        }

        void windowEditBrand_DataBaseUpdated(object sender, EventArgs e)
        {
            Entities = new SparePartsEntities();
            BindGirdViewBrands();
            //NotifyOpenWindows();
        }

        private void ShowMessageInStatusbar(string msg)
        {
            StatusBar1.Items.Add(msg);
        }

        private void ClearStatusbar()
        {
            StatusBar1.Items.Clear();
        }
    }
}
