using Budget.Mobile.ViewModels;

namespace Budget.Mobile.Views;

public partial class ProjetDetailPage : ContentPage
{
    private readonly ProjetDetailViewModel _viewModel;

    public ProjetDetailPage(ProjetDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.ChargerDetailsAsync();
    }
}