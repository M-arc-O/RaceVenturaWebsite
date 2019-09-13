namespace Adventure4YouAPI.ViewModels.Points
{
    public class PointDetailViewModel : PointViewModel
    {
        public PointTypeViewModel Type { get; set; }
        public int Value { get; set; }
        public string Coordinates { get; set; }
        public string Answer { get; set; }
        public string Message { get; set; }
    }
}
