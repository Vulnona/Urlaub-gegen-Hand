namespace UGHApi.Models
{
    public class OfferWithImage
    {
        public Offer offer { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
