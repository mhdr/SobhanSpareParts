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
        private MachinesObservableCollection _machinesCollection;
        private ListCollectionView _view;

        private MachineWithNotify _machineToEdit;

        public WindowEditMachine()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public MachineWithNotify MachineToEdit
        {
            get { return _machineToEdit; }
            set { _machineToEdit = value; }
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

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            ClearStatusbar();

            if (TextBoxMachine.Text.Length > 0)
            {
                if (Entities.Machines.Any(x => x.MachineName.ToLower() == TextBoxMachine.Text.ToLower()))
                {
                    ShowMessageInStatusbar("the machine is already saved");
                    return;
                }

                MachineToEdit.MachineName = TextBoxMachine.Text;
                var result = MachinesCollection.Update(View.CurrentPosition, MachineToEdit);

                if (result)
                {
                    NotifyOpenWindows();
                    this.Close();
                }
            }
        }

        private void WindowInsertMachine_OnLoaded(object sender, RoutedEventArgs e)
        {
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
