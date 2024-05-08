namespace VehicleAlert.Core.PdfCreator
{
    public static class RaportDataSource
    {
        public static RaportModel GetRaportDetails(VehiclePagesView vehiclePagesView)
        {
            var currentVehicle = vehiclePagesView.SelectedVehicle;

            var actions = currentVehicle.AlertActions.Select(i => GetActionItem(i)).ToList();

            return new RaportModel
            {
                Name = currentVehicle.Brand,
                Plate = currentVehicle.Plate,
                Course = currentVehicle.Course,
                GeneratingDate = DateTime.UtcNow,
                Actions = actions
            };
        }

        private static ActionItem GetActionItem(ActionVehicleView actionVehicleView)
        {

            return new ActionItem
            {
                Description = actionVehicleView.Description,
                Interval = actionVehicleView.Interval,
                KmToServis = actionVehicleView.KmToServis,
                LastActionCourse = actionVehicleView.LastActionCourse,
                progress = actionVehicleView.progress

            };
        }
    }
}
