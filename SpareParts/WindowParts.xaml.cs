﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace SpareParts
{
    /// <summary>
    /// Interaction logic for WindowParts.xaml
    /// </summary>
    public partial class WindowParts : Window
    {
        private SparePartsEntities _entities=new SparePartsEntities();
        private ListCollectionView _view;

        public WindowParts()
        {
            StyleManager.ApplicationTheme = new Office_BlackTheme();
            InitializeComponent();
        }

        public SparePartsEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public ListCollectionView View
        {
            get { return _view; }
            set { _view = value; }
        }

        private void WindowParts_OnLoaded(object sender, RoutedEventArgs e)
        {
            BindGridViewParts();
        }

        private void BindGridViewParts()
        {
            GridViewParts.ItemsSource = Entities.Parts.ToList();
        }

        private void RibbonButtonBrands_OnClick(object sender, RoutedEventArgs e)
        {
            WindowBrands windowBrands=new WindowBrands();
            windowBrands.Show();
        }

        private void GridViewParts_OnGrouping(object sender, GridViewGroupingEventArgs e)
        {
            if (e.Action == GroupingEventAction.Place)
            {
                e.Cancel = true;
                var c = (ColumnGroupDescriptor)e.GroupDescriptor;
                CountFunction f = new CountFunction();
                f.Caption = "Count: ";
                GroupDescriptor descriptor = new GroupDescriptor();
                descriptor.Member = c.Column.UniqueName;
                descriptor.DisplayContent = c.DisplayContent;
                descriptor.AggregateFunctions.Add(f);
                this.GridViewParts.GroupDescriptors.Add(descriptor);
            }
        }

        private void RibbonButtonMachines_OnClick(object sender, RoutedEventArgs e)
        {
            WindowMachines windowMachines=new WindowMachines();
            windowMachines.Show();
        }

        private void RibbonButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            WindowInsertPart windowInsertPart=new WindowInsertPart();
            windowInsertPart.DataBaseUpdated += windowInsertPart_DataBaseUpdated;
            windowInsertPart.Show();
        }

        void windowInsertPart_DataBaseUpdated(object sender, EventArgs e)
        {
            BindGridViewParts();
        }

        private void RibbonButtonRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            BindGridViewParts();
        }

        private void RibbonButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (GridViewParts.SelectedItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            var part = GridViewParts.SelectedItem as Part;
            Entities.Parts.Remove(part);

            if (Entities.SaveChanges() > 0)
            {
                BindGridViewParts();
                ClearStatusbar();
                ShowMessageInStatusbar("Part removed");
            }
            else
            {
                ClearStatusbar();
                ShowMessageInStatusbar("Failed");       
            }
        }

        private void ShowMessageInStatusbar(string msg)
        {
            StatusBar1.Items.Add(msg);
        }

        private void ClearStatusbar()
        {
            StatusBar1.Items.Clear();
        }

        private void RibbonButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (GridViewParts.SelectedItem == null)
            {
                ClearStatusbar();
                ShowMessageInStatusbar("First select an item");
                return;
            }

            WindowEditPart windowEditPart=new WindowEditPart();
            windowEditPart.PartToEdit = GridViewParts.SelectedItem as Part;
            windowEditPart.DataBaseUpdated += windowEditPart_DataBaseUpdated;
            windowEditPart.Show();
        }

        void windowEditPart_DataBaseUpdated(object sender, EventArgs e)
        {
            Entities=new SparePartsEntities();
            BindGridViewParts();
        }
    }
}
