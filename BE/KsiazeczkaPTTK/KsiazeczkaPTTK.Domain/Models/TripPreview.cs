namespace KsiazeczkaPttk.Domain.Models
{
    public class TripPreview
    {
        public Trip Trip { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Localization { get; set; }
        public int Points { get; set; }
    }
}
