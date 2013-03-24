using System;
using System.Collections.Generic; //IEnumerable
using System.Collections.ObjectModel;
using System.Linq; //Ienumerable.Count
using Microsoft.Phone.UserData; //calendar data
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage; //persist setting to phone
using Microsoft.Phone.Shell;

namespace VisualCalendar.ViewModels
{

    // individual card item for display
    // properties being databound must be public 
    public class Card
    {
        // dirty way to highlight current Event
        public int height { get; set; }
        public int width { get; set; }

        // title text
        public string title { get; set; }

        // image source
        public string imageUri { get; set; }
   
        public Card(string title)
        {
            this.title = title;
            this.imageUri = CardMapping.GetImage(title);
            this.height = 240;
            this.width = 240;
        }
    }


    // wrapper for a list that has current, next and previous explicitly accessible
    public class Cards
    {
        private List<Card> cards;
        private Appointments appointments = new Appointments();

        public Account account;
        public bool AccountWasChosen;
        public ObservableCollection<Card> currentCards;
        public Card prevCard;
        public Card currentCard;
        public Card nextCard;

        public Cards()
        {
            cards = new List<Card>();
            currentCards = new ObservableCollection<Card>();
            if (IsolatedStorageSettings.ApplicationSettings.Contains(App.SelectedAccount))
            {
                string accountname = "";
                IsolatedStorageSettings.ApplicationSettings.TryGetValue(App.SelectedAccount, out accountname);
                List<Account> accounts = new List<Account>(new Appointments().Accounts);
                foreach (Account acct in accounts)
                {
                    if (acct.Name.Equals(accountname) )
                        account = acct;
                }
            }
            AccountWasChosen = false;
        }

        public void Add(Card card)
        {
            cards.Add(card);
        }

        public void LoadCards()
        {
            // fake data 
           /* foreach (string title in CardMapping.GetEventTypes())
            {
                Card newest = new Card(title);
                cards.Add(newest);
                if (currentCard == null)
                    currentCard = newest;
                else if (prevCard == null)
                    prevCard = newest;
                else if (nextCard == null)
                    nextCard = newest;
            }*/

            appointments.SearchCompleted += new EventHandler<AppointmentsSearchEventArgs>(appointments_SearchCompleted);
            // TODO choose an account to look at the calendar for. 
            /*
             *  foreach (Account acct in (new Appointments()).Accounts)
            {
                appts.SearchAsync(start, end, 20, acct, "Appointments Test " + acct.Kind.ToString());
            }
             */
            appointments.SearchAsync(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2), account, null); // find all events today
        }

        // on load, we searched the calendar for all of today's events. Now we got them as an enumerator
        void appointments_SearchCompleted(object sender, AppointmentsSearchEventArgs e)
        {
            if ((e.Results == null) || (e.Results.Count() == 0))
            {
                // error, OR no events for the day
                // display a single question mark tile
                this.currentCard = new Card("unscheduled");
            }
            else
            {
                this.LoadCards(e.Results);

            }
            //fill in prev/current/next as unscheduled if there is nothing available for any of them
            if (prevCard == null) prevCard = new Card("unscheduled");
            currentCards.Add(prevCard);
            if (currentCard == null) currentCard = new Card("unscheduled");
            currentCard.height = 280;
            currentCard.width = 280;
            currentCards.Add(currentCard);
            if (nextCard == null) nextCard = new Card("unscheduled");
            currentCards.Add(nextCard);
            IsDataLoaded = true;

        }

        public void LoadCards(IEnumerable<Appointment> appointments)
        {            
            // for each event returned, if the title contains VisualCalendar, then
            foreach (Appointment item in appointments)
            {
                //BUG: not sure why we're getting these null items?
                if (item.Subject == null)
                    continue;
                Card newCard = new Card(item.Subject);
                if (item.EndTime < DateTime.Now)
                {
                    this.prevCard = newCard; //this can't be current but it might be the most recent
                }
                else if (item.StartTime < DateTime.Now && item.EndTime > DateTime.Now && this.currentCard == null)
                {
                    this.currentCard = newCard; // if there are multiple now, we took the first one
                }
                else if (item.StartTime > DateTime.Now && this.nextCard == null)
                {
                    this.nextCard = newCard; // this is the first 'not yet' event we've seen
                    if (this.currentCard == null) //nothing was scheduled now
                    {
                        this.currentCard = new Card("freetime"); //show 'free time' if there is no event right now'
                        this.Add(this.currentCard);
                    }
                }
            }
        }


