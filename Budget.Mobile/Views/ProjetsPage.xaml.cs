using Budget.Mobile.ViewModels;

namespace Budget.Mobile.Views;

public partial class ProjetsPage : ContentPage
{
    private readonly ProjetsViewModel _viewModel;

    public ProjetsPage(ProjetsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Force le rechargement dès que l'écran redevient visible
        await _viewModel.ChargerProjetsAsync();
    }
}