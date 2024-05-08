using System.Collections.ObjectModel;

namespace VehicleAlert.Core
{
    public class VehicleView
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string VinNumber { get; set; }
        public int Course { get; set; }
        public string Plate { get; set; }
        public ObservableCollection<ActionVehicleView> AlertActions { get; set; } = new ObservableCollection<ActionVehicleView>();

    }
}
