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
    /// Interaction logic for WindowInsertMachine.xaml
    /// </summary>
    public partial class WindowEditMachine : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();
        public event EventHandler DataBaseUpdated;

        protected virtual void OnDataBaseUpdated()
        {
            EventHandler handler = DataBaseUpdated;
            if (handler != null) handler(this, EventArgs.Empty);
        }

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
                if (Entities.Machines.Any(x => x.MachineName.ToLower() == TextBoxMachine.Text.ToLower()))
                {
                    ShowMessageInStatusbar("the machine is already saved");
                    return;
                }

                Machine currentMachine = Entities.Machines.FirstOrDefault(x => x.MachineId == MachineToEdit.MachineId);
                currentMachine.MachineName = TextBoxMachine.Text;

                if (Entities.SaveChanges() > 0)
                {
                    OnDataBaseUpdated();
                    ShowMessageInStatusbar("machine edited");
                    TextBoxMachine.SelectAll();
                    TextBoxMachine.Focus();
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
    }
}
