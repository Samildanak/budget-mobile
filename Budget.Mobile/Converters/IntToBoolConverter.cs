using System.Globalization;

namespace Budget.Mobile.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // On vérifie si la valeur reçue est un entier (int)
            if (value is int count)
            {
                // Renvoie Vrai si le nombre est supérieur à 0
                return count > 0;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}