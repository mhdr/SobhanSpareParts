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

        private void GridViewMachines_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (GridViewMachines.SelectedItem == null)
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
            Entities.Machines.Remove((Machine) GridViewMachines.SelectedItem);
            Entities.SaveChanges();
            BindGridViewMachines();
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
        }

        private void RibbonButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            WindowEditMachine windowEditMachine = new WindowEditMachine();
            windowEditMachine.MachineToEdit = (Machine) GridViewMachines.SelectedItem;
            windowEditMachine.DataBaseUpdated += windowEditMachine_DataBaseUpdated;
            windowEditMachine.Show();
        }

        void windowEditMachine_DataBaseUpdated(object sender, EventArgs e)
        {
            Entities=new SparePartsEntities();
            BindGridViewMachines();
        }
    }
}
