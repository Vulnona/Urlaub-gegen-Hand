namespace UGHApi.ViewModels;

public class ReviewOfferDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public double AverageRating { get; set; }
    public byte[] ImageData { get; set; }
    public string ImageMimeType { get; set; }
}
