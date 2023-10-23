namespace TravelPal.Models
{
    public interface PackingListItem
    {
        public string Name { get; set; }
        public string GetInfo();
    }
}
