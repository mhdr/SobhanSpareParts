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
        private Part _loadedPart;

        public WindowInsertPart()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public Part LoadedPart
        {
            get { return _loadedPart; }
            set { _loadedPart = value; }
        }

        private void WindowInsertPart_OnLoaded(object sender, RoutedEventArgs e)
        {
            ComboBoxBrand.ItemsSource = Entities.Brands.ToList();
            ComboBoxBrand.DisplayMemberPath = "BrandName";
            ComboBoxBrand.SelectedValuePath = "BrandId";

            ComboBoxMachine.ItemsSource = Entities.Machines.ToList();
            ComboBoxMachine.DisplayMemberPath = "MachineName";
            ComboBoxMachine.SelectedValuePath = "MachineId";
        }

        private void ButtonTryLoad_OnClick(object sender, RoutedEventArgs e)
        {
            BusyIndicator1.IsBusy = true;
            //LoadedPart = PartsCollection.FirstOrDefault(x => x.PartNo == TextBoxPartNo.Text);
            LoadedPart = Entities.Parts.FirstOrDefault(x => x.PartNo == TextBoxPartNo.Text);
            BusyIndicator1.IsBusy = false;
        }
    }
}
