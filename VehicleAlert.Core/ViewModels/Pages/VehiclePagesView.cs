using FluentValidation.Results;
using System.Collections.ObjectModel;
using VehicleAlert.Core.Services.Implementations;

namespace VehicleAlert.Core
{
    public class VehiclePagesView : BaseViewModel
    {
        private VehicleView _selectedVehicle;
        public ObservableCollection<VehicleView> VehicleViews { get; set; } = new ObservableCollection<VehicleView>();
        public ObservableCollection<ActionVehicleView> AlertActions { get; set; } = new ObservableCollection<ActionVehicleView>();
        public ObservableCollection<ActionVehicleView> PageAlertActions { get; set; } = new ObservableCollection<ActionVehicleView>();
        public ValidationResult validationResult { get; set; }
        public VehicleView SelectedVehicle
        {
            get { return _selectedVehicle; }

            set
            {
                _selectedVehicle = value;
                if (value != null)
                {
                    this.AlertActions = value.AlertActions;
                }

                SetCourse();
                OnPropertyChanged(nameof(SelectedVehicle));
                OnPropertyChanged(nameof(this.AlertActions));
                OnPropertyChanged(nameof(ActualCourse));
                OnPropertyChanged(nameof(NewVehicleVinNumber));

                //Paging();
                PaginationService.Paging(this);

                OnPropertyChanged(nameof(Pages));
                //ElementsOnPage();
                PaginationService.ElementsOnPage(this);
                OnPropertyChanged(nameof(PageAlertActions));
                OnPropertyChanged(nameof(CurrentPage));
            }
        }


        // Vehicle
        public string NewVehicleName { get; set; }
        public int? NewVehicleCourse { get; set; } = null;
        public string NewVehiclePlate { get; set; }
        public int? ActualCourse { get; set; } = null;
        public string NewVehicleVinNumber { get; set; }

        // VehicleError
        public string ErrorName { get; set; }
        public string ErrorCourse { get; set; }
        public string ErrorPlate { get; set; }

        // ActionError
        public string ErrorDescription { get; set; }
        public string ErrorLastActionCourse { get; set; }
        public string ErrorInterval { get; set; }
        public string ErrorKmToServis { get; set; }

        // Nowa akcja serwisowa
        public string NewDescription { get; set; }
        public int? NewLastActionCourse { get; set; } = null;
        public int? NewInterval { get; set; } = null;
        public int? _NewKmToServis { get; set; }
        public int? NewKmToServis
        {
            get { return _NewKmToServis; }
            set { _NewKmToServis = NewLastActionCourse + NewInterval - _selectedVehicle.Course; }
        }

        // Pages
        public int CurrentPage { get; set; } = 1;
        public int Pages { get; set; } = 1;
        public int ElementsPerPage = 12;
        public int SortingDirection = 1;

        // Selected
        public int quantityOfSelected { get; set; }
        public int? UpdatedCourse { get; set; } = null;


        public VehiclePagesView()
        {
            BaseService.LoadVehicles(VehicleViews);
            BaseService.LoadAlerts(VehicleViews);

        }


        public void SetCourse()
        {
            if (_selectedVehicle != null)
            {
                ActualCourse = _selectedVehicle.Course;
            }
        }

        public void Sorting(string selector)
        {
            var desiredProperty = typeof(ActionVehicleView).GetProperty(selector);

            if (SortingDirection == 1)
            {
                AlertActions = new ObservableCollection<ActionVehicleView>(AlertActions.OrderBy(i => desiredProperty.GetValue(i)));
                //ElementsOnPage();
                PaginationService.ElementsOnPage(this);
                SortingDirection = 2;
                //FirstPage();
                PaginationService.FirstPage(this);
            }
            else
            {
                AlertActions = new ObservableCollection<ActionVehicleView>(AlertActions.OrderByDescending(i => desiredProperty.GetValue(i)));
                //ElementsOnPage();
                PaginationService.ElementsOnPage(this);
                SortingDirection = 1;
                //FirstPage();
                PaginationService.FirstPage(this);
            }
        }

        public void RefreshErrors()
        {
            OnPropertyChanged(nameof(ErrorName));
            OnPropertyChanged(nameof(ErrorCourse));
            OnPropertyChanged(nameof(ErrorPlate));
            OnPropertyChanged(nameof(ErrorDescription));
            OnPropertyChanged(nameof(ErrorLastActionCourse));
            OnPropertyChanged(nameof(ErrorInterval));
            validationResult = null;

        }
        public void RefreshNewVehicle()
        {
            NewVehicleName = null;
            NewVehicleCourse = null;
            NewVehiclePlate = null;
            NewVehicleVinNumber = null;
            OnPropertyChanged(nameof(NewVehicleName));
            OnPropertyChanged(nameof(NewVehicleCourse));
            OnPropertyChanged(nameof(NewVehiclePlate));
            OnPropertyChanged(nameof(NewVehicleVinNumber));
        }
        public void RefreshNewAction()
        {
            NewDescription = null;
            NewLastActionCourse = null;
            NewInterval = null;
        }


        //public void LoadVehicles()
        //{
        //    foreach (var task in DatabaseLocator.Database.Vehicles.ToList())
        //    {
        //        VehicleViews.Add(new VehicleView
        //        {
        //            Id = task.Id,
        //            Name = task.Name,
        //            Course = task.Course,
        //            Plate = task.Plate,
        //        });
        //    }
        //}
        //public void LoadAlerts()
        //{
        //    foreach (var task2 in VehicleViews.ToList())
        //    {
        //        foreach (var task1 in DatabaseLocator.Database.VehicleActions
        //            .Where(c => c.VehicleId == task2.Id)
        //            .Select(c => new { c.Description, c.LastActionCourse, c.Interval, c.KmToServis, c.progress, c.Id })
        //            .ToList())
        //        {
        //            task2.AlertActions.Add(new ActionVehicleView
        //            {
        //                Id = task1.Id,
        //                Description = task1.Description,
        //                LastActionCourse = task1.LastActionCourse,
        //                Interval = task1.Interval,
        //                VehicleViewId = task2.Id,
        //                KmToServis = task1.KmToServis,
        //                progress = task1.progress
        //            });
        //        }
        //    }
        //}


        //public void AddNewVehicle()
        //{
        //    AddVehicleValidation addVehicleVal = new AddVehicleValidation();
        //    validationResult = addVehicleVal.Validate(this);

        //    if (validationResult.IsValid)
        //    {
        //        int? intId = DatabaseLocator.Database.Vehicles.Max(c => (int?)c.Id);
        //        var newVehicle = new VehicleView
        //        {
        //            Id = (int)intId + 1,
        //            Name = NewVehicleName,
        //            Course = (int)NewVehicleCourse,
        //            Plate = NewVehiclePlate,
        //        };

        //        VehicleViews.Add(newVehicle);
        //        AddNewVehicleToDatabase();
        //        RefreshNewVehicle();
        //    }
        //    else
        //    {
        //        if (validationResult.Errors.Where(p => p.ErrorCode == "Name").Count() != 0)
        //        {
        //            ErrorName = validationResult.Errors.Where(p => p.ErrorCode == "Name").Single().ErrorMessage;
        //        }
        //        if (validationResult.Errors.Where(p => p.ErrorCode == "Course").Count() != 0)
        //        {
        //            ErrorCourse = validationResult.Errors.Where(p => p.ErrorCode == "Course").Single().ErrorMessage;
        //        }
        //        if (validationResult.Errors.Where(p => p.ErrorCode == "Plate").Count() != 0)
        //        {
        //            ErrorPlate = validationResult.Errors.Where(p => p.ErrorCode == "Plate").Single().ErrorMessage;
        //        }
        //        RefreshData();
        //    }
        //}
        //public void AddNewVehicleToDatabase()
        //{
        //    DatabaseLocator.Database.Vehicles.Add(new Vehicle
        //    {
        //        Name = NewVehicleName,
        //        Course = (int)NewVehicleCourse,
        //        Plate = NewVehiclePlate,
        //    });
        //    DatabaseLocator.Database.SaveChanges();
        //}

        //public void AddNewActionVehicle()
        //{
        //    AddActionValidation addVehicleVal = new AddActionValidation();
        //    validationResult = addVehicleVal.Validate(this);
        //    if (validationResult.IsValid)
        //    {
        //        var newAction = new ActionVehicleView
        //        {
        //            Description = NewDescription,
        //            LastActionCourse = (int)NewLastActionCourse,
        //            Interval = (int)NewInterval,
        //            KmToServis = (int)NewLastActionCourse + (int)NewInterval - _selectedVehicle.Course,
        //            VehicleViewId = _selectedVehicle.Id,
        //            progress = Progress(),
        //        };
        //        AlertActions.Add(newAction);
        //        AddNewActionVehicleToDatabase();
        //        RefreshNewAction();
        //    }
        //    else
        //    {
        //        if (validationResult.Errors.Where(p => p.ErrorCode == "Name").Count() != 0)
        //        {
        //            ErrorDescription = validationResult.Errors.Where(p => p.ErrorCode == "Name").Single().ErrorMessage;
        //        }
        //        if (validationResult.Errors.Where(p => p.ErrorCode == "LastActionCourse").Count() != 0)
        //        {
        //            ErrorLastActionCourse = validationResult.Errors.Where(p => p.ErrorCode == "LastActionCourse").Single().ErrorMessage;
        //        }
        //        if (validationResult.Errors.Where(p => p.ErrorCode == "Interval").Count() != 0)
        //        {
        //            ErrorInterval = validationResult.Errors.Where(p => p.ErrorCode == "Interval").Single().ErrorMessage;
        //        }
        //        RefreshData();
        //    }

        //    int currentPages = Pages;
        //    //Paging();
        //    PaginationService.Paging(this);
        //    //ElementsOnPage();
        //    PaginationService.ElementsOnPage(this);

        //    if (Pages > currentPages)
        //    {
        //        //NextPage();
        //        PaginationService.NextPage(this);
        //    }
        //}
        //private void AddNewActionVehicleToDatabase()
        //{
        //    DatabaseLocator.Database.VehicleActions.Add(new ActionVehicle
        //    {
        //        VehicleId = _selectedVehicle.Id,
        //        Description = NewDescription,
        //        LastActionCourse = (int)NewLastActionCourse,
        //        Interval = (int)NewInterval,
        //        KmToServis = (int)NewLastActionCourse + (int)NewInterval - _selectedVehicle.Course,
        //        progress = Progress()
        //    });
        //    DatabaseLocator.Database.SaveChanges();
        //}


        //public ICommand AddNewVehicleCommand { get; set; }
        //public ICommand AddNewActionVehicleCommand { get; set; }
        //public ICommand AddToServis { get; set; }
        //public ICommand AddUpdateCourse { get; set; }
        //public ICommand DeleteVehicleCommand { get; set; }
        //public ICommand DeleteSelectedTaskCommand { get; set; }
        //public ICommand NextPageCommand { get; set; }
        //public ICommand PreviousPageCommand { get; set; }
        //public ICommand FirstPageCommand { get; set; }
        //public ICommand LastPageCommand { get; set; }
        //public ICommand SaveEditedActionCommand { get; set; }


        // wybranie ComboBox

        //public void UpdateCourse(VehicleView vehicle)
        //{
        //    vehicle.Course = (int)UpdatedCourse;
        //    ActualCourse = (int)UpdatedCourse;
        //    UpdatedCourse = null;


        //    var vehicleDB = DatabaseLocator.Database.Vehicles.FirstOrDefault(v => v.Id == _selectedVehicle.Id);
        //    vehicleDB.Course = (int)vehicle.Course;
        //    DatabaseLocator.Database.Vehicles.Update(vehicleDB);

        //    foreach (var item in _selectedVehicle.AlertActions.ToList())
        //    {
        //        item.KmToServis = item.Interval - (vehicle.Course - item.LastActionCourse);
        //        item.progress = UpdateProgress(item);

        //    }

        //    foreach (var item in DatabaseLocator.Database.VehicleActions.Where(x => x.VehicleId == _selectedVehicle.Id).ToList())
        //    {
        //        item.KmToServis = item.Interval - (vehicle.Course - item.LastActionCourse);
        //        item.progress = UpdateProgress(item);
        //        DatabaseLocator.Database.Update(item);
        //    }
        //    DatabaseLocator.Database.SaveChanges();
        //    OnPropertyChanged(nameof(ActualCourse));
        //}

        //public decimal Progress()
        //{
        //    decimal progress = ((_selectedVehicle.Course - (int)NewLastActionCourse) / (decimal)NewInterval * 100);
        //    if (progress > 100)
        //    {
        //        return 100;
        //    }

        //    return progress;
        //}
        //public decimal UpdateProgress(ActionVehicleView alert)
        //{
        //    decimal progress = ((_selectedVehicle.Course - alert.LastActionCourse) / (decimal)alert.Interval * 100);
        //    if (progress > 100)
        //    {
        //        return 100;
        //    }
        //    return progress;
        //}
        //public decimal UpdateProgress(ActionVehicle alert)
        //{
        //    decimal progress = ((_selectedVehicle.Course - alert.LastActionCourse) / (decimal)alert.Interval * 100);
        //    if (progress > 100)
        //    {
        //        return 100;
        //    }
        //    return progress;
        //}


        //public void DeleteVehicle()
        //{
        //    //BaseService.DeleteActions(AlertActions, _selectedVehicle);
        //    DeleteActions();
        //    var vehicle = _selectedVehicle;
        //    VehicleViews.Remove(vehicle);
        //    var vehicleDb = DatabaseLocator.Database.Vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
        //    if (vehicleDb != null)
        //    {
        //        DatabaseLocator.Database.Vehicles.Remove(vehicleDb);
        //    }
        //    DatabaseLocator.Database.SaveChanges();
        //    ActualCourse = null;
        //    OnPropertyChanged(nameof(ActualCourse));
        //}
        //public void DeleteActions()
        //{
        //    var selectedAction = AlertActions.Where(x => x.VehicleViewId == _selectedVehicle.Id).ToList();
        //    foreach (var action in selectedAction)
        //    {
        //        AlertActions.Remove(action);
        //        var selectedActionDb = DatabaseLocator.Database.VehicleActions.FirstOrDefault(x => x.VehicleId == _selectedVehicle.Id);
        //        if (selectedActionDb != null)
        //        {
        //            DatabaseLocator.Database.VehicleActions.Remove(selectedActionDb);
        //        }
        //    }
        //}


        // Aktualizuj przebieg
        //public RelayCommand AddUpdateC => new RelayCommand(faction => UpdateCourse(_selectedVehicle));


        //private void DeleteSelectedAction()
        //{
        //    int currentPages = Pages;
        //    var selectedVehicle = AlertActions.Where(x => x.IsSelected).ToList();
        //    foreach (var task in selectedVehicle)
        //    {
        //        AlertActions.Remove(task);
        //        var foundEntity = DatabaseLocator.Database.VehicleActions.FirstOrDefault(x => x.VehicleId == _selectedVehicle.Id);

        //        if (foundEntity != null)
        //        {
        //            DatabaseLocator.Database.VehicleActions.Remove(foundEntity);
        //        }
        //    }
        //    DatabaseLocator.Database.SaveChanges();
        //    //Paging();
        //    PaginationService.Paging(this);
        //    //ElementsOnPage();
        //    PaginationService.ElementsOnPage(this);


        //    if (Pages < currentPages)
        //    {
        //        //PreviousPage();
        //        PaginationService.PreviousPage(this);
        //    }
        //}
        //public void LoadSelectedAction()
        //{
        //    quantityOfSelected = AlertActions.Where(x => x.IsSelected).ToList().Count();
        //    var action = AlertActions.FirstOrDefault(x => x.IsSelected);

        //    if (quantityOfSelected == 1)
        //    {
        //        NewDescription = action.Description;
        //        NewInterval = action.Interval;
        //        NewLastActionCourse = action.LastActionCourse;
        //    }
        //}
        //public void EditSelectedAction()
        //{
        //    var action = AlertActions.FirstOrDefault(x => x.IsSelected);
        //    action.Description = NewDescription;
        //    action.Interval = (int)NewInterval;
        //    action.LastActionCourse = (int)NewLastActionCourse;
        //    action.KmToServis = (int)NewLastActionCourse + (int)NewInterval - _selectedVehicle.Course;
        //    action.progress = UpdateProgress(action);

        //    ActionVehicle actionDb = DatabaseLocator.Database.VehicleActions.FirstOrDefault(x => x.Id == action.Id);
        //    actionDb.Description = NewDescription;
        //    actionDb.Interval = (int)NewInterval;
        //    actionDb.LastActionCourse = (int)NewLastActionCourse;
        //    actionDb.KmToServis = (int)NewLastActionCourse + (int)NewInterval - _selectedVehicle.Course;
        //    actionDb.progress = UpdateProgress(action);
        //    DatabaseLocator.Database.Entry(actionDb).State = EntityState.Modified;
        //    DatabaseLocator.Database.SaveChanges();
        //    //ElementsOnPage();
        //    PaginationService.ElementsOnPage(this);
        //}

        //public void Paging()
        //{
        //    if (_selectedVehicle != null)
        //    {
        //        int elements = _selectedVehicle.AlertActions.Count();
        //        if (elements % ElementsPerPage != 0)
        //        {
        //            Pages = (int)(elements / ElementsPerPage) + 1;
        //        }
        //        else
        //        {
        //            Pages = (int)(elements / ElementsPerPage);
        //            if (Pages == 0)
        //            {
        //                Pages = 1;
        //            }
        //        }
        //        OnPropertyChanged(nameof(Pages));
        //    }
        //}
        //public ObservableCollection<ActionVehicleView> ElementsOnPage()
        //{
        //    PageAlertActions = new ObservableCollection<ActionVehicleView>(AlertActions
        //                .Skip((CurrentPage - 1) * ElementsPerPage)
        //                .Take(ElementsPerPage));
        //    OnPropertyChanged(nameof(PageAlertActions));

        //    return PageAlertActions;
        //}
        //public void NextPage()
        //{
        //    if (CurrentPage < Pages)
        //    {
        //        CurrentPage++;
        //        ElementsOnPage();
        //        OnPropertyChanged(nameof(CurrentPage));
        //    }
        //}
        //public void PreviousPage()
        //{
        //    if (CurrentPage > 1)
        //    {
        //        CurrentPage--;
        //        ElementsOnPage();
        //        OnPropertyChanged(nameof(CurrentPage));
        //    }
        //}
        //public void FirstPage()
        //{
        //    CurrentPage = 1;
        //    ElementsOnPage();
        //    OnPropertyChanged(nameof(CurrentPage));
        //}
        //public void LastPage()
        //{
        //    CurrentPage = Pages;
        //    ElementsOnPage();
        //    OnPropertyChanged(nameof(CurrentPage));
        //}



        //public void LoadVehicles()
        //{
        //    foreach (var task in DatabaseLocator.Database.Vehicles.ToList())
        //    {
        //        VehicleViews.Add(new VehicleView
        //        {
        //            Id = task.Id,
        //            Name = task.Name,
        //            Course = task.Course,
        //            Plate = task.Plate,
        //        });
        //    }
        //}
        //public void LoadAlerts()
        //{
        //    foreach (var task2 in VehicleViews.ToList())
        //    {
        //        foreach (var task1 in DatabaseLocator.Database.VehicleActions
        //            .Where(c => c.VehicleId == task2.Id)
        //            .Select(c => new { c.Description, c.LastActionCourse, c.Interval, c.KmToServis, c.progress, c.Id })
        //            .ToList())
        //        {
        //            task2.AlertActions.Add(new ActionVehicleView
        //            {
        //                Id = task1.Id,
        //                Description = task1.Description,
        //                LastActionCourse = task1.LastActionCourse,
        //                Interval = task1.Interval,
        //                VehicleViewId = task2.Id,
        //                KmToServis = task1.KmToServis,
        //                progress = task1.progress
        //            });
        //        }
        //    }
        //}


        //public void AddNewVehicle()
        //{
        //    AddVehicleValidation addVehicleVal = new AddVehicleValidation();
        //    validationResult = addVehicleVal.Validate(this);

        //    if (validationResult.IsValid)
        //    {
        //        int? intId = DatabaseLocator.Database.Vehicles.Max(c => (int?)c.Id);
        //        var newVehicle = new VehicleView
        //        {
        //            Id = (int)intId + 1,
        //            Name = NewVehicleName,
        //            Course = (int)NewVehicleCourse,
        //            Plate = NewVehiclePlate,
        //        };

        //        VehicleViews.Add(newVehicle);
        //        AddNewVehicleToDatabase();
        //        RefreshNewVehicle();
        //    }
        //    else
        //    {
        //        if (validationResult.Errors.Where(p => p.ErrorCode == "Name").Count() != 0)
        //        {
        //            ErrorName = validationResult.Errors.Where(p => p.ErrorCode == "Name").Single().ErrorMessage;
        //        }
        //        if (validationResult.Errors.Where(p => p.ErrorCode == "Course").Count() != 0)
        //        {
        //            ErrorCourse = validationResult.Errors.Where(p => p.ErrorCode == "Course").Single().ErrorMessage;
        //        }
        //        if (validationResult.Errors.Where(p => p.ErrorCode == "Plate").Count() != 0)
        //        {
        //            ErrorPlate = validationResult.Errors.Where(p => p.ErrorCode == "Plate").Single().ErrorMessage;
        //        }
        //        RefreshData();
        //    }
        //}
        //public void AddNewVehicleToDatabase()
        //{
        //    DatabaseLocator.Database.Vehicles.Add(new Vehicle
        //    {
        //        Name = NewVehicleName,
        //        Course = (int)NewVehicleCourse,
        //        Plate = NewVehiclePlate,
        //    });
        //    DatabaseLocator.Database.SaveChanges();
        //}

        //public void AddNewActionVehicle()
        //{
        //    AddActionValidation addVehicleVal = new AddActionValidation();
        //    validationResult = addVehicleVal.Validate(this);
        //    if (validationResult.IsValid)
        //    {
        //        var newAction = new ActionVehicleView
        //        {
        //            Description = NewDescription,
        //            LastActionCourse = (int)NewLastActionCourse,
        //            Interval = (int)NewInterval,
        //            KmToServis = (int)NewLastActionCourse + (int)NewInterval - _selectedVehicle.Course,
        //            VehicleViewId = _selectedVehicle.Id,
        //            progress = Progress(),
        //        };
        //        AlertActions.Add(newAction);
        //        AddNewActionVehicleToDatabase();
        //        RefreshNewAction();
        //    }
        //    else
        //    {
        //        if (validationResult.Errors.Where(p => p.ErrorCode == "Name").Count() != 0)
        //        {
        //            ErrorDescription = validationResult.Errors.Where(p => p.ErrorCode == "Name").Single().ErrorMessage;
        //        }
        //        if (validationResult.Errors.Where(p => p.ErrorCode == "LastActionCourse").Count() != 0)
        //        {
        //            ErrorLastActionCourse = validationResult.Errors.Where(p => p.ErrorCode == "LastActionCourse").Single().ErrorMessage;
        //        }
        //        if (validationResult.Errors.Where(p => p.ErrorCode == "Interval").Count() != 0)
        //        {
        //            ErrorInterval = validationResult.Errors.Where(p => p.ErrorCode == "Interval").Single().ErrorMessage;
        //        }
        //        RefreshData();
        //    }

        //    int currentPages = Pages;
        //    //Paging();
        //    PaginationService.Paging(this);
        //    //ElementsOnPage();
        //    PaginationService.ElementsOnPage(this);

        //    if (Pages > currentPages)
        //    {
        //        //NextPage();
        //        PaginationService.NextPage(this);
        //    }
        //}
        //private void AddNewActionVehicleToDatabase()
        //{
        //    DatabaseLocator.Database.VehicleActions.Add(new ActionVehicle
        //    {
        //        VehicleId = _selectedVehicle.Id,
        //        Description = NewDescription,
        //        LastActionCourse = (int)NewLastActionCourse,
        //        Interval = (int)NewInterval,
        //        KmToServis = (int)NewLastActionCourse + (int)NewInterval - _selectedVehicle.Course,
        //        progress = Progress()
        //    });
        //    DatabaseLocator.Database.SaveChanges();
        //}

    }
}

