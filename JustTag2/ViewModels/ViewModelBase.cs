using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JustTag2
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetAndRaise<T>(ref T backingField, T value, params string[] properties)
        {
            backingField = value;

            foreach (string p in properties)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }
    }
}
