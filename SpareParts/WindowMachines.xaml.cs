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
    /// Interaction logic for WindowMachines.xaml
    /// </summary>
    public partial class WindowMachines : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();


        public WindowMachines()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private void WindowMachines_OnLoaded(object sender, RoutedEventArgs e)
        {
            BindGridViewMachines();
        }

        private void BindGridViewMachines()
        {
            GridViewMachines.ItemsSource = Entities.Machines.ToList();
        }

        private void RibbonButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (GridViewMachines.SelectedItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            var selectedMachine = GridViewMachines.SelectedItem as Machine;
            if (Entities.Parts.Any(x => x.MachineId == selectedMachine.MachineId))
            {
                ClearStatusbar();
                ShowMessageInStatusbar("This machine is in use");
                return;
            }

            Entities.Machines.Remove((Machine)GridViewMachines.SelectedItem);
            if (Entities.SaveChanges() > 0)
            {
                BindGridViewMachines();
                ClearStatusbar();
                ShowMessageInStatusbar("Machine removed");
                NotifyOpenWindows();
            }
            else
            {
                ClearStatusbar();
                ShowMessageInStatusbar("Failed");
            }
        }

        private void RibbonButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            WindowInsertMachine windowInsertMachine = new WindowInsertMachine();
            windowInsertMachine.DataBaseUpdated += windowInsertMachine_DataBaseUpdated;
            windowInsertMachine.Show();
        }

        void windowInsertMachine_DataBaseUpdated(object sender, EventArgs e)
        {
            BindGridViewMachines();
            NotifyOpenWindows();
        }

        private void RibbonButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (GridViewMachines.SelectedItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            WindowEditMachine windowEditMachine = new WindowEditMachine();
            windowEditMachine.MachineToEdit = (Machine) GridViewMachines.SelectedItem;
            windowEditMachine.DataBaseUpdated += windowEditMachine_DataBaseUpdated;
            windowEditMachine.Show();
        }

        void windowEditMachine_DataBaseUpdated(object sender, EventArgs e)
        {
            Entities=new SparePartsEntities();
            BindGridViewMachines();
            NotifyOpenWindows();
        }

        private void ShowMessageInStatusbar(string msg)
        {
            StatusBar1.Items.Add(msg);
        }

        private void ClearStatusbar()
        {
            StatusBar1.Items.Clear();
        }

        private static void NotifyOpenWindows()
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(WindowInsertPart))
                {
                    (window as WindowInsertPart).BindComboBoxMachine();
                }

                if (window.GetType() == typeof(WindowEditPart))
                {
                    (window as WindowEditPart).BindComboBoxMachine();
                }
            }
        }
    }
}
