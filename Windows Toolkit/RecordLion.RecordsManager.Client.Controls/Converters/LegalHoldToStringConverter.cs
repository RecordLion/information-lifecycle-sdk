using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace RecordLion.RecordsManager.Client.Controls.Converters
{
    public class LegalHoldToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            LegalHold hold = value as LegalHold;

            if (hold != null)
            {
                string returnValue = hold.LegalCaseTitle;

                if (!string.IsNullOrEmpty(hold.LegalCaseNumber))
                    returnValue += string.Format(" ({0})", hold.LegalCaseNumber);

                return returnValue;
            }

            return DependencyProperty.UnsetValue;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}