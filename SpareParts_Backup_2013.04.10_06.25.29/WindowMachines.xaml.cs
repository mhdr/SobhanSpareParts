using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for WindowMachines.xaml
    /// </summary>
    public partial class WindowMachines : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();
        private MachinesObservableCollection _machinesCollection;
        private ListCollectionView _view;

        public WindowMachines()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public MachinesObservableCollection MachinesCollection
        {
            get { return _machinesCollection; }
            set { _machinesCollection = value; }
        }

        public ListCollectionView View
        {
            get { return _view; }
            set { _view = value; }
        }

        private void WindowMachines_OnLoaded(object sender, RoutedEventArgs e)
        {
            BindGridViewMachines();
        }

        private void BindGridViewMachines()
        {

            var machinesQuery = from machine in Entities.Machines
                                orderby machine.MachineName ascending
                                select machine;
            MachinesCollection = new MachinesObservableCollection(machinesQuery.ToList(), Entities);
            var machinesSource = (CollectionViewSource)FindResource("MachinesSource");
            machinesSource.Source = MachinesCollection;
            View = (ListCollectionView)machinesSource.View;
            View.SortDescriptions.Add(new SortDescription("MachineName", ListSortDirection.Ascending));
        }


        private void RibbonButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (GridViewMachines.SelectedItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            MachineWithNotify selectedMachine = (MachineWithNotify) View.CurrentItem;
            if (Entities.Parts.Any(x => x.MachineId == selectedMachine.MachineId))
            {
                ClearStatusbar();
                ShowMessageInStatusbar("This machine is in use");
                return;
            }

            var result = MachinesCollection.Delete(View.CurrentPosition);
            if (result)
            {
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
            windowInsertMachine.Entities = this.Entities;
            windowInsertMachine.MachinesCollection = this.MachinesCollection;
            windowInsertMachine.View = this.View;
            windowInsertMachine.Show();
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
            windowEditMachine.Entities = this.Entities;
            windowEditMachine.MachinesCollection = this.MachinesCollection;
            windowEditMachine.View = this.View;
            windowEditMachine.MachineToEdit = (MachineWithNotify) View.CurrentItem;
            windowEditMachine.Index = View.CurrentPosition;
            windowEditMachine.Show();
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
