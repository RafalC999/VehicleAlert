using QuestPDF.Fluent;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using VehicleAlert.Core;
using VehicleAlert.Core.PdfCreator;
using VehicleAlert.Core.Services.Implementations;
using VehicleAlert.Windows;

namespace VehicleAlert
{
    /// <summary>
    /// Logika interakcji dla klasy VehiclePage.xaml
    /// </summary>
    public partial class VehiclePage : Page
    {
        public int nameSort { get; set; } = 1;
        public int intervalSort { get; set; } = 1;
        public int kmToServiceSort { get; set; } = 1;
        public int lastServiceSort { get; set; } = 1;
        public int progressSort { get; set; } = 1;
        public int selectedSort { get; set; } = 1;

        public VehiclePage()
        {
            InitializeComponent();
            DataContext = new VehiclePagesView();

        }


        private void Button_UpdateCourse(object sender, RoutedEventArgs e)
        {
            VehiclePagesView selectedVehicle = new VehiclePagesView();
            selectedVehicle = (VehiclePagesView)DATA.DataContext;
            VehicleService.UpdateCourse(selectedVehicle);
            DATA.Items.Refresh();
        }


        public void Button_DeleteVehicle(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy chcesz usunąc wybrany pojazd?", "Usuwanie pojazdu?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                VehiclePagesView selectedVehicle = new VehiclePagesView();
                selectedVehicle = (VehiclePagesView)DATA.DataContext;
                //selectedVehicle.DeleteVehicle();
                BaseService.DeleteVehicle(selectedVehicle);

            }
        }

        private void Button_AddVehicle(object sender, RoutedEventArgs e)
        {
            AddVehicleWindow vehicleWindow = new AddVehicleWindow();
            vehicleWindow.DataContext = this.DataContext;
            vehicleWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            vehicleWindow.ShowDialog();
        }

        private void Button_AddAction(object sender, RoutedEventArgs e)
        {
            VehiclePagesView selectedVehicle = new VehiclePagesView();
            selectedVehicle = (VehiclePagesView)DATA.DataContext;
            if (selectedVehicle.SelectedVehicle != null)
            {
                AddAction actionWindow = new AddAction();
                actionWindow.DataContext = this.DataContext;
                actionWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                actionWindow.Show();
            }
            else
            {
                MessageBox.Show("Aby dodać akcję, wybierz pojazd!", "Ostrzeżenie", MessageBoxButton.OK);
            }

        }
        private void Button_EditAction(object sender, RoutedEventArgs e)
        {
            VehiclePagesView selectedVehicle = new VehiclePagesView();
            selectedVehicle = (VehiclePagesView)DATA.DataContext;
            //selectedVehicle.LoadSelectedAction();
            ActionService.LoadSelectedAction(selectedVehicle);
            if (selectedVehicle.quantityOfSelected == 0)
            {
                MessageBox.Show("Wybierz akcję!", "Ostrzeżenie", MessageBoxButton.OK);

            }
            else if (selectedVehicle.quantityOfSelected > 1)
            {
                MessageBox.Show("Aby edytować, wybierz tylko jedną akcję!", "Ostrzeżenie", MessageBoxButton.OK);
            }
            else
            {
                EditAction editActionWindow = new EditAction();
                editActionWindow.DataContext = this.DataContext;
                editActionWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //selectedVehicle.LoadSelectedAction();
                ActionService.LoadSelectedAction(selectedVehicle);
                editActionWindow.ShowDialog();
            }


        }
        //private void Button_Click_4(object sender, RoutedEventArgs e)
        //{

        //    VEH.Items.Refresh();
        //}

        private void Button_DeleteAction(object sender, RoutedEventArgs e)
        {
            VehiclePagesView selectedVehicle = new VehiclePagesView();
            selectedVehicle = (VehiclePagesView)DATA.DataContext;
            ActionService.DeleteSelectedAction(selectedVehicle);
            DATA.Items.Refresh();
        }

