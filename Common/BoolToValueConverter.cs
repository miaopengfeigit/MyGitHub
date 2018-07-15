using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Common
{
    public class BoolToValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NLog.Info("","Convert{0}", value.ToString());
            if ((bool)value)
                return parameter;
            else
                return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NLog.Info("","ConvertBack{0}", value.ToString());
            return object.Equals(value, parameter);
        }

        #endregion
    }
}
