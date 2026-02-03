using System.Globalization;
using Budget.Mobile.Helpers; // Pour accéder au Helper
using Budget.Mobile.Models;

namespace Budget.Mobile.Converters
{
    public class CategoryNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // On vérifie si la valeur reçue est bien une catégorie
            if (value is TypeCategorie cat)
            {
                return CategoryHelper.GetNomAffichage(cat);
            }

            return "Inconnu";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}