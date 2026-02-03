using Budget.Mobile.ViewModels;

namespace Budget.Mobile.Views;

public partial class TransactionsPage : ContentPage
{
    public TransactionsPage(TransactionsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void CheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (BindingContext is TransactionsViewModel vm)
        {
            vm.Filtrer();
        }
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (BindingContext is TransactionsViewModel vm)
        {
            vm.Filtrer();
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is TransactionsViewModel vm)
        {
            vm.ChargerDonnees();
        }
    }
}
