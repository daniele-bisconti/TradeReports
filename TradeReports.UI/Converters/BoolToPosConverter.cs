using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TradeReports.Core.Models;

namespace TradeReports.UI.Converters
{
    public class BoolToPosConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is PosType)
                return value != null && ((PosType)value).Equals((PosType)parameter);
            else if (parameter is string)
                return value != null && (string)parameter == ((PosType)value).ToString();

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PosType pos = Enum.Parse<PosType>((string)parameter);
            return (bool)value ? pos : PosType.Short;
        }
    }
}
