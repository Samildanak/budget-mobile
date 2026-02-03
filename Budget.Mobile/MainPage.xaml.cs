using Budget.Mobile.ViewModels;

namespace Budget.Mobile
{
    public partial class MainPage : ContentPage
    {
        // On demande le ViewModel au constructeur
        public MainPage(DepensesViewModel vm)
        {
            InitializeComponent();

            // On dit à la page : "Tes données viennent de ce ViewModel"
            BindingContext = vm;
        }
    }
}