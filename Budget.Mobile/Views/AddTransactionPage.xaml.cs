using Budget.Mobile.ViewModels;

namespace Budget.Mobile.Views;

public partial class AddTransactionPage : ContentPage
{
	public AddTransactionPage(AddTransactionViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}