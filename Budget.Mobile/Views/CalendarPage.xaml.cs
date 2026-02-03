using Budget.Mobile.ViewModels;

namespace Budget.Mobile.Views;

public partial class CalendarPage : ContentPage
{
	public CalendarPage(CalendarViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CalendarViewModel vm)
        {
            await vm.ChargerDonneesCommand.ExecuteAsync(null);
        }
    }
}