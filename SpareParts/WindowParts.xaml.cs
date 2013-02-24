using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace SpareParts
{
    /// <summary>
    /// Interaction logic for WindowParts.xaml
    /// </summary>
    public partial class WindowParts : Window
    {
        private SparePartsEntities _entities=new SparePartsEntities();
        private Lib.ObservableParts _partsCollection;
        private ListCollectionView _view;
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

        public ObservableParts PartsCollection
        {
            get { return _partsCollection; }
            set { _partsCollection = value; }
        }

        public ListCollectionView View
        {
            get { return _view; }
            set { _view = value; }
        }


        private void WindowParts_OnLoaded(object sender, RoutedEventArgs e)
        {
            PartsCollection=new ObservableParts(Entities.Parts,Entities);
            var partSource = (CollectionViewSource) this.FindResource("PartSource");
            partSource.Source = PartsCollection;
            View = (ListCollectionView) partSource.View;
        }

        private void RibbonButtonBrands_OnClick(object sender, RoutedEventArgs e)
        {
            WindowBrands windowBrands=new WindowBrands();
            windowBrands.Show();
        }

        private void GridViewParts_OnGrouping(object sender, GridViewGroupingEventArgs e)
        {
            if (e.Action == GroupingEventAction.Place)
            {
                e.Cancel = true;
                var c = (ColumnGroupDescriptor)e.GroupDescriptor;
                CountFunction f = new CountFunction();
                f.Caption = "Count: ";
                GroupDescriptor descriptor = new GroupDescriptor();
                descriptor.Member = c.Column.UniqueName;
                descriptor.DisplayContent = c.DisplayContent;
                descriptor.AggregateFunctions.Add(f);
                this.GridViewParts.GroupDescriptors.Add(descriptor);
            }
        }

        private void RibbonButtonMachines_OnClick(object sender, RoutedEventArgs e)
        {
            WindowMachines windowMachines=new WindowMachines();
            windowMachines.Show();
        }
    }
}
