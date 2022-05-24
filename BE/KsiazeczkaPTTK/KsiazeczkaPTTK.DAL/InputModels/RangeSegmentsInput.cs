namespace KsiazeczkaPTTK.DAL.InputModels
{
    public class RangeSegmentsInput
    {
        public string Group { get; set; }
        public string Range { get; set; }

        public IEnumerable<SegmentInput> Segments { get; set; }
    }
}
