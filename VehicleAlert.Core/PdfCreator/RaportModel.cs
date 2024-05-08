namespace VehicleAlert.Core.PdfCreator
{
    public class RaportModel
    {
        public string Name { get; set; }
        public int Course { get; set; }
        public string? Plate { get; set; }
        public DateTime GeneratingDate { get; set; }
        public List<ActionItem> Actions { get; set; }
    }
}
