using FluentValidation;
using System.Collections.ObjectModel;

namespace VehicleAlert.Core.Services.Implementations
{
    public static class BaseService
    {
        public static void LoadVehicles(ObservableCollection<VehicleView> vehicleViews)
        {
            foreach (var task in DatabaseLocator.Database.Vehicles.ToList())
            {
                vehicleViews.Add(new VehicleView
                {
                    Id = task.Id,
                    Brand = task.Brand,
                    Course = task.Course,
                    Plate = task.Plate,
                    VinNumber = task.VinNumber,
                });
            }
        }
        public static void LoadAlerts(ObservableCollection<VehicleView> vehicleViews)
        {
            foreach (var vehicle in vehicleViews.ToList())
            {
                foreach (var task in DatabaseLocator.Database.VehicleActions
                    .Where(c => c.VehicleId == vehicle.Id)
                    .Select(c => new { c.Description, c.LastActionCourse, c.Interval, c.KmToServis, c.progress, c.Id })
                    .ToList())
                {
                    vehicle.AlertActions.Add(new ActionVehicleView
                    {
                        Id = task.Id,
                        Description = task.Description,
                        LastActionCourse = task.LastActionCourse,
                        Interval = task.Interval,
                        VehicleViewId = task.Id,
                        KmToServis = task.KmToServis,
                        progress = task.progress
                    });
                }
            }
        }

        public static void DeleteVehicle(VehiclePagesView vehiclePagesView)
        {
            BaseService.DeleteActions(vehiclePagesView.AlertActions, vehiclePagesView.SelectedVehicle);
            var vehicle = vehiclePagesView.SelectedVehicle;
            vehiclePagesView.VehicleViews.Remove(vehicle);
            var vehicleDb = DatabaseLocator.Database.Vehicles.FirstOrDefault(v => v.Id == vehicle.Id);

            if (vehicleDb != null)
            {
                DatabaseLocator.Database.Vehicles.Remove(vehicleDb);
            }
            DatabaseLocator.Database.SaveChanges();
            vehiclePagesView.ActualCourse = null;
            vehiclePagesView.OnPropertyChanged(nameof(vehiclePagesView.ActualCourse));
        }

        public static void DeleteActions(ObservableCollection<ActionVehicleView> alertActions, VehicleView selectedVehicle)
        {
            var selectedAction = alertActions.Where(x => x.VehicleViewId == selectedVehicle.Id).ToList();
            foreach (var action in selectedAction)
            {
                alertActions.Remove(action);
                var selectedActionDb = DatabaseLocator.Database.VehicleActions.FirstOrDefault(x => x.VehicleId == selectedVehicle.Id);
                if (selectedActionDb != null)
                {
                    DatabaseLocator.Database.VehicleActions.Remove(selectedActionDb);
                }
            }
        }
    }
}
