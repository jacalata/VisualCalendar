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
using DataBoundApp2.ViewModels;

using Microsoft.Phone.UserData; //calendar data


namespace DataBoundApp2
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();

                MainListBox.DataContext = App.CardSet.currentCards;
               /* previous.DataContext = App.CardSet.prevCard;
                titleTextBlock.Text = App.CardSet.prevCard.title;
                current.DataContext = App.CardSet.currentCard;
                next.DataContext = App.CardSet.nextCard;
                //image.Source = (System.Windows.Media.ImageSource)App.CardSet.currentCard.imageUri;
                * */
        }

        public void OnNavigatedTo()
        {
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
    }
}