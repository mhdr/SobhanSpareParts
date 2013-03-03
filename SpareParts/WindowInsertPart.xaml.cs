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
    /// Interaction logic for WindowInsertPart.xaml
    /// </summary>
    public partial class WindowInsertPart : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();
        public event EventHandler DataBaseUpdated;

        protected virtual void OnDataBaseUpdated()
        {
            EventHandler handler = DataBaseUpdated;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public WindowInsertPart()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private void WindowInsertPart_OnLoaded(object sender, RoutedEventArgs e)
        {
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
            //LoadedPart = PartsCollection.FirstOrDefault(x => x.PartNo == TextBoxPartNo.Text);
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
            Part newPart = new Part();
            newPart.PartNo = TextBoxPartNo.Text;
            newPart.Location = TextBoxLocation.Text;
            newPart.TagName = TextBoxTagName.Text;
            newPart.ResolutionPartNo = TextBoxResolutionPartNo.Text;
            newPart.PartName = TextBoxPartName.Text;
            newPart.PartNoOrignal = TextBoxPartNoOrignal.Text;

            if (ComboBoxMachine.SelectedIndex != -1)
            {
                newPart.MachineId = (ComboBoxMachine.SelectedItem as Machine).MachineId;
            }

            if (ComboBoxBrand.SelectedIndex != -1)
            {
                newPart.BrandId = (ComboBoxBrand.SelectedItem as Brand).BrandId;
            }

            Entities.Parts.Add(newPart);

            if (Entities.SaveChanges() > 0)
            {
                OnDataBaseUpdated();
                ClearStatusbar();
                ShowMessageInStatusbar("New part added");

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
