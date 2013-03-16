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
    /// Interaction logic for WindowEditRequest.xaml
    /// </summary>
    public partial class WindowEditRequest : Window
    {
        private SparePartsEntities _entities;
        private RequestsObservableCollection _requestsCollection;
        private ListCollectionView _view;
        private RequestWithNotify _requestToEdit;
        private int _index;

        public WindowEditRequest()
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

        public RequestWithNotify RequestToEdit
        {
            get { return _requestToEdit; }
            set { _requestToEdit = value; }
        }

        public int Index
        {
            get { return _index; }
            set { _index = value; }
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

            RequestToEdit.ResolutionPartNo = TextBoxResolutionPartNo.Text;
            RequestToEdit.PartNo = TextBoxPartNo.Text;
            RequestToEdit.PartNoOriginal = TextBoxPartNoOriginal.Text;
            RequestToEdit.RequestDate = (DateTime)DatePickerRequestDate.SelectedValue;
            RequestToEdit.Qty = (int)NumericUpDownQty.Value;
            RequestToEdit.EntranceDate = DatePickerEntranceDate.SelectedValue;
            RequestToEdit.Description = TextBoxDescription.Text;
            var result = RequestsCollection.Update(Index, RequestToEdit);

            if (result)
            {
                ShowMessageInStatusbar("request saved");
            }
            else
            {
                ShowMessageInStatusbar("Failed");
            }
        }

        private void LoadRequest(RequestWithNotify item)
        {
            TextBoxResolutionPartNo.Text = item.ResolutionPartNo;
            TextBoxPartNo.Text = item.PartNo;
            TextBoxPartNoOriginal.Text = item.ResolutionPartNo;
            DatePickerRequestDate.SelectedValue = item.RequestDate;
            NumericUpDownQty.Value = item.Qty;
            DatePickerEntranceDate.SelectedValue = item.EntranceDate;
            TextBoxDescription.Text = item.Description;
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

        private void WindowEditRequest_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadRequest(this.RequestToEdit);
        }
    }
}
