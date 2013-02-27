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
                NotifyOpenWindows();
            }
            else
            {
                ClearStatusbar();
                ShowMessageInStatusbar("Failed");
            }
            
        }

        private static void NotifyOpenWindows()
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window.GetType() == typeof (WindowInsertPart))
                {
                    (window as WindowInsertPart).BindComboBoxBrand();
                }

                if (window.GetType() == typeof (WindowEditPart))
                {
                    (window as WindowEditPart).BindComboBoxBrand();
                }
            }
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
            NotifyOpenWindows();
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
            NotifyOpenWindows();
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
