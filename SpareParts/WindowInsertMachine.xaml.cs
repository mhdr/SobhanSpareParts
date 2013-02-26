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
    public partial class WindowInsertMachine : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();
        public event EventHandler DataBaseUpdated;

        protected virtual void OnDataBaseUpdated()
        {
            EventHandler handler = DataBaseUpdated;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public WindowInsertMachine()
        {
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            ClearStatusbar();

            if (TextBoxMachine.Text.Length > 0)
            {
                if (Entities.Machines.Any(x => x.MachineName.ToLower() == TextBoxMachine.Text.ToLower()))
                {
                    ShowMessageInStatusbar("the machine is already added");
                    return;
                }

                Machine newMachine = new Machine();
                newMachine.MachineName = TextBoxMachine.Text;
                Entities.Machines.Add(newMachine);

                if (Entities.SaveChanges() > 0)
                {
                    OnDataBaseUpdated();
                    TextBoxMachine.Text = "";
                    ShowMessageInStatusbar("new machine added");
                }
            }
        }

        private void WindowInsertMachine_OnLoaded(object sender, RoutedEventArgs e)
        {
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
