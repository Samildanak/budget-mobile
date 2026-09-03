using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Budget.Mobile.Helpers;
using Budget.Mobile.Models;
using Budget.Mobile.Services;
using Microsoft.Maui.Controls;

namespace Budget.Mobile.ViewModels;

public class CreateProjetViewModel : BindableObject
{
    private readonly DepenseService _depenseService;
    private string _nom = string.Empty;
    private bool _isBusy;
    private string _messageErreur = string.Empty;

    public string Nom
    {
        get => _nom;
        set
        {
            _nom = value;
            OnPropertyChanged();
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    public string MessageErreur
    {
        get => _messageErreur;
        set
        {
            _messageErreur = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<SelectableCategory> CategoriesEntrees { get; } = new();
    public ObservableCollection<SelectableCategory> CategoriesSorties { get; } = new();

    public ICommand SauvegarderCommand { get; }
    public ICommand ToggleCategoryCommand { get; }

    public CreateProjetViewModel(DepenseService depenseService)
    {
        _depenseService = depenseService;

        SauvegarderCommand = new Command(async () => await SauvegarderAsync());
        ToggleCategoryCommand = new Command<SelectableCategory>(cat =>
        {
            if (cat != null) cat.IsSelected = !cat.IsSelected;
        });

        InitialiserCategories();
    }

    private void InitialiserCategories()
    {
        CategoriesEntrees.Clear();
        CategoriesSorties.Clear();

        foreach (TypeCategorie cat in Enum.GetValues(typeof(TypeCategorie)))
        {
            int id = (int)cat;
            string nomAffiche = CategoryHelper.GetNomAffichage(cat);

            // Entrée : EstEntree = true (nuances vertes)
            CategoriesEntrees.Add(new SelectableCategory
            {
                Id = id,
                Nom = nomAffiche,
                EstEntree = true
            });

            // Sortie : EstEntree = false (nuances rouges)
            CategoriesSorties.Add(new SelectableCategory
            {
                Id = id,
                Nom = nomAffiche,
                EstEntree = false
            });
        }
    }

    private async Task SauvegarderAsync()
    {
        if (string.IsNullOrWhiteSpace(Nom))
        {
            MessageErreur = "Veuillez entrer un nom pour le projet.";
            return;
        }

        // Récupération des IDs cochés
        var entrees = CategoriesEntrees.Where(c => c.IsSelected).Select(c => c.Id).ToList();
        var sorties = CategoriesSorties.Where(c => c.IsSelected).Select(c => c.Id).ToList();

        if (entrees.Count == 0 && sorties.Count == 0)
        {
            MessageErreur = "Sélectionnez au moins une catégorie (entrée ou sortie).";
            return;
        }

        MessageErreur = string.Empty;
        IsBusy = true;

        try
        {
            var dto = new CreateProjetDto
            {
                Nom = Nom.Trim(),
                CategoriesEntrees = entrees,
                CategoriesSorties = sorties
            };

            var idCree = await _depenseService.CreateProjetAsync(dto);

            if (idCree.HasValue)
            {
                // Revient sur la page précédente (ProjetsPage)
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                MessageErreur = "Erreur lors de la création sur le serveur.";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Erreur CreateProjet: {ex.Message}");
            MessageErreur = "Impossible de contacter l'API.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}