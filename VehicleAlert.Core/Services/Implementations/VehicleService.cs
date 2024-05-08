using FluentValidation;
using VehicleAlert.Core.Validation;
using VehicleAlert.Data;

namespace VehicleAlert.Core.Services.Implementations
{
    public class VehicleService
    {
        private static VehicleView GetVehicleView(VehiclePagesView vehiclePagesView)
        {
            int? currentId = DatabaseLocator.Database.Vehicles.Max(c => (int?)c.Id);

            return new VehicleView
            {
                Id = currentId.Value,
                Brand = vehiclePagesView.NewVehicleName,
                Course = (int)vehiclePagesView.NewVehicleCourse,
                Plate = vehiclePagesView.NewVehiclePlate,
                VinNumber = vehiclePagesView.NewVehicleVinNumber,
            };
        }
        public static void AddNewVehicle(VehiclePagesView vehiclePagesView)
        {
            AddVehicleValidation vehicleValidation = new AddVehicleValidation();
            vehiclePagesView.validationResult = vehicleValidation.Validate(vehiclePagesView);

            if (vehiclePagesView.validationResult.IsValid)
            {
                AddNewVehicleToDatabase(vehiclePagesView);
                vehiclePagesView.VehicleViews.Add(GetVehicleView(vehiclePagesView));
                vehiclePagesView.RefreshNewVehicle();
            }
            else
            {
                vehicleValidation.VehicleValidationErrors(vehiclePagesView);
            }
        }
        public static void AddNewVehicleToDatabase(VehiclePagesView vehiclePagesView)
        {
            DatabaseLocator.Database.Vehicles.Add(new Vehicle
            {
                Brand = vehiclePagesView.NewVehicleName,
                Course = (int)vehiclePagesView.NewVehicleCourse,
                Plate = vehiclePagesView.NewVehiclePlate,
                VinNumber = vehiclePagesView.NewVehicleVinNumber
            });
            DatabaseLocator.Database.SaveChanges();
        }

        public static void UpdateCourse(VehiclePagesView vehiclePagesView)
        {
            vehiclePagesView.SelectedVehicle.Course = (int)vehiclePagesView.UpdatedCourse;
            vehiclePagesView.ActualCourse = (int)vehiclePagesView.UpdatedCourse;
            vehiclePagesView.UpdatedCourse = null;

            var vehicleDB = DatabaseLocator.Database.Vehicles.FirstOrDefault(v => v.Id == vehiclePagesView.SelectedVehicle.Id);
            vehicleDB.Course = (int)vehiclePagesView.SelectedVehicle.Course;
            DatabaseLocator.Database.Vehicles.Update(vehicleDB);

            foreach (var item in vehiclePagesView.SelectedVehicle.AlertActions.ToList())
            {
                item.KmToServis = item.Interval - (vehiclePagesView.SelectedVehicle.Course - item.LastActionCourse);
                item.progress = ActionService.UpdateProgress(item, vehiclePagesView.SelectedVehicle);
            }

            foreach (var item in DatabaseLocator.Database.VehicleActions.Where(x => x.VehicleId == vehiclePagesView.SelectedVehicle.Id).ToList())
            {
                item.KmToServis = item.Interval - (vehiclePagesView.SelectedVehicle.Course - item.LastActionCourse);
                item.progress = ActionService.UpdateProgress(item, vehiclePagesView.SelectedVehicle);
                DatabaseLocator.Database.Update(item);
            }
            DatabaseLocator.Database.SaveChanges();
            vehiclePagesView.UpdatedCourse = null;
            vehiclePagesView.OnPropertyChanged(nameof(vehiclePagesView.ActualCourse));
            vehiclePagesView.OnPropertyChanged(nameof(vehiclePagesView.UpdatedCourse));
        }
    }
}
