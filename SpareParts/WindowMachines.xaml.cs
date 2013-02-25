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
    /// Interaction logic for WindowMachines.xaml
    /// </summary>
    public partial class WindowMachines : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();
        private Lib.ObservableMachines _machinesCollection;
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

        public ObservableMachines MachinesCollection
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
            MachinesCollection = new ObservableMachines(Entities.Machines, Entities);
            var machinesSource = (CollectionViewSource)this.FindResource("MachinesSource");
            machinesSource.Source = MachinesCollection;
            View = (ListCollectionView)machinesSource.View;
        }

        private void GridViewMachines_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (View.CurrentItem == null)
            {
                RibbonButtonDelete.IsEnabled = false;
                RibbonButtonEdit.IsEnabled = false;
            }
            else
            {
                RibbonButtonDelete.IsEnabled = true;
                RibbonButtonEdit.IsEnabled = true;
            }
        }

        private void RibbonButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            View.RemoveAt(View.CurrentPosition);
            Entities.SaveChanges();
        }

        private void RibbonButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            WindowInsertMachine windowInsertMachine = new WindowInsertMachine();
            windowInsertMachine.MachinesCollection = MachinesCollection;
            windowInsertMachine.Show();
        }

        private void RibbonButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            WindowEditMachine windowEditMachine = new WindowEditMachine();
            windowEditMachine.MachinesCollection = MachinesCollection;
            windowEditMachine.MachineToEdit = (Machine)View.CurrentItem;
            windowEditMachine.Show();
        }
    }
}
