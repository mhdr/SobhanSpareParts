using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private BrandsObservableCollection _brandsCollection;
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

        public BrandsObservableCollection BrandsCollection
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
            var brandsQuery = from brand in Entities.Brands
                              orderby brand.BrandName ascending
                              select brand;
            BrandsCollection = new BrandsObservableCollection(brandsQuery.ToList(), Entities);
            var brandsSource = (CollectionViewSource)FindResource("BrandsSource");
            brandsSource.Source = BrandsCollection;
            View = (ListCollectionView)brandsSource.View;
            View.SortDescriptions.Add(new SortDescription("BrandName", ListSortDirection.Ascending));
        }

        private void RibbonButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (GridViewBrands.SelectedItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            var selectedBrand = View.CurrentItem as BrandWithINotify;
            if (Entities.Parts.Any(x => x.BrandId == selectedBrand.BrandId))
            {
                ClearStatusbar();
                ShowMessageInStatusbar("This brand is in use");
                return;
            }

            bool result = BrandsCollection.Delete(View.CurrentPosition);

            if (result)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("Barnd removed");
                NotifyOpenWindows();
            }
            else
            {
                ClearStatusbar();
                ShowMessageInStatusbar("Failed");
            }

        }

        private void RibbonButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            WindowInsertBrand windowInsertBrand = new WindowInsertBrand();
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

            WindowEditBrand windowEditBrand = new WindowEditBrand();
            windowEditBrand.Entities = Entities;
            windowEditBrand.BrandsCollection = BrandsCollection;
            windowEditBrand.View = View;
            windowEditBrand.BrandToEdit = (BrandWithINotify) View.CurrentItem;
            windowEditBrand.Show();
        }

        private void ShowMessageInStatusbar(string msg)
        {
            StatusBar1.Items.Add(msg);
        }

        private void ClearStatusbar()
        {
            StatusBar1.Items.Clear();
        }

        private static void NotifyOpenWindows()
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(WindowInsertPart))
                {
                    (window as WindowInsertPart).BindComboBoxBrand();
                }

                if (window.GetType() == typeof(WindowEditPart))
                {
                    (window as WindowEditPart).BindComboBoxBrand();
                }
            }
        }
    }
}