        public bool IsDataLoaded
        {
            get;
            private set;
        }

    }

    // canonical list of known event types and images for eachs
    public static class CardMapping
    {
        private static string unknownEventImage = "\\images\\question.png";

        static CardMapping()
        {
            CreateNewEventType("bathroom", "\\images\\bathroombreak.png");
            CreateNewEventType("bathroombreak", "\\images\\bathroombreak.png");
            CreateNewEventType("bathroom break", "\\images\\bathroombreak.png");
            CreateNewEventType("break", "\\images\\break.png");
            CreateNewEventType("bus", "\\images\\bus.png");
            CreateNewEventType("calendar", "\\images\\calendar.png");
            CreateNewEventType("car", "\\images\\car.png");
            CreateNewEventType("circle", "\\images\\circletime.png");
            CreateNewEventType("circle time", "\\images\\circletime.png");
            CreateNewEventType("circletime", "\\images\\circletime.png");
            CreateNewEventType("computer", "\\images\\computer.png");
            CreateNewEventType("computer time", "\\images\\computertime.png");
            CreateNewEventType("dinner", "\\images\\lunchdinner.png");
            CreateNewEventType("getdressed", "\\images\\getdressed.png");
            CreateNewEventType("get dressed", "\\images\\getdressed.png");
            CreateNewEventType("learningtime", "\\images\\learningtime.png");
            CreateNewEventType("learning time", "\\images\\learningtime.png");
            CreateNewEventType("learning", "\\images\\learningtime.png");
            CreateNewEventType("library", "\\images\\library.png");
            CreateNewEventType("lunchdinner", "\\images\\lunchdinner.png");
            CreateNewEventType("lunch", "\\images\\lunchdinner.png");
            CreateNewEventType("lunchsnack", "\\images\\lunchsnack.png");
            CreateNewEventType("makebed", "\\images\\makebed.png");
            CreateNewEventType("make bed", "\\images\\makebed.png");
            CreateNewEventType("music", "\\images\\music.png");
            CreateNewEventType("question", "\\images\\question.png");
            CreateNewEventType("readingastory", "\\images\\readingastory.png");
            CreateNewEventType("reading a story", "\\images\\readingastory.png");
            CreateNewEventType("read a story", "\\images\\readingastory.png");
            CreateNewEventType("read story", "\\images\\readingastory.png");
            CreateNewEventType("story", "\\images\\readingastory.png");
            CreateNewEventType("readingtime", "\\images\\readingtime.png");
            CreateNewEventType("reading time", "\\images\\readingtime.png");
            CreateNewEventType("reading", "\\images\\readingtime.png");
            CreateNewEventType("read", "\\images\\readingtime.png");
            CreateNewEventType("recess", "\\images\\recess.png");
            CreateNewEventType("snack", "\\images\\snack.png");
            CreateNewEventType("teachtowntime", "\\images\\teachtown.png");
            CreateNewEventType("teachtown", "\\images\\teachtown.png");
            CreateNewEventType("teach town", "\\images\\music.png");
            CreateNewEventType("unscheduled", "\\images\\break.png");
            CreateNewEventType("wash hands", "\\images\\hands.jpg");
        }

        private static void CreateNewEventType(string title, string imageUri)
        {
            // a card might require multiple keywords to be present ('make bed', 'wash hands') 
            if (knownEvents.Contains(title))
                return; // can't add it twice, let's go with the first image they used. 
            knownEvents.Add(title.ToLower());
            imageMappings.Add(title, imageUri);
        }

        static Dictionary<string, string> imageMappings = new Dictionary<string, string>();
        public static string GetImage(string title)
        {
            foreach (string keyword in knownEvents)
            {
                if (title.ToLower().Contains(keyword.ToLower()))
                    return imageMappings[keyword];
            }
            return unknownEventImage;
        }

        static List<string> knownEvents = new List<string>();
        public static List<string> GetEventTypes()
        {
            return knownEvents;
        }

    }

}
