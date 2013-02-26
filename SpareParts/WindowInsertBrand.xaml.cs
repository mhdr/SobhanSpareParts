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
    /// Interaction logic for WindowInsertBrand.xaml
    /// </summary>
    public partial class WindowInsertBrand : Window
    {
        private SparePartsEntities _entities = new SparePartsEntities();
        public event EventHandler DataBaseUpdated;

        protected virtual void OnDataBaseUpdated()
        {
            EventHandler handler = DataBaseUpdated;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public WindowInsertBrand()
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

            if (TextBoxBrand.Text.Length > 0)
            {
                if (Entities.Brands.Any(x => x.BrandName.ToLower() == TextBoxBrand.Text.ToLower()))
                {
                    ShowMessageInStatusbar("the brand is already added");
                    return;
                }

                Brand newBrand=new Brand();
                newBrand.BrandName = TextBoxBrand.Text;
                Entities.Brands.Add(newBrand);

                if (Entities.SaveChanges() > 0)
                {
                    OnDataBaseUpdated();
                    TextBoxBrand.Text = "";
                    ShowMessageInStatusbar("new brand added");
                }
            }
        }

        private void WindowInsertBrand_OnLoaded(object sender, RoutedEventArgs e)
        {
            TextBoxBrand.Focus();
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
