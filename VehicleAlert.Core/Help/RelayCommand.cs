using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VehicleAlert.Core
{
    public class RelayCommand : ICommand
    {
        private Action vAction;

        private Action<object> fAction;
        private Action<object, object> fAction2;

        public RelayCommand(Action<object, object> faction2)
        {
            this.fAction2 = faction2;
        }

        public RelayCommand(Action<object> faction)
        {
            this.fAction = faction;
        }

        public RelayCommand(Action action)
        {
            vAction = action;
        }



        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (vAction != null)
            {
                vAction();
            }

            if (fAction != null)
            {
                fAction(parameter);
            } 
        }
        public void Execute(object? parameter, object? parameter2)
        {
            if (vAction != null)
            {
                vAction();
            }

            if (fAction != null)
            {
                fAction2(parameter,parameter2);
            }
        }
    }
}
