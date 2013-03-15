using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        private SparePartsEntities _entities = new SparePartsEntities();
        private ListCollectionView _view;
        private PartsObservableCollection _partsCollection;

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

        public ListCollectionView View
        {
            get { return _view; }
            set { _view = value; }
        }

        public PartsObservableCollection PartsCollection
        {
            get { return _partsCollection; }
            set { _partsCollection = value; }
        }

        private void WindowParts_OnLoaded(object sender, RoutedEventArgs e)
        {
            BindGridViewParts();
        }

        private void BindGridViewParts()
        {
            var partsQuery = from part in Entities.Parts
                             orderby part.PartId descending
                             select part;
            PartsCollection = new PartsObservableCollection(partsQuery.ToList(), Entities);
            CollectionViewSource partsSource = (CollectionViewSource)FindResource("PartsSource");
            partsSource.Source = PartsCollection;
            View = (ListCollectionView)partsSource.View;
        }

        private void RibbonButtonBrands_OnClick(object sender, RoutedEventArgs e)
        {
            WindowBrands windowBrands = new WindowBrands();
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
            WindowMachines windowMachines = new WindowMachines();
            windowMachines.Show();
        }

        private void RibbonButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            WindowInsertPart windowInsertPart = new WindowInsertPart();
            windowInsertPart.Entities = this.Entities;
            windowInsertPart.PartsCollection = PartsCollection;
            windowInsertPart.View = this.View;
            windowInsertPart.Show();
        }

        private void RibbonButtonRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            Entities = new SparePartsEntities();
            BindGridViewParts();
        }

        private void RibbonButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (View.CurrentItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            var result = PartsCollection.Delete(View.CurrentPosition);

            ClearStatusbar();
            if (result)
            {
                ShowMessageInStatusbar("Part removed");
            }
            else
            {
                ShowMessageInStatusbar("Failed");
            }
        }

        private void ShowMessageInStatusbar(string msg)
        {
            StatusBar1.Items.Add(msg);
        }

        private void ClearStatusbar()
        {
            StatusBar1.Items.Clear();
        }

        private void RibbonButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (GridViewParts.SelectedItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            WindowEditPart windowEditPart = new WindowEditPart();
            windowEditPart.Entities = this.Entities;
            windowEditPart.PartsCollection = PartsCollection;
            windowEditPart.View = this.View;
            windowEditPart.PartToEdit = View.CurrentItem as PartWithNotify;
            windowEditPart.Index = View.CurrentPosition;
            windowEditPart.Show();
        }

        private void RibbonButtonPartNo_OnClick(object sender, RoutedEventArgs e)
        {
            if (View.CurrentItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            Clipboard.SetText((View.CurrentItem as PartWithNotify).PartNo);
        }

        private void RibbonButtonLocation_OnClick(object sender, RoutedEventArgs e)
        {
            if (View.CurrentItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            Clipboard.SetText((View.CurrentItem as PartWithNotify).Location);
        }

        private void RibbonButtonTagName_OnClick(object sender, RoutedEventArgs e)
        {
            if (View.CurrentItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            Clipboard.SetText((View.CurrentItem as PartWithNotify).TagName);
        }

        private void RibbonButtonResolutionPartNo_OnClick(object sender, RoutedEventArgs e)
        {
            if (View.CurrentItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            Clipboard.SetText((View.CurrentItem as PartWithNotify).ResolutionPartNo);
        }

        private void RibbonButtonPartName_OnClick(object sender, RoutedEventArgs e)
        {
            if (View.CurrentItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            Clipboard.SetText((View.CurrentItem as PartWithNotify).PartName);
        }

        private void RibbonButtonPartNoOrignal_OnClick(object sender, RoutedEventArgs e)
        {
            if (View.CurrentItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            Clipboard.SetText((View.CurrentItem as PartWithNotify).PartNoOrignal);
        }

        private void RibbonButtonRequests_OnClick(object sender, RoutedEventArgs e)
        {
            WindowRequests windowRequests = new WindowRequests();
            windowRequests.Show();
        }

        private void RibbonButtonAnalyzeRequests_OnClick(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(AnalyzeRequests);
            thread.Start();
        }

        private void AnalyzeRequests()
        {
            if (PartsCollection.Count == 0)
            {
                return;
            }

            foreach (var part in PartsCollection)
            {
                if (part.ResolutionPartNo != null)
                {
                    var requestsResolutionPartNo = Entities.Requests.Where(x => x.EntranceDate == null & x.ResolutionPartNo == part.ResolutionPartNo);
                    int qtyResolutionPartNo = Enumerable.Sum(requestsResolutionPartNo, request => request.Qty);
                    part.PendingRequestsResolutionPartNo = qtyResolutionPartNo;
                }

                if (part.PartNo != null)
                {
                    var requestsPartNo = Entities.Requests.Where(x => x.EntranceDate == null & x.PartNo == part.PartNo);
                    int qtyPartNo = Enumerable.Sum(requestsPartNo, request => request.Qty);
                    part.PendingRequestsPartNo = qtyPartNo;
                }

            }
        }
    }
}
