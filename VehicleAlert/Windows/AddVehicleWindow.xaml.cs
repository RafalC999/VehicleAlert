using System.Windows;
using VehicleAlert.Core;
using VehicleAlert.Core.Services.Implementations;

namespace VehicleAlert
{
    /// <summary>
    /// Interaction logic for AddVehicleWindow.xaml
    /// </summary>
    public partial class AddVehicleWindow : Window
    {
        private VehiclePagesView _view;
        public AddVehicleWindow()
        {
            InitializeComponent();
            DataContext = new VehiclePagesView();
            _view = new VehiclePagesView();

        }

        private void Button_AddVehicle(object sender, RoutedEventArgs e)
        {
            _view = (VehiclePagesView)DataContext;
            VehicleService.AddNewVehicle(_view);
            if (_view.validationResult != null)
            {
                if (_view.validationResult.IsValid)
                {
                    this.Close();
                }
            }
        }

        private void Name_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _view = (VehiclePagesView)DataContext;
            if (_view.ErrorName != null)
            {
                _view.ErrorName = null;
                _view.RefreshErrors();
            }
        }

        private void Course_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _view = (VehiclePagesView)DataContext;
            if (_view.ErrorCourse != null)
            {
                _view.ErrorCourse = null;
                _view.RefreshErrors();
            }
        }

        private void Plate_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _view = (VehiclePagesView)DataContext;
            if (_view.ErrorPlate != null)
            {
                _view.ErrorPlate = null;
                _view.RefreshErrors();
            }
        }
    }
}
