using System.Globalization;

namespace Budget.Mobile.Converters
{
    // 1. Converter pour la COULEUR (Vert si Revenu, Rouge si Dépense)
    public class AmountColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // value est le booléen "Est_Revenu"
            if (value is bool estRevenu && estRevenu)
                return Colors.Green; // Revenu = Vert

            return Colors.Red; // Dépense = Rouge
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    // 2. Converter pour le TEXTE (Ajoute un "+" ou un "-")
    public class AmountTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // On attend ici l'objet "Depense" entier ou juste le montant + le booléen
            // Pour faire simple, passons le Montant, et utilisons un paramètre pour le signe, 
            // MAIS le plus simple en XAML est souvent de faire deux labels ou de gérer ça dans le VM.
            // ICI, on va faire une astuce XAML plus bas. Gardons juste le ColorConverter qui est essentiel.
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}