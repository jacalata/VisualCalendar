using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage; //persist setting to phone
using Microsoft.Phone.UserData; //calendar data

namespace VisualCalendar
{
    public partial class Settings : PhoneApplicationPage
    {

        private List<Account> accounts;
        Account userChosenAccount;
        public Settings()
        {
            InitializeComponent();
            accounts = new List<Account>(new Appointments().Accounts);
            this.DataContext = accounts;
            AccountsListBox.DataContext = accounts;
            userChosenAccount = accounts.First(); //set a default in case they don't care and just hit next
        }

        private void OnCalendarChosen(object sender, RoutedEventArgs e)
        {
            // we only come to this page if it doesn't exist, so no need to check the value firsts
            IsolatedStorageSettings.ApplicationSettings.Add(App.SelectedAccount, userChosenAccount.Name);
            App.CardSet.account = userChosenAccount;
            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            userChosenAccount = (Account)((RadioButton)sender).DataContext;
        }
    }
}