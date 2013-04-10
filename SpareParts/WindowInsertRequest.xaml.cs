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
    /// Interaction logic for WindowInsertRequest.xaml
    /// </summary>
    public partial class WindowInsertRequest : Window
    {
        private SparePartsEntities _entities;
        private RequestsObservableCollection _requestsCollection;
        private ListCollectionView _view;

        public WindowInsertRequest()
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

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            ClearStatusbar();

            if (TextBoxResolutionPartNo.Text.Length == 0 && TextBoxPartNo.Text.Length == 0 &&
                TextBoxPartNoOriginal.Text.Length == 0)
            {
                ShowMessageInStatusbar("you should provide at least one part no");
                return;
            }

            if (DatePickerRequestDate.SelectedValue == null)
            {
                ShowMessageInStatusbar("Request date can not be empty");
                return;
            }

            if (NumericUpDownQty.Value == null)
            {
                ShowMessageInStatusbar("Qty can not be empty");
                return;
            }

            RequestWithNotify requestWithNotify=new RequestWithNotify();
            requestWithNotify.ResolutionPartNo = TextBoxResolutionPartNo.Text;
            requestWithNotify.PartNo = TextBoxPartNo.Text;
            requestWithNotify.PartNoOriginal = TextBoxPartNoOriginal.Text;
            requestWithNotify.RequestDate = (DateTime) DatePickerRequestDate.SelectedValue;
            requestWithNotify.Qty = (int) NumericUpDownQty.Value;
            requestWithNotify.EntranceDate = DatePickerEntranceDate.SelectedValue;
            requestWithNotify.Description = TextBoxDescription.Text;

            requestWithNotify.RequestStatus = (RequestStatus) ComboBoxRequestStatus.SelectedIndex;

            var result= RequestsCollection.AddNew(0, requestWithNotify);

            if (result)
            {
                ShowMessageInStatusbar("new requestd added");
            }
            else
            {
                ShowMessageInStatusbar("Failed");
            }
        }

        private void ButtonClear_OnClick(object sender, RoutedEventArgs e)
        {
            TextBoxResolutionPartNo.Text = null;
            TextBoxPartNo.Text = null;
            TextBoxPartNoOriginal.Text = null;
            DatePickerRequestDate.SelectedValue = null;
            NumericUpDownQty.Value = null;
            DatePickerEntranceDate.SelectedValue = null;
            TextBoxDescription.Text = null;
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
