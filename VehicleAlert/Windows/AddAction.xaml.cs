using System.Windows;
using VehicleAlert.Core;
using VehicleAlert.Core.Services.Implementations;

namespace VehicleAlert.Windows
{
    /// <summary>
    /// Interaction logic for AddAction.xaml
    /// </summary>
    public partial class AddAction : Window
    {
        private VehiclePagesView _view;
        public AddAction()
        {
            InitializeComponent();
            DataContext = new VehiclePagesView();
            _view = new VehiclePagesView();
        }

        private void Button_AddAction(object sender, RoutedEventArgs e)
        {
            _view = (VehiclePagesView)DataContext;
            ActionService.AddNewActionVehicle(_view);
            if (_view.validationResult != null)
            {
                if (_view.validationResult.IsValid)
                {
                    this.Close();
                }
            }
        }

        private void Description_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _view = (VehiclePagesView)DataContext;
            if (_view.ErrorDescription != null)
            {
                _view.ErrorDescription = null;
                _view.RefreshErrors();
            }
        }

        private void LastActionCourse_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _view = (VehiclePagesView)DataContext;
            if (_view.ErrorLastActionCourse != null)
            {
                _view.ErrorLastActionCourse = null;
                _view.RefreshErrors();
            }
        }

        private void Interval_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _view = (VehiclePagesView)DataContext;
            if (_view.ErrorInterval != null)
            {
                _view.ErrorInterval = null;
                _view.RefreshErrors();
            }
        }
    }
}
