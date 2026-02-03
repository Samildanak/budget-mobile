using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Budget.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var culturequebec = new CultureInfo("fr-CA");
            CultureInfo.DefaultThreadCurrentCulture = culturequebec;
            CultureInfo.DefaultThreadCurrentUICulture = culturequebec;
            Application.Current.UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}