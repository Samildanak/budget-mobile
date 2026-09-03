using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Budget.Mobile.Models;
using Budget.Mobile.Services;

namespace Budget.Mobile.ViewModels;

public class ProjetsViewModel : BindableObject
{
    private readonly DepenseService _depenseService;
    private bool _isBusy;

    public ObservableCollection<ProjetItem> Items { get; } = new();

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    public ICommand ChargerProjetsCommand { get; }
    public ICommand ItemCliqueCommand { get; }

    public ProjetsViewModel(DepenseService depenseService)
    {
        _depenseService = depenseService;

        ChargerProjetsCommand = new Command(async () => await ChargerProjetsAsync());
        ItemCliqueCommand = new Command<ProjetItem>(async (item) => await GererClicAsync(item));
    }

    public async Task ChargerProjetsAsync()
    {
        try
        {
            IsBusy = true;
            var liste = await _depenseService.GetProjetsAsync();

            // On s'assure d'opérer sur le thread UI principal pour MAUI
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Items.Clear();
                foreach (var p in liste)
                {
                    Items.Add(ProjetItem.DepuisProjet(p));
                }
                // Toujours replacer le bouton d'ajout à la fin
                Items.Add(ProjetItem.CreerBoutonAjout());
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur lors du rafraîchissement des projets : {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task GererClicAsync(ProjetItem? item)
    {
        if (item == null) return;

        if (item.EstBoutonAjout)
        {
            await Shell.Current.GoToAsync("CreateProjetPage");
        }
        else
        {
            await Shell.Current.GoToAsync($"ProjetDetailPage?id={item.Id}");
        }
    }
}