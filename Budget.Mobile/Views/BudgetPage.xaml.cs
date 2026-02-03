using Budget.Mobile.ViewModels;

namespace Budget.Mobile.Views;

public partial class BudgetPage : ContentPage
{
	public BudgetPage(BudgetViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is BudgetViewModel vm)
        {
            await vm.ChargerBudgetCommand.ExecuteAsync(null);
        }
    }
}