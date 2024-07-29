namespace UGHApi.Models
{
    public class OfferViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Contact { get; set; }
        public string Accomodation { get; set; }
        public string accomodationsuitable { get; set; }
        public string skills { get; set; }
        public int Skill_ID { get; set; }
        public int Region_ID { get; set; }
        public int User_Id { get; set; }
        public IFormFile? Image { get; set; }
    }
}
