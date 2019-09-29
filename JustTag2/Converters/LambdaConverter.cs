using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace JustTag2.Converters
{
    public abstract class LambdaConverter<TIn, TOut> : IValueConverter
    {
        public abstract Func<TIn, TOut> ConvertFunc { get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ConvertFunc((TIn)value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
