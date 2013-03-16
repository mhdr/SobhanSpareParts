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
    /// Interaction logic for WindowRequests.xaml
    /// </summary>
    public partial class WindowRequests : Window
    {
        private SparePartsEntities _entities=new SparePartsEntities();
        private RequestsObservableCollection _requestsCollection;
        private ListCollectionView _view;

        public WindowRequests()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public RequestsObservableCollection RequestsCollection
        {
            get { return _requestsCollection; }
            set { _requestsCollection = value; }
        }

        public ListCollectionView View
        {
            get { return _view; }
            set { _view = value; }
        }

        private void RibbonButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            WindowInsertRequest windowInsertRequest=new WindowInsertRequest();
            windowInsertRequest.Entities = Entities;
            windowInsertRequest.RequestsCollection = RequestsCollection;
            windowInsertRequest.View = View;
            windowInsertRequest.Show();
        }

        private void RibbonButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (View.CurrentItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            WindowEditRequest windowEditRequest=new WindowEditRequest();
            windowEditRequest.Entities = Entities;
            windowEditRequest.RequestsCollection = RequestsCollection;
            windowEditRequest.View = View;
            windowEditRequest.Index = View.CurrentPosition;
            windowEditRequest.RequestToEdit = (RequestWithNotify) View.CurrentItem;
            windowEditRequest.Show();
        }

        private void RibbonButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (View.CurrentItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            var result = RequestsCollection.Delete(View.CurrentPosition);

            ClearStatusbar();
            if (result)
            {
                ShowMessageInStatusbar("Request removed");
            }
            else
            {
                ShowMessageInStatusbar("Failed");
            }
        }

        private void WindowRequests_OnLoaded(object sender, RoutedEventArgs e)
        {
            BindGridViewRequests();
        }

        private void BindGridViewRequests()
        {
            var requestQuery = from request in Entities.Requests
                               orderby request.RequestId descending
                               select request;

            RequestsCollection = new RequestsObservableCollection(requestQuery.ToList(), Entities);
            CollectionViewSource requestSource = (CollectionViewSource)FindResource("RequestsSource");
            requestSource.Source = RequestsCollection;
            View = (ListCollectionView) requestSource.View;
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
