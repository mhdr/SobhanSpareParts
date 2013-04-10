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
    public partial class WindowInsertBrand : Window
    {
        private SparePartsEntities _entities;
        private BrandsObservableCollection _brandsCollection;
        private ListCollectionView _view;

        public WindowInsertBrand()
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

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            ClearStatusbar();

            if (TextBoxBrand.Text.Length > 0)
            {
                if (Entities.Brands.Any(x => x.BrandName.ToLower() == TextBoxBrand.Text.ToLower()))
                {
                    ShowMessageInStatusbar("the brand is already added");
                    return;
                }


                BrandWithINotify newBrand = new BrandWithINotify();
                newBrand.BrandName = TextBoxBrand.Text;
                bool result = BrandsCollection.AddNew(0, newBrand);

                if (result)
                {
                    NotifyOpenWindows();
                    TextBoxBrand.Text = "";
                    ShowMessageInStatusbar("new brand added");
                }
            }
        }

        private void WindowInsertBrand_OnLoaded(object sender, RoutedEventArgs e)
        {
            TextBoxBrand.Focus();
        }

        private void ShowMessageInStatusbar(string msg)
        {
            StatusBar1.Items.Add(msg);
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

        private void ClearStatusbar()
        {
            StatusBar1.Items.Clear();
        }

    }
}
