using Microsoft.EntityFrameworkCore;
using VehicleAlert.Core.Validation;
using VehicleAlert.Data.Entities;

namespace VehicleAlert.Core.Services.Implementations
{
    public class ActionService : VehiclePagesView
    {
        private static ActionVehicleView GetActionVehicleView(VehiclePagesView vehiclePagesView)
        {
            int? currentId = DatabaseLocator.Database.VehicleActions.Max(c => (int?)c.Id);
            return new ActionVehicleView
            {
                Id = currentId.Value,
                Description = vehiclePagesView.NewDescription,
                LastActionCourse = (int)vehiclePagesView.NewLastActionCourse,
                Interval = (int)vehiclePagesView.NewInterval,
                KmToServis = (int)vehiclePagesView.NewLastActionCourse + (int)vehiclePagesView.NewInterval - vehiclePagesView.SelectedVehicle.Course,
                VehicleViewId = vehiclePagesView.SelectedVehicle.Id,
                progress = ActionService.Progress(vehiclePagesView)
            };
        }
        public static void AddNewActionVehicle(VehiclePagesView vehiclePagesView)
        {
            AddActionValidation actionValidation = new AddActionValidation();
            vehiclePagesView.validationResult = actionValidation.Validate(vehiclePagesView);

            if (vehiclePagesView.validationResult.IsValid)
            {
                ActionService.AddNewActionVehicleToDatabase(vehiclePagesView);
                vehiclePagesView.AlertActions.Add(GetActionVehicleView(vehiclePagesView));
                vehiclePagesView.RefreshNewAction();
            }
            else
            {
                actionValidation.ActionValidationErrors(vehiclePagesView);
            }
            PaginationService.CheckPages(vehiclePagesView);
        }
        private static void AddNewActionVehicleToDatabase(VehiclePagesView vehiclePagesView)
        {
            DatabaseLocator.Database.VehicleActions.Add(new ActionVehicle
            {
                VehicleId = vehiclePagesView.SelectedVehicle.Id,
                Description = vehiclePagesView.NewDescription,
                LastActionCourse = (int)vehiclePagesView.NewLastActionCourse,
                Interval = (int)vehiclePagesView.NewInterval,
                KmToServis = (int)vehiclePagesView.NewLastActionCourse + (int)vehiclePagesView.NewInterval - vehiclePagesView.SelectedVehicle.Course,
                progress = ActionService.Progress(vehiclePagesView)
            });
            DatabaseLocator.Database.SaveChanges();
        }

        public static void LoadSelectedAction(VehiclePagesView vehiclePagesView)
        {
            vehiclePagesView.quantityOfSelected = vehiclePagesView.AlertActions.Where(x => x.IsSelected).ToList().Count();

            var action = vehiclePagesView.AlertActions.FirstOrDefault(x => x.IsSelected);
            if (vehiclePagesView.quantityOfSelected == 1)
            {
                vehiclePagesView.NewDescription = action.Description;
                vehiclePagesView.NewInterval = action.Interval;
                vehiclePagesView.NewLastActionCourse = action.LastActionCourse;
            }
        }
        public static void EditSelectedAction(VehiclePagesView vehiclePagesView)
        {
            var action = vehiclePagesView.AlertActions.FirstOrDefault(x => x.IsSelected);
            if (action != null)
            {
                action.Description = vehiclePagesView.NewDescription;
                action.Interval = (int)vehiclePagesView.NewInterval;
                action.LastActionCourse = (int)vehiclePagesView.NewLastActionCourse;
                action.KmToServis = (int)vehiclePagesView.NewLastActionCourse + (int)vehiclePagesView.NewInterval - vehiclePagesView.SelectedVehicle.Course;
                action.progress = ActionService.UpdateProgress(action, vehiclePagesView.SelectedVehicle);
            }

            ActionVehicle actionDb = DatabaseLocator.Database.VehicleActions.FirstOrDefault(x => x.Id == action.Id);
            if (actionDb != null)
            {
                actionDb.Description = vehiclePagesView.NewDescription;
                actionDb.Interval = (int)vehiclePagesView.NewInterval;
                actionDb.LastActionCourse = (int)vehiclePagesView.NewLastActionCourse;
                actionDb.KmToServis = (int)vehiclePagesView.NewLastActionCourse + (int)vehiclePagesView.NewInterval - vehiclePagesView.SelectedVehicle.Course;
                actionDb.progress = ActionService.UpdateProgress(action, vehiclePagesView.SelectedVehicle);

                DatabaseLocator.Database.Entry(actionDb).State = EntityState.Modified;
                DatabaseLocator.Database.SaveChanges();
            }
            PaginationService.ElementsOnPage(vehiclePagesView);
        }
        public static void DeleteSelectedAction(VehiclePagesView vehiclePagesView)
        {
            var selectedVehicle = vehiclePagesView.AlertActions.Where(x => x.IsSelected).ToList();
            foreach (var task in selectedVehicle)
            {
                vehiclePagesView.AlertActions.Remove(task);
                var foundEntity = DatabaseLocator.Database.VehicleActions.FirstOrDefault(x => x.VehicleId == vehiclePagesView.SelectedVehicle.Id);

                if (foundEntity != null)
                {
                    DatabaseLocator.Database.VehicleActions.Remove(foundEntity);
                    DatabaseLocator.Database.SaveChanges();
                }
            }
            PaginationService.CheckPages(vehiclePagesView);
        }

        public static decimal Progress(VehiclePagesView vehiclePagesView)
        {
            decimal progress = ((vehiclePagesView.SelectedVehicle.Course - (int)vehiclePagesView.NewLastActionCourse) / (decimal)vehiclePagesView.NewInterval * 100);
            if (progress > 100)
            {
                return 100;
            }
            else if (progress < 0)
            {
                return 0;
            }
            return progress;
        }

        public static decimal UpdateProgress(ActionVehicleView alert, VehicleView selectedVehicle)
        {
            decimal progress = ((selectedVehicle.Course - alert.LastActionCourse) / (decimal)alert.Interval * 100);
            if (progress > 100)
            {
                return 100;
            }
            else if (progress < 0)
            {
                return 0;
            }
            return progress;
        }

        public static decimal UpdateProgress(ActionVehicle alert, VehicleView selectedVehicle)
        {
            decimal progress = ((selectedVehicle.Course - alert.LastActionCourse) / (decimal)alert.Interval * 100);
            if (progress > 100)
            {
                return 100;
            }
            else if (progress < 0)
            {
                return 0;
            }
            return progress;
        }
    }
}
