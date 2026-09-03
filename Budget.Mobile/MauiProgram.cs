using Budget.Mobile.Services;
using Budget.Mobile.ViewModels;
using Budget.Mobile.Views;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;

namespace Budget.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMicrocharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<Budget.Mobile.Services.DepenseService>();

            builder.Services.AddTransient<Budget.Mobile.ViewModels.DepensesViewModel>();

            // 1. Dashboard (Accueil)
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<DashboardViewModel>();

            // 2. Transactions (Historique)
            builder.Services.AddTransient<TransactionsPage>();
            builder.Services.AddTransient<TransactionsViewModel>();

            // 3. Budgets
            builder.Services.AddTransient<BudgetPage>();
            builder.Services.AddTransient<BudgetViewModel>();

            builder.Services.AddTransient<EditBudgetPage>();
            builder.Services.AddTransient<EditBudgetViewModel>();

            // 4. Calendrier
            builder.Services.AddTransient<CalendarPage>();
            builder.Services.AddTransient<CalendarViewModel>();

            builder.Services.AddTransient<AddTransactionPage>();
            builder.Services.AddTransient<AddTransactionViewModel>();

            // 5. Projets
            builder.Services.AddTransient<ProjetsViewModel>();
            builder.Services.AddTransient<ProjetsPage>();

            // 6. Créer un projet
            builder.Services.AddTransient<CreateProjetViewModel>();
            builder.Services.AddTransient<CreateProjetPage>();

            // 7. Détails d'un projet
            builder.Services.AddTransient<ProjetDetailViewModel>();
            builder.Services.AddTransient<ProjetDetailPage>();

            builder.Services.AddTransient<MainPage>();

            return builder.Build();
        }
    }
}
