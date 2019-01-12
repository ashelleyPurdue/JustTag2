using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JustTag2.Util;
using System.Collections.Specialized;
using System.ComponentModel;

namespace UtilsTests
{
    [TestClass]
    public class ObservableListTests
    {
        private class DummyObservable : INotifyPropertyChanged
        {
            public int value;
            public event PropertyChangedEventHandler PropertyChanged;
        }

        private static bool CheckEventFired<T>(ObservableList<T> list, Action action)
        {
            bool eventFired = false;
            void List_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
                => eventFired = true;

            list.CollectionChanged += List_CollectionChanged;
            action();
            list.CollectionChanged -= List_CollectionChanged;

            return eventFired;
        }

        [TestMethod]
        public void CheckEventFiredWorks()
        {
            var list = new ObservableList<int>();
            bool result = CheckEventFired(list, () =>
            {
                list.Add(0);
            });

            Assert.IsTrue(result);
        }
    }
}
