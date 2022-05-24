namespace KsiazeczkaPTTK.DAL.InputModels
{
    public class SegmentInput
    {
        public string Destination { get; set; }
        public int Altitude { get; set; }
        public IEnumerable<string> StartPoints { get; set; }
        public IEnumerable<int> PointsToDestination { get; set; }
        public IEnumerable<int> PointsFromDestination { get; set; }
    }
}
