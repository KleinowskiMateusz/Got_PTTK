namespace KsiazeczkaPttk.Domain.Models
{
    public class NeighboringSegment
    {
        public int Id { get; set; }

        public int Version { get; set; }

        public string Name { get; set; }

        public int Points { get; set; }

        public bool IsBack { get; set; }

        public int BackPoints { get; set; }

        public int FromId { get; set; }

        public TerrainPoint From { get; set; }

        public int Target { get; set; }

        public TerrainPoint TargetId { get; set; }

        public int MountainRangeId { get; set; }

        public MountainRange MountainRange { get; set; }

        public string TouristsBookOwner { get; set; }

        public TouristsBook TouristsBook { get; set; }
    }
}
