using Budget.Mobile.ViewModels;

namespace Budget.Mobile.Views;

public partial class CreateProjetPage : ContentPage
{
    public CreateProjetPage(CreateProjetViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}