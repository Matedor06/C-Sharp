﻿using ErrorOr;

namespace Solution.DesktopApp.ViewModels;

[ObservableObject]
public partial class CreateOrEditMotorcycleViewModel(
    AppDbContext dbContext,
    IMotorcycleService motorcycleService,
    IGoogleDriveService googleDriveService) : MotorcycleModel(), IQueryAttributable
{
    #region life cycle commands
    public IAsyncRelayCommand AppearingCommand => new AsyncRelayCommand(OnAppearingkAsync);
    public IAsyncRelayCommand DisappearingCommand => new AsyncRelayCommand(OnDisappearingAsync);
    #endregion

    #region validation commands
    public IRelayCommand ManufacturerIndexChangedCommand => new RelayCommand(() => this.Manufacturer.Validate());

    public IRelayCommand CoolIndexChangedCommand => new RelayCommand(() => this.Cool.Validate());

    public IRelayCommand CylindersIndexChangedCommand => new RelayCommand(() => this.NumberOfCylinders.Validate());
    public IRelayCommand ModelValidationCommand => new RelayCommand(() => this.Model.Validate());
    public IRelayCommand CubicValidationCommand => new RelayCommand(() => this.Cubic.Validate());
    public IRelayCommand ReleaseYearValidationCommand => new RelayCommand(() => this.ReleaseYear.Validate());
    #endregion

    private delegate Task ButtonActionDelagate();
    private ButtonActionDelagate asyncButtonAction;

    [ObservableProperty]
    private string title;

    public IAsyncRelayCommand SubmitCommand => new AsyncRelayCommand(OnSubmitAsync);

    [ObservableProperty]
    private IList<ManufacturerModel> manufacturers = [];

    [ObservableProperty]
    private IList<CoolModel> cools = [];

    [ObservableProperty]
    private IList<uint> cylinders = [1, 2, 3, 4, 6, 8];

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        await Task.Run(() => LoadManufacturersAsync());
        await Task.Run(() => LoadCoolsAsync());

        bool hasValue = query.TryGetValue("Motorcycle", out object result);

        if(!hasValue)
        {
            asyncButtonAction = OnSaveAsync;
            Title = "Add new  motorcycle";
            return;
        }

        MotorcycleModel motorcycle = result as MotorcycleModel;

        this.Id = motorcycle.Id;
        this.Manufacturer.Value = motorcycle.Manufacturer.Value;
        this.Cool.Value = motorcycle.Cool.Value;
        this.Model.Value = motorcycle.Model.Value;
        this.ReleaseYear.Value = motorcycle.ReleaseYear.Value;
        this.Cubic.Value = motorcycle.Cubic.Value;
        this.NumberOfCylinders.Value = motorcycle.NumberOfCylinders.Value;

        asyncButtonAction = OnUpdateAsync;
        Title = "Update motorcycle";
    }

    private async Task OnAppearingkAsync()
    {
    }

    private async Task OnDisappearingAsync()
    { }

    private async Task OnSubmitAsync() => await asyncButtonAction();

    private async Task OnSaveAsync()
    {
        if (!IsFormValid())
        {
            return;
        }

        var result = await motorcycleService.CreateAsync(this);

        var message = result.IsError ? result.FirstError.Description : "Motorcycle saved.";
        var title = result.IsError ? "Error" : "Information";

        if (!result.IsError)
        {
            ClearForm();
        }

        await Application.Current.MainPage.DisplayAlert(title, message, "OK");
    }

    private async Task OnUpdateAsync()
    {
        if (!IsFormValid())
        {
            return;
        }

        var result = await motorcycleService.UpdateAsync(this);

        var message = result.IsError ? result.FirstError.Description : "Motorcycle updated.";
        var title = result.IsError ? "Error" : "Information";

        await Application.Current.MainPage.DisplayAlert(title, message, "OK");
    }

    private async Task LoadManufacturersAsync()
    {
        Manufacturers = await dbContext.Manufacturers.AsNoTracking()
                                                     .OrderBy(x => x.Name)
                                                     .Select(x => new ManufacturerModel(x))
                                                     .ToListAsync();
    }

    private async Task LoadCoolsAsync()
    {
        Cools = await dbContext.Cools.AsNoTracking()
                                   .OrderBy(x => x.Name)
                                   .Select(x => new CoolModel(x))
                                   .ToListAsync();
    }

    private void ClearForm()
    {
        this.Manufacturer.Value = null;
        this.Cool.Value = null;
        this.Model.Value = null;
        this.Cubic.Value = null;
        this.ReleaseYear.Value = null;
        this.NumberOfCylinders.Value = null;
    }

    private bool IsFormValid()
    {
        this.Manufacturer.Validate();
        this.Cool.Validate();
        this.Model.Validate();
        this.Cubic.Validate();
        this.ReleaseYear.Validate();
        this.NumberOfCylinders.Validate();


        return (this.Manufacturer?.IsValid ?? false) &&
               (this.Cool?.IsValid ?? false) &&
               this.Model.IsValid &&
               this.Cubic.IsValid &&
               this.ReleaseYear.IsValid &&
               this.NumberOfCylinders.IsValid;
    }
}
