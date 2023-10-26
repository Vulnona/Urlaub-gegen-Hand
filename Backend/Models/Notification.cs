namespace UGHApi.Models
{
    public class Notification
    {
        public Notification(string title, string caption, Profile userProfile) 
        {
            
                this.Title = title;
                this.Caption = caption;
                this.UserProfile = userProfile;
               
        }
                public int Notification_ID{get;set;}
        public string Title{get;set;}
        public string Caption{get;set;}
        public Profile UserProfile{get;set;}
    }
}