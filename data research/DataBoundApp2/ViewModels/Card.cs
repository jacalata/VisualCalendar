using System;
using System.Collections.Generic; //IEnumerable
using System.Collections.ObjectModel;
using System.Linq; //Ienumerable.Count
using Microsoft.Phone.UserData; //calendar data
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace DataBoundApp2.ViewModels
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
        Appointments appointments = new Appointments();
        public ObservableCollection<Card> currentCards;
        public Card prevCard;
        public Card currentCard;
        public Card nextCard;

        public Cards()
        {
            cards = new List<Card>();
            currentCards = new ObservableCollection<Card>();
        }

        public void Add(Card card)
        {
            cards.Add(card);
        }

        public void LoadCards()
        {
            // fake data to sort out data binding
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

            // Set the data context of the LongListSelector control to the sample data
            appointments.SearchCompleted += new EventHandler<AppointmentsSearchEventArgs>(appointments_SearchCompleted);
            // TODO choose an account to look at the calendar for. 
            appointments.SearchAsync(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2), null); // find all events today
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
        private static string unknownEventImage = "\\images\\BREAK.jpg";

        static CardMapping()
        {
            CreateNewEventType("unscheduled", "\\images\\BREAK.jpg");
            CreateNewEventType("bus", "\\images\\BUS.jpg");
            CreateNewEventType("snack", "\\images\\SNACK.jpg");
            CreateNewEventType("toys", "\\images\\TOYS.jpg");
            CreateNewEventType("breakfast", "\\images\\breakfast.jpg");
            CreateNewEventType("dinner", "\\images\\dinner.jpg");
            CreateNewEventType("lunch", "\\images\\lunch.jpg");
            CreateNewEventType("get dressed", "\\images\\getdressed.jpg");
            CreateNewEventType("bathroom", "\\images\\bathrom.jpg");
            CreateNewEventType("hands", "\\images\\hands.jpg");

        }

        private static void CreateNewEventType(string title, string imageUri)
        {
            knownEvents.Add(title.ToLower());
            imageMappings.Add(title, imageUri);
        }

        static Dictionary<string, string> imageMappings = new Dictionary<string, string>();
        public static string GetImage(string title)
        {
            string[] titleWords = title.Split(' ');
            foreach (string word in titleWords)
            {
                if (knownEvents.Contains(word.ToLower()))
                {
                    return imageMappings[word.ToLower()];
                }
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
