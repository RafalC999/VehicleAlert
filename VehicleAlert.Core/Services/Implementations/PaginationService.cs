using System.Collections.ObjectModel;

namespace VehicleAlert.Core.Services.Implementations
{
    public class PaginationService : VehiclePagesView
    {
        public static void Paging(VehiclePagesView vehiclePagesView)
        {
            if (vehiclePagesView.SelectedVehicle != null)
            {
                int elements = vehiclePagesView.SelectedVehicle.AlertActions.Count();
                if (elements % vehiclePagesView.ElementsPerPage != 0)
                {
                    vehiclePagesView.Pages = (int)(elements / vehiclePagesView.ElementsPerPage) + 1;
                }
                else
                {
                    vehiclePagesView.Pages = (int)(elements / vehiclePagesView.ElementsPerPage);
                    if (vehiclePagesView.Pages == 0)
                    {
                        vehiclePagesView.Pages = 1;
                    }
                }
                vehiclePagesView.OnPropertyChanged(nameof(vehiclePagesView.Pages));
            }
        }
        public static ObservableCollection<ActionVehicleView> ElementsOnPage(VehiclePagesView vehiclePagesView)
        {
            vehiclePagesView.PageAlertActions = new ObservableCollection<ActionVehicleView>(vehiclePagesView.AlertActions
                        .Skip((vehiclePagesView.CurrentPage - 1) * vehiclePagesView.ElementsPerPage)
                        .Take(vehiclePagesView.ElementsPerPage));
            vehiclePagesView.OnPropertyChanged(nameof(vehiclePagesView.PageAlertActions));

            return vehiclePagesView.PageAlertActions;
        }
        public static void NextPage(VehiclePagesView vehiclePagesView)
        {
            if (vehiclePagesView.CurrentPage < vehiclePagesView.Pages)
            {
                vehiclePagesView.CurrentPage++;
                ElementsOnPage(vehiclePagesView);
                vehiclePagesView.OnPropertyChanged(nameof(vehiclePagesView.CurrentPage));
            }
        }
        public static void PreviousPage(VehiclePagesView vehiclePagesView)
        {
            if (vehiclePagesView.CurrentPage > 1)
            {
                vehiclePagesView.CurrentPage--;
                ElementsOnPage(vehiclePagesView);
                vehiclePagesView.OnPropertyChanged(nameof(CurrentPage));
            }
        }
        public static void FirstPage(VehiclePagesView vehiclePagesView)
        {
            vehiclePagesView.CurrentPage = 1;
            ElementsOnPage(vehiclePagesView);
            vehiclePagesView.OnPropertyChanged(nameof(CurrentPage));
        }
        public static void LastPage(VehiclePagesView vehiclePagesView)
        {
            vehiclePagesView.CurrentPage = vehiclePagesView.Pages;
            ElementsOnPage(vehiclePagesView);
            vehiclePagesView.OnPropertyChanged(nameof(CurrentPage));
        }
        public static void CheckPages(VehiclePagesView vehiclePagesView)
        {
            int currentPages = vehiclePagesView.Pages;

            PaginationService.Paging(vehiclePagesView);
            PaginationService.ElementsOnPage(vehiclePagesView);

            if (vehiclePagesView.Pages > currentPages)
            {
                PaginationService.NextPage(vehiclePagesView);
            }
            else if (vehiclePagesView.Pages < currentPages)
            {
                PaginationService.PreviousPage(vehiclePagesView);
            }
        }
    }
}
