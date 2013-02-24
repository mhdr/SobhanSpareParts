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
        private SparePartsEntities _entities=new SparePartsEntities();

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
            DataFormMachines.ItemsSource = Entities.Machines;
        }
    }
}
