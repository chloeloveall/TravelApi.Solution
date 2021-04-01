namespace TravelApi.Models
{
  public class Review
  {
    public int ReviewId {get; set; }
    public int Rating { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public int LocationId { get; set; }
  }
}