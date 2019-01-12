using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace JustTag2.Util
{
    /// <summary>
    /// Like ObservableCollection, except it also notifies if one of its elements
    /// has a property changed event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableList<T> : ObservableCollection<T>
    {
        public ObservableList()
        {
            base.CollectionChanged += ObservableList_CollectionChanged;
        }

        private void ObservableList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (T item in e.OldItems)
            {
                if (item is INotifyPropertyChanged i)
                    i.PropertyChanged -= Item_PropertyChanged;
            }

            foreach (T item in e.NewItems)
            {
                if (item is INotifyPropertyChanged i)
                    i.PropertyChanged += Item_PropertyChanged;
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender);
            this.OnCollectionChanged(args);
        }
    }
}
