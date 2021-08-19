using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TradeReports.UI.Converters
{
    public class FirstTenCharactersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = (string)value;
            if (input == null) return "";
            int len = input.Length < 10 ? input.Length : 10;
            return string.IsNullOrEmpty(input) ? "" : (input).Substring(0, len) + "...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
