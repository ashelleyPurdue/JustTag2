using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace JustTag2.Views
{
    public class BoolToVisibilityCollapsed : LambdaConverter<bool, Visibility>
    {
        public override Func<bool, Visibility> ConvertFunc => value => value
            ? Visibility.Visible
            : Visibility.Collapsed;
    }
}
