using Budget.Mobile.ViewModels;

namespace Budget.Mobile.Views;

public partial class EditBudgetPage : ContentPage
{
	public EditBudgetPage(EditBudgetViewModel vm)
    {
		InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Si le contexte est bien notre ViewModel, on lance la commande ChargerDonnees
        if (BindingContext is EditBudgetViewModel vm)
        {
            await vm.ChargerDonneesCommand.ExecuteAsync(null);
        }
    }

    private void Entry_Completed(object sender, EventArgs e)
    {
        if (BindingContext is EditBudgetViewModel vm)
        {
            vm.RecalculerTotaux();
        }
    }
}