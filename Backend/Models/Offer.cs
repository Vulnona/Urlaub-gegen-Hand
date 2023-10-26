namespace UGHApi.Models
{
    public class Offer : Notification
    {
        public Offer(string title, string caption, Profile userProfile) : base(title, caption, userProfile)
        {
        }


        public int Offer_ID{get;set;}
        
        //TODO: Aufgabe, bei was wird Hilfe benötigt
        
        public int NumberOfPersons{get;set;}
        
        //TODO: Rating

        public string Housing{get;set;}
        public string MealsIncluded{get;set;}

    }
}