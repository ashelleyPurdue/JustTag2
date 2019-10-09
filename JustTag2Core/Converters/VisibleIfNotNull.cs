using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JustTag2.Views
{
    public class VisibleIfNotNull : LambdaConverter<object, Visibility>
    {
        public override Func<object, Visibility> ConvertFunc => obj => (obj == null)
            ? Visibility.Hidden
            : Visibility.Visible;
    }
}
