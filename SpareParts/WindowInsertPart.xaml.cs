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
    /// Interaction logic for WindowInsertPart.xaml
    /// </summary>
    public partial class WindowInsertPart : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();
        private PartsObservableCollection _partsCollection;
        private ListCollectionView _view;

        public WindowInsertPart()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public PartsObservableCollection PartsCollection
        {
            get { return _partsCollection; }
            set { _partsCollection = value; }
        }

        public ListCollectionView View
        {
            get { return _view; }
            set { _view = value; }
        }

        private void WindowInsertPart_OnLoaded(object sender, RoutedEventArgs e)
        {
            TextBoxPartNo.Focus();

            BindComboBoxBrand();
            BindComboBoxMachine();
        }

        public void BindComboBoxMachine()
        {
            var machineQuery = from machine in Entities.Machines
                               orderby machine.MachineName ascending
                               select machine;
            ComboBoxMachine.ItemsSource = machineQuery.ToList();
            ComboBoxMachine.DisplayMemberPath = "MachineName";
            ComboBoxMachine.SelectedValuePath = "MachineId";
        }

        public void BindComboBoxBrand()
        {
            var brandQuery = from brand in Entities.Brands
                             orderby brand.BrandName ascending
                             select brand;

            ComboBoxBrand.ItemsSource = brandQuery.ToList();
            ComboBoxBrand.DisplayMemberPath = "BrandName";
            ComboBoxBrand.SelectedValuePath = "BrandId";
        }

        private void ButtonTryLoad_OnClick(object sender, RoutedEventArgs e)
        {
            var partQuery = from part in Entities.Parts
                            orderby part.PartId descending
                            select part;
            var loadedPart = partQuery.FirstOrDefault(x => x.PartNo == TextBoxPartNo.Text);
            if (loadedPart != null)
            {
                LoadPartPartial(loadedPart);
                ClearStatusbar();
                ShowMessageInStatusbar("Part loaded");
            }
            else
            {
                ClearStatusbar();
                ShowMessageInStatusbar("Part no does not exist");
            }

            TextBoxLocation.Focus();
        }

        private void LoadPartPartial(Part part)
        {
            //TextBoxPartNo.Text = part.PartNo;
            //TextBoxLocation.Text = part.Location;
            //TextBoxTagName.Text = part.TagName;
            //TextBoxResolutionPartNo.Text = part.ResolutionPartNo;
            TextBoxPartName.Text = part.PartName;
            TextBoxPartNoOrignal.Text = part.PartNoOrignal;
            //ComboBoxMachine.SelectedItem = part.Machine;
            ComboBoxBrand.SelectedItem = part.Brand;
        }

        private void LoadFull(Part part)
        {
            //TextBoxPartNo.Text = part.PartNo;
            TextBoxLocation.Text = part.Location;
            TextBoxTagName.Text = part.TagName;
            TextBoxResolutionPartNo.Text = part.ResolutionPartNo;
            TextBoxPartName.Text = part.PartName;
            TextBoxPartNoOrignal.Text = part.PartNoOrignal;
            ComboBoxMachine.SelectedItem = part.Machine;
            ComboBoxBrand.SelectedItem = part.Brand;
        }

        private void LoadPartForEdit(Part part)
        {
            TextBoxPartNo.Text = part.PartNo;
            TextBoxLocation.Text = part.Location;
            TextBoxTagName.Text = part.TagName;
            TextBoxResolutionPartNo.Text = part.ResolutionPartNo;
            TextBoxPartName.Text = part.PartName;
            TextBoxPartNoOrignal.Text = part.PartNoOrignal;
            ComboBoxMachine.SelectedItem = part.Machine;
            ComboBoxBrand.SelectedItem = part.Brand;
        }

        private void ButtonClear_OnClick(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            TextBoxPartNo.Text = "";
            TextBoxLocation.Text = "";
            TextBoxTagName.Text = "";
            TextBoxResolutionPartNo.Text = "";
            TextBoxPartName.Text = "";
            TextBoxPartNoOrignal.Text = "";
            ComboBoxMachine.SelectedIndex = -1;
            ComboBoxBrand.SelectedIndex = -1;
        }

        private void ShowMessageInStatusbar(string msg)
        {
            StatusBar1.Items.Add(msg);
        }

        private void ClearStatusbar()
        {
            StatusBar1.Items.Clear();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            PartWithNotify newPart = new PartWithNotify();
            newPart.PartNo = TextBoxPartNo.Text;
            newPart.Location = TextBoxLocation.Text;
            newPart.TagName = TextBoxTagName.Text;
            newPart.ResolutionPartNo = TextBoxResolutionPartNo.Text;
            newPart.PartName = TextBoxPartName.Text;
            newPart.PartNoOrignal = TextBoxPartNoOrignal.Text;

            if (ComboBoxMachine.SelectedIndex != -1)
            {
                newPart.MachineId = (ComboBoxMachine.SelectedItem as Machine).MachineId;
                newPart.MachineName=(ComboBoxMachine.SelectedItem as Machine).MachineName;
            }

            if (ComboBoxBrand.SelectedIndex != -1)
            {
                newPart.BrandId = (ComboBoxBrand.SelectedItem as Brand).BrandId;
                newPart.BrandName = (ComboBoxBrand.SelectedItem as Brand).BrandName;
            }

            var result= PartsCollection.AddNew(0, newPart);

            if (result)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("New part added");
                Clear();
                TextBoxPartNo.Focus();
            }
            else
            {
                ClearStatusbar();
                ShowMessageInStatusbar("Failed");
            }
        }

        private void ButtonTryFullLoad_OnClick(object sender, RoutedEventArgs e)
        {
            var partQuery = from part in Entities.Parts
                            orderby part.PartId descending
                            select part;
            var loadedPart = partQuery.FirstOrDefault(x => x.PartNo == TextBoxPartNo.Text);

            if (loadedPart != null)
            {
                LoadFull(loadedPart);
                ClearStatusbar();
                ShowMessageInStatusbar("Part loaded");
            }
            else
            {
                ClearStatusbar();
                ShowMessageInStatusbar("Part no does not exist");
            }

            TextBoxLocation.Focus();
        }

        private void ButtonLoadLastEntry_OnClick(object sender, RoutedEventArgs e)
        {
            var partQuery = from part in Entities.Parts
                            orderby part.PartId descending
                            select part;
            var loadedPart = partQuery.FirstOrDefault();

            if (loadedPart != null)
            {
                LoadPartForEdit(loadedPart);
                ClearStatusbar();
                ShowMessageInStatusbar("Part loaded");
            }
        }
    }
}
