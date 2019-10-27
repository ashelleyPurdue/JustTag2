using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JustTag2
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Dictionary<string, HashSet<Action>> propertyChangedHandlers = new Dictionary<string, HashSet<Action>>();

        public ViewModelBase()
        {
            // Invoke all registered property changed handlers.
            PropertyChanged += (s, e) =>
            {
                var handlers = GetOrCreateHandlers(e.PropertyName);

                foreach (var handler in handlers)
                    handler();
            };
        }

        /// <summary>
        /// Invokes the given handler when the property with the given name changes.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="handler"></param>
        public void OnPropertyChanged(string propertyName, Action handler)
        {
            GetOrCreateHandlers(propertyName).Add(handler);
        }

        /// <summary>
        /// Unsubscribes the given handler from property changed notifications.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="handler"></param>
        public void UnregisterPropertyChanged(string propertyName, Action handler)
        {
            GetOrCreateHandlers(propertyName).Remove(handler);
        }

        private HashSet<Action> GetOrCreateHandlers(string propertyName)
        {
            if (!propertyChangedHandlers.ContainsKey(propertyName))
                propertyChangedHandlers.Add(propertyName, new HashSet<Action>());

            return propertyChangedHandlers[propertyName];
        }

        protected void Raise(params string[] properties)
        {
            foreach (string p in properties)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

        protected void SetAndRaise<T>(ref T backingField, T value, params string[] properties)
        {
            backingField = value;
            Raise(properties);
        }
    }
}
