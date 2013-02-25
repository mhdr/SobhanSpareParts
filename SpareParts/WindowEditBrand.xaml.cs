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
    /// Interaction logic for WindowInsertBrand.xaml
    /// </summary>
    public partial class WindowEditBrand : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();
        private Lib.ObservableBrands _brandsCollection;
        private ListCollectionView _view;
        private Brand _brandToEdit;

        public WindowEditBrand()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public ObservableBrands BrandsCollection
        {
            get { return _brandsCollection; }
            set { _brandsCollection = value; }
        }

        public ListCollectionView View
        {
            get { return _view; }
            set { _view = value; }
        }

        public Brand BrandToEdit
        {
            get { return _brandToEdit; }
            set { _brandToEdit = value; }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            ClearStatusbar();

            if (TextBoxBrand.Text.Length > 0)
            {
                if (BrandsCollection.Any(x => x.BrandName.ToLower() == TextBoxBrand.Text.ToLower()))
                {
                    ShowMessageInStatusbar("the brand is already saved");
                    return;
                }


                View.EditItem(BrandToEdit);
                BrandToEdit.BrandName = TextBoxBrand.Text;
                View.CommitEdit();
                BrandsCollection.Notify();

                if (Entities.SaveChanges() > 0)
                {
                    ShowMessageInStatusbar("brand edited");
                    TextBoxBrand.SelectAll();
                    TextBoxBrand.Focus();
                }
            }
        }

        private void WindowInsertBrand_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Entities = BrandsCollection.Entities;
            //BrandsCollection = new ObservableBrands(Entities.Brands, Entities);
            var brandSource = (CollectionViewSource)this.FindResource("BrandSource");
            brandSource.Source = BrandsCollection;
            View = (ListCollectionView)brandSource.View;

            TextBoxBrand.Text = BrandToEdit.BrandName;
            TextBoxBrand.SelectAll();
            TextBoxBrand.Focus();
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
