using System.Collections.ObjectModel;
using VehicleAlert.Data.Entities;

namespace VehicleAlert.Data
{
    public class Vehicle
    {

        public int Id { get; set; }

        public string Brand { get; set; }

        public string Plate { get; set; }

        //public bool IsSelected { get; set; }

        public string VinNumber { get; set; }
        public int Course { get; set; }

        public ObservableCollection<ActionVehicle> VehicleActions { get; set; } = new();

    }
}

