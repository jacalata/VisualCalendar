using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
//using DataBoundApp2.Resources;
using VisualCalendar.ViewModels;

using Microsoft.Phone.UserData; //calendar data


namespace VisualCalendar
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            MainListBox.DataContext = App.CardSet.currentCards;
        }

        // Handle selection changed on LongListSelector
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (MainListBox.SelectedItem == null)
                return;

            // Reset selected item to null (no selection)
            MainListBox.SelectedItem = null;

        }
        
        // we only have the Settings page, so if we get here, remove it from the back stack. 
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.NavigationService.RemoveBackEntry();
        }
    }
}