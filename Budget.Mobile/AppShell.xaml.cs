using Budget.Mobile.Views;

namespace Budget.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditBudgetPage), typeof(EditBudgetPage));
            Routing.RegisterRoute(nameof(CreateProjetPage), typeof(CreateProjetPage));
            Routing.RegisterRoute(nameof(ProjetDetailPage), typeof(ProjetDetailPage));
        }
    }
}
