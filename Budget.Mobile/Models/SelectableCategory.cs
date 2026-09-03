using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Windows.Input;

namespace Budget.Mobile.Models;

public class SelectableCategory : BindableObject
{
    private bool _isSelected;

    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public bool EstEntree { get; set; } // Pour adapter la couleur (Vert vs Rouge)

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CouleurFond));
                OnPropertyChanged(nameof(CouleurBordure));
                OnPropertyChanged(nameof(CouleurTexte));
            }
        }
    }

    // Propriétés visuelles calculées
    public Color CouleurFond => IsSelected
        ? (EstEntree ? Color.FromArgb("#E8F8F5") : Color.FromArgb("#FDEDEC"))
        : Color.FromArgb("#F8F9FA");

    public Color CouleurBordure => IsSelected
        ? (EstEntree ? Color.FromArgb("#27AE60") : Color.FromArgb("#E74C3C"))
        : Color.FromArgb("#E2E8F0");

    public Color CouleurTexte => IsSelected
        ? (EstEntree ? Color.FromArgb("#27AE60") : Color.FromArgb("#E74C3C"))
        : Color.FromArgb("#2C3E50");

    // Commande directe liée à chaque puce
    public ICommand ToggleCommand { get; }

    public SelectableCategory()
    {
        ToggleCommand = new Command(() => IsSelected = !IsSelected);
    }
}