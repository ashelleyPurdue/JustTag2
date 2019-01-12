using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTag2.Util
{
    public class ObservableList<T> : ObservableCollection<T>, IList<T>
    {
    }
}