        private void VEH_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetStartStyle();
        }

        private void DataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            foreach (DataGridColumn dataGridColumn in dataGrid.Columns)
            {
                if (TryFindColumnHeader(dataGrid, dataGridColumn.DisplayIndex, out DataGridColumnHeader columnHeader))
                {
                    columnHeader.Click += (sender, args) => SortingTable(columnHeader.DisplayIndex);
                }
            }
        }

        public bool TryFindColumnHeader(
          DependencyObject dataGrid,
          int columnIndex,
          out DataGridColumnHeader dataGridColumnHeader)
        {
            dataGridColumnHeader = null;

            for (var childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(dataGrid); childIndex++)
            {
                DependencyObject childElement = VisualTreeHelper.GetChild(dataGrid, childIndex);

                if (childElement is DataGridColumnHeader columnHeader && columnHeader.DisplayIndex.Equals(columnIndex))
                {
                    dataGridColumnHeader = columnHeader;
                    return true;
                }

                if (TryFindColumnHeader(childElement, columnIndex, out dataGridColumnHeader))
                {
                    return true;
                }
            }
            return false;
        }


        public void SortingTable(int header)
        {
            VehiclePagesView dGrid = new VehiclePagesView();
            dGrid = (VehiclePagesView)DATA.DataContext;

            switch (header)
            {
                case 0:

                    if (nameSort == 1)
                    {
                        SetStartStyle();
                        dGrid.SortingDirection = 1;
                        dGrid.Sorting(("Description"));
                        description.HeaderStyle = (Style)DATA.FindResource("AscStyle");
                        nameSort = 2;
                        break;
                    }
                    if (nameSort == 2)
                    {
                        SetStartStyle();
                        dGrid.Sorting("Description");
                        description.HeaderStyle = (Style)DATA.FindResource("DescStyle");
                        nameSort = 1;
                        break;
                    }
                    break;
                case 1:
                    if (intervalSort == 1)
                    {
                        SetStartStyle();
                        dGrid.SortingDirection = 1;
                        dGrid.Sorting("Interval");
                        interval.HeaderStyle = (Style)DATA.FindResource("AscStyle");
                        intervalSort = 2;
                        break;
                    }
                    if (intervalSort == 2)
                    {
                        SetStartStyle();
                        dGrid.Sorting("Interval");
                        interval.HeaderStyle = (Style)DATA.FindResource("DescStyle");
                        intervalSort = 1;
                        break;
                    }
                    break;
                case 2:
                    if (kmToServiceSort == 1)
                    {
                        SetStartStyle();
                        dGrid.SortingDirection = 1;
                        dGrid.Sorting("LastActionCourse");
                        lastService.HeaderStyle = (Style)DATA.FindResource("AscStyle");
                        kmToServiceSort = 2;
                        break;
                    }
                    if (kmToServiceSort == 2)
                    {
                        SetStartStyle();
                        dGrid.Sorting("LastActionCourse");
                        lastService.HeaderStyle = (Style)DATA.FindResource("DescStyle");
                        kmToServiceSort = 1;
                        break;
                    }
                    break;
                case 3:
                    if (kmToServiceSort == 1)
                    {
                        SetStartStyle();
                        dGrid.SortingDirection = 1;
                        dGrid.Sorting("KmToServis");
                        kmToService.HeaderStyle = (Style)DATA.FindResource("AscStyle");
                        kmToServiceSort = 2;
                        break;
                    }
                    if (kmToServiceSort == 2)
                    {
                        SetStartStyle();
                        dGrid.Sorting("KmToServis");
                        kmToService.HeaderStyle = (Style)DATA.FindResource("DescStyle");
                        kmToServiceSort = 1;
                        break;
                    }
                    break;
                case 4:
                    if (progressSort == 1)
                    {
                        SetStartStyle();
                        dGrid.SortingDirection = 1;
                        dGrid.Sorting("KmToServis");
                        progress.HeaderStyle = (Style)DATA.FindResource("DescStyle");
                        progressSort = 2;
                        break;
                    }
                    if (progressSort == 2)
                    {
                        SetStartStyle();
                        dGrid.Sorting("KmToServis");
                        progress.HeaderStyle = (Style)DATA.FindResource("AscStyle");
                        progressSort = 1;
                        break;
                    }
                    break;
                case 5:
                    if (selectedSort == 1)
                    {
                        SetStartStyle();
                        dGrid.SortingDirection = 1;
                        dGrid.Sorting("KmToServis");
                        selected.HeaderStyle = (Style)DATA.FindResource("AscStyle");
                        selectedSort = 2;
                        break;
                    }
                    if (selectedSort == 2)
                    {
                        SetStartStyle();
                        dGrid.Sorting("KmToServis");
                        selected.HeaderStyle = (Style)DATA.FindResource("DescStyle");
                        selectedSort = 1;
                        break;
                    }
                    break; // naprawić
            }
        }
        public void SetStartStyle()
        {
            description.HeaderStyle = (Style)DATA.FindResource("StartStyle");
            interval.HeaderStyle = (Style)DATA.FindResource("StartStyle");
            kmToService.HeaderStyle = (Style)DATA.FindResource("StartStyle");
            lastService.HeaderStyle = (Style)DATA.FindResource("StartStyle");
            progress.HeaderStyle = (Style)DATA.FindResource("StartStyle");
            selected.HeaderStyle = (Style)DATA.FindResource("StartStyle");
            nameSort = 1;
            intervalSort = 1;
            kmToServiceSort = 1;
            lastServiceSort = 1;
            progressSort = 1;
            selectedSort = 1;
        }

        private void Button_NextPage(object sender, RoutedEventArgs e)
        {
            VehiclePagesView selectedVehicle = new VehiclePagesView();
            selectedVehicle = (VehiclePagesView)DATA.DataContext;

            PaginationService.NextPage(selectedVehicle);
        }

        private void Button_PreviousPage(object sender, RoutedEventArgs e)
        {
            VehiclePagesView selectedVehicle = new VehiclePagesView();
            selectedVehicle = (VehiclePagesView)DATA.DataContext;

            PaginationService.PreviousPage(selectedVehicle);
        }

        private void Button_FirstPage(object sender, RoutedEventArgs e)
        {
            VehiclePagesView selectedVehicle = new VehiclePagesView();
            selectedVehicle = (VehiclePagesView)DATA.DataContext;

            PaginationService.FirstPage(selectedVehicle);
        }

        private void Button_LastPage(object sender, RoutedEventArgs e)
        {
            VehiclePagesView selectedVehicle = new VehiclePagesView();
            selectedVehicle = (VehiclePagesView)DATA.DataContext;

            PaginationService.LastPage(selectedVehicle);
        }

        private void Button_CreatePDF(object sender, RoutedEventArgs e)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            QuestPDF.Settings.EnableDebugging = false;
            VehiclePagesView selectedVehicle = new VehiclePagesView();
            selectedVehicle = (VehiclePagesView)DATA.DataContext;

            var filePath = "test.pdf";

            var model = RaportDataSource.GetRaportDetails(selectedVehicle);
            var document = new RaportDocument(model);
            document.GeneratePdf(filePath);

            Process.Start("explorer.exe", filePath);

        }
    }
}

