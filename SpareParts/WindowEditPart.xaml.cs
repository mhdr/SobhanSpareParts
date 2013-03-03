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
    /// Interaction logic for WindowEditPart.xaml
    /// </summary>
    public partial class WindowEditPart : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();
        public event EventHandler DataBaseUpdated;
        private Part _partToEdit;

        protected virtual void OnDataBaseUpdated()
        {
            EventHandler handler = DataBaseUpdated;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public WindowEditPart()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public Part PartToEdit
        {
            get { return _partToEdit; }
            set { _partToEdit = value; }
        }

        private void WindowEditPart_OnLoaded(object sender, RoutedEventArgs e)
        {
            BindComboBoxBrand();
            BindComboBoxMachine();

            var selectedPart = Entities.Parts.FirstOrDefault(x => x.PartId == PartToEdit.PartId);
            LoadPartForEdit(selectedPart);
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
            var loadedPart = Entities.Parts.FirstOrDefault(x => x.PartNo == TextBoxPartNo.Text);
            if (loadedPart != null)
            {
                LoadPart(loadedPart);
                ClearStatusbar();
                ShowMessageInStatusbar("Part loaded");
            }
            else
            {
                ClearStatusbar();
                ShowMessageInStatusbar("Part no does not exist");
            }
        }

        private void LoadPart(Part part)
        {
            //TextBoxPartNo.Text = part.PartNo;
            //TextBoxLocation.Text = part.Location;
            //TextBoxTagName.Text = part.TagName;
            TextBoxResolutionPartNo.Text = part.ResolutionPartNo;
            TextBoxPartName.Text = part.PartName;
            TextBoxPartNoOrignal.Text = part.PartNoOrignal;
            //ComboBoxMachine.SelectedItem = part.Machine;
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
            Part editPart = Entities.Parts.FirstOrDefault(x => x.PartId == PartToEdit.PartId);
            editPart.PartNo = TextBoxPartNo.Text;
            editPart.Location = TextBoxLocation.Text;
            editPart.TagName = TextBoxTagName.Text;
            editPart.ResolutionPartNo = TextBoxResolutionPartNo.Text;
            editPart.PartName = TextBoxPartName.Text;
            editPart.PartNoOrignal = TextBoxPartNoOrignal.Text;

            if (ComboBoxMachine.SelectedIndex != -1)
            {
                editPart.MachineId = (ComboBoxMachine.SelectedItem as Machine).MachineId;
            }

            if (ComboBoxBrand.SelectedIndex != -1)
            {
                editPart.BrandId = (ComboBoxBrand.SelectedItem as Brand).BrandId;
            }

            if (Entities.SaveChanges() > 0)
            {
                OnDataBaseUpdated();
                ClearStatusbar();
                ShowMessageInStatusbar("Part saved");

                if (ToggleButtonRetain.IsChecked == false)
                {
                    Clear();
                }
            }
            else
            {
                ClearStatusbar();
                ShowMessageInStatusbar("Failed");
            }
        }
    }
}
