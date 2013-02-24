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
        private Lib.ObservableBrands _brandsCollection;
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

        private void WindowBrands_OnLoaded(object sender, RoutedEventArgs e)
        {
            BrandsCollection=new ObservableBrands(Entities.Brands,Entities);
            var brandSource = (CollectionViewSource) this.FindResource("BrandSource");
            brandSource.Source = BrandsCollection;
            View = (ListCollectionView) brandSource.View;
        }

        private void GridViewBrands_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (View.CurrentItem == null)
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
            View.RemoveAt(View.CurrentPosition);
            Entities.SaveChanges();
        }
    }
}
