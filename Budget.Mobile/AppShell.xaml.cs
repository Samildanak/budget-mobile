using Budget.Mobile.Views;

namespace Budget.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EditBudgetPage), typeof(EditBudgetPage));
        }
    }
}
