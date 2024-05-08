namespace VehicleAlert.Core
{
    public class ActionVehicleView
    {
        public int Id { get; set; }
        public int VehicleViewId { get; set; }
        public string Description { get; set; }
        public int LastActionCourse { get; set; }
        public int? Cost { get; set; }
        public int Interval { get; set; }
        public int KmToServis { get; set; }
        public bool Alert { get; set; }
        public decimal progress { get; set; }
        public bool IsSelected { get; set; }
    }
}
