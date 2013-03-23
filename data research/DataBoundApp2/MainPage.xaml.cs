using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;


namespace DataBoundApp2
{
    public partial class MainPage : PhoneApplicationPage
    {

        //ooh. store current selection so we know which item to mark as unselected by hand
        private int currentlySelectedIndex = -1;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Handle selection changed on ListBox
        /*
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (MainListBox.SelectedIndex == -1)
                return;

            /*
            
            // find the last selected item and mark it as no longer selected
            App.ViewModel.Items.ElementAt<ItemViewModel>(currentlySelectedIndex).IsSelected = false;
            currentlySelectedIndex = MainListBox.SelectedIndex;
            App.ViewModel.Items.ElementAt<ItemViewModel>(currentlySelectedIndex).IsSelected = true; 

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
            */
    //    }
    
        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }
    }
}