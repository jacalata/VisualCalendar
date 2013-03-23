using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iCalTestApp.ViewModels
{

    // individual card item for display
    public class Card
    {   
        // title text
        string title;

        // image source
        string imageUri;
   
        public Card(string title)
        {
            this.title = title;
            this.imageUri = CardMapping.GetImage(title);
        }
    }


    // wrapper for a list that has current, next and previous explicitly accessible
    public class Cards
    {
        private List<Card> cards;

        public Card prevCard;
        public Card currentCard;
        public Card nextCard;

        
        public List<Card> allCardsToday()
        {
            return cards;
        }

        public Cards()
        {
            cards = new List<Card>();
        }

        public void Add(Card card)
        {
            cards.Add(card);
        }

    }

    // canonical list of known event types and images for eachs
    public static class CardMapping
    {
        private static string unknownEventImage = "\\Assets\\superkid.jpg";

        static CardMapping()
        {
            CreateNewEventType("unscheduled", "\\Assets\\superkid.jpg");
            CreateNewEventType("bus", "\\Assets\bus.jpg");
            CreateNewEventType("snack", "\\Assets\\snack.jpg");
            CreateNewEventType("toys", "\\Assets\toys.jpg");

        }

        private static void CreateNewEventType(string title, string imageUri)
        {
            knownEvents.Add(title.ToLower());
            imageMappings.Add(title, imageUri);
        }

        static Dictionary<string, string> imageMappings = new Dictionary<string, string>();
        public static string GetImage(string title)
        {
            if (knownEvents.Contains(title.ToLower()) )
                return imageMappings[title];
            else
                return unknownEventImage;
        }

        static List<string> knownEvents = new List<string>();
        public static List<string> GetEventTypes()
        {
            return knownEvents;
        }

    }

}
