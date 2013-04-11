using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SpareParts.Lib;
using Telerik.Windows.Controls;

namespace SpareParts
{
    /// <summary>
    /// Interaction logic for WindowRequests.xaml
    /// </summary>
    public partial class WindowRequests : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();
        private RequestsObservableCollection _requestsCollection;
        private ListCollectionView _view;
        public event EventHandler WindowLoaded;

        protected virtual void OnWindowLoaded()
        {
            EventHandler handler = WindowLoaded;
            if (handler != null) handler(this, EventArgs.Empty);
        }

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
            OpenWindowInsertRequest();
        }

        public void OpenWindowInsertRequest()
        {
            WindowInsertRequest windowInsertRequest = new WindowInsertRequest();
            windowInsertRequest.Entities = Entities;
            windowInsertRequest.RequestsCollection = RequestsCollection;
            windowInsertRequest.View = View;
            windowInsertRequest.Show();
        }

        public void OpenWindowInsertRequest(PartWithNotify part)
        {
            WindowInsertRequest windowInsertRequest = new WindowInsertRequest();
            windowInsertRequest.Entities = Entities;
            windowInsertRequest.RequestsCollection = RequestsCollection;
            windowInsertRequest.View = View;
            windowInsertRequest.PartNo = part.PartNo;
            windowInsertRequest.PartNoOrignal = part.PartNoOrignal;
            windowInsertRequest.ResolutionPartNo = part.ResolutionPartNo;
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

            WindowEditRequest windowEditRequest = new WindowEditRequest();
            windowEditRequest.Entities = Entities;
            windowEditRequest.RequestsCollection = RequestsCollection;
            windowEditRequest.View = View;
            windowEditRequest.Index = View.CurrentPosition;
            windowEditRequest.RequestToEdit = (RequestWithNotify)View.CurrentItem;
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
            OnWindowLoaded();
        }

        private void BindGridViewRequests()
        {
            var requestQuery = from request in Entities.Requests
                               orderby request.RequestId descending
                               select request;

            RequestsCollection = new RequestsObservableCollection(requestQuery.ToList(), Entities);
            CollectionViewSource requestSource = (CollectionViewSource)FindResource("RequestsSource");
            requestSource.Source = RequestsCollection;
            View = (ListCollectionView)requestSource.View;
        }

        private void ShowMessageInStatusbar(string msg)
        {
            StatusBar1.Items.Add(msg);
        }

        private void ClearStatusbar()
        {
            StatusBar1.Items.Clear();
        }

        public void FilterPartNo(string partNo)
        {
            var partNoColumn = this.GridViewRequests.Columns["PartNo"];
            partNoColumn.ColumnFilterDescriptor.Clear();
            var partNoFilter = partNoColumn.ColumnFilterDescriptor;
            partNoFilter.DistinctFilter.AddDistinctValue(partNo);
        }

        public void FilterPartNoOriginal(string partNo)
        {
            var partNoColumn = this.GridViewRequests.Columns["PartNoOriginal"];
            partNoColumn.ColumnFilterDescriptor.Clear();
            var partNoFilter = partNoColumn.ColumnFilterDescriptor;
            partNoFilter.DistinctFilter.AddDistinctValue(partNo);
        }

        public void FilterResultionPartNo(string partNo)
        {
            var partNoColumn = this.GridViewRequests.Columns["ResolutionPartNo"];
            partNoColumn.ColumnFilterDescriptor.Clear();
            var partNoFilter = partNoColumn.ColumnFilterDescriptor;
            partNoFilter.DistinctFilter.AddDistinctValue(partNo);
        }

        private void RibbonButtonExportInitializes_OnClick(object sender, RoutedEventArgs e)
        {
            FileInfo fileInfo = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Workbook| *.xls;*.xlsx";
            if (saveFileDialog.ShowDialog() == true)
            {
                fileInfo = new FileInfo(saveFileDialog.FileName);
            }

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            ExcelPackage excelPackage = new ExcelPackage(fileInfo);
            var ws = excelPackage.Workbook.Worksheets.Add("Parts");
            ws.View.RightToLeft = true;
            ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            ws.Cells["A1"].Value = "نام قطعه";
            ws.Cells["B1"].Value = "شماره فنی";
            ws.Cells["C1"].Value = "شرکت سازنده";
            ws.Cells["D1"].Value = "شماره فنی شرکت سازنده";
            ws.Cells["E1"].Value = "دستگاه";
            ws.Cells["F1"].Value = "تعداد";
            ws.Cells["A1:F1"].Style.Font.Bold = true;

            var requests = RequestsCollection.Where(x => x.RequestStatus == RequestStatus.Initialize);

            int i = 2;
            foreach (var requestWithNotify in requests)
            {
                IEnumerable<Part> parts = null;
                Part firstPart = null;

                string partNo = "";

                if (requestWithNotify.PartNo.Length > 0)
                {
                    parts = Entities.Parts.Where(x => x.PartNo == requestWithNotify.PartNo);
                    partNo = requestWithNotify.PartNo;
                }
                else if (requestWithNotify.ResolutionPartNo.Length > 0)
                {
                    parts = Entities.Parts.Where(x => x.ResolutionPartNo == requestWithNotify.ResolutionPartNo);
                    partNo = requestWithNotify.ResolutionPartNo;
                }
                else if (requestWithNotify.PartNoOriginal.Length > 0)
                {
                    parts = Entities.Parts.Where(x => x.PartNoOrignal == requestWithNotify.PartNoOriginal);
                    partNo = requestWithNotify.PartNoOriginal;
                }

                firstPart = parts.FirstOrDefault();

                ws.Cells[string.Format("A{0}", i)].Value = firstPart.PartName;
                ws.Cells[string.Format("B{0}", i)].Value = partNo;
                ws.Cells[string.Format("C{0}", i)].Value = firstPart.Brand.BrandName;
                ws.Cells[string.Format("D{0}", i)].Value = firstPart.PartNoOrignal;

                var machines = from part in parts
                               select part.Machine.MachineName;
                var machinesDistinct = machines.Distinct();

                string machinesComma = string.Join(",", machinesDistinct);

                ws.Cells[string.Format("E{0}", i)].Value = machinesComma;
                ws.Cells[string.Format("F{0}", i)].Value = requestWithNotify.Qty;
                i++;
            }

            excelPackage.Save();

        }
    }
}
