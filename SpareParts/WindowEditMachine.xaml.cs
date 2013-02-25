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
    /// Interaction logic for WindowInsertMachine.xaml
    /// </summary>
    public partial class WindowEditMachine : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();
        private Lib.ObservableMachines _machinesCollection;
        private ListCollectionView _view;
        private Machine _machineToEdit;

        public WindowEditMachine()
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

        public Machine MachineToEdit
        {
            get { return _machineToEdit; }
            set { _machineToEdit = value; }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            ClearStatusbar();

            if (TextBoxMachine.Text.Length > 0)
            {
                if (MachinesCollection.Any(x => x.MachineName.ToLower() == TextBoxMachine.Text.ToLower()))
                {
                    ShowMessageInStatusbar("the machine is already saved");
                    return;
                }


                View.EditItem(MachineToEdit);
                MachineToEdit.MachineName = TextBoxMachine.Text;
                View.CommitEdit();
                MachinesCollection.Notify();

                if (Entities.SaveChanges() > 0)
                {
                    ShowMessageInStatusbar("machine edited");
                    TextBoxMachine.SelectAll();
                    TextBoxMachine.Focus();
                }
            }
        }

        private void WindowInsertMachine_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Entities = MachinesCollection.Entities;
            //MachinesCollection = new ObservableMachines(Entities.Machines, Entities);
            var machineSource = (CollectionViewSource)this.FindResource("MachinesSource");
            machineSource.Source = MachinesCollection;
            View = (ListCollectionView)machineSource.View;

            TextBoxMachine.Text = MachineToEdit.MachineName;
            TextBoxMachine.SelectAll();
            TextBoxMachine.Focus();
        }

        private void ShowMessageInStatusbar(string msg)
        {
            StatusBar1.Items.Add(msg);
        }

        private void ClearStatusbar()
        {
            StatusBar1.Items.Clear();
        }
    }
}
