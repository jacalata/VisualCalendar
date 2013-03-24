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
        Appointments appointments = new Appointments();
        public Cards cards = new Cards();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the LongListSelector control to the sample data
            appointments.SearchCompleted += new EventHandler<AppointmentsSearchEventArgs>(appointments_SearchCompleted);
            // TODO choose an account to look at the calendar for. 
            appointments.SearchAsync(DateTime.Today, DateTime.Today.AddDays(1), null); // find all events today
        }

        // on load, we searched the calendar for all of today's events. Now we got them as an enumerator
        void appointments_SearchCompleted(object sender, AppointmentsSearchEventArgs e)
        {

            if (e.Results == null || e.Results.Count() == 0)
            {
                // error, OR no events for the day
                // display a single question mark tile
                cards.Add(new Card("unscheduled"));
                // note for ui: this means we might have only one card for the day, need to handle null prev/next cards
            }
            CreateCards(e.Results);

            Calendar.DataContext = cards.allCardsToday();
           //Calendar.DataContext = cards;

        }

        public void CreateCards(IEnumerable<Appointment> appointments)
        {            
            // for each event returned, if the title contains VisualCalendar, then
            foreach (Appointment item in appointments)
            {

                Card newCard = new Card(item.Subject);
                if (item.EndTime < DateTime.Now)
                {
                    cards.prevCard = newCard; //this can't be current but it might be the most recent
                }
                else if (item.StartTime < DateTime.Now && item.EndTime > DateTime.Now && cards.currentCard == null)
                {
                    cards.prevCard = newCard; // if there are multiple now, we took the first one
                }
                else if (item.StartTime > DateTime.Now && cards.nextCard == null)
                {
                    cards.nextCard = newCard; // this is the first 'not yet' event we've seen
                    if (cards.currentCard == null) //nothing was scheduled now
                    {
                        cards.currentCard = new Card("freetime"); //show 'free time' if there is no event right now'
                        cards.Add(cards.currentCard);
                    }
                }
            }
        }

        /* bugbug compile error commented out
        // Handle selection changed on LongListSelector
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            // Reset selected item to null (no selection)
            MainLongListSelector.SelectedItem = null;
        }
        */
    }
}