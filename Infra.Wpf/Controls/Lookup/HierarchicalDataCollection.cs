using System;
using System.Collections;
using System.Collections.Generic;

namespace Infra.Wpf.Controls
{
    public class HierarchicalDataCollection : IList<HierarchicalData>, IList
    {
        private IList<HierarchicalData> items;

        public HierarchicalDataCollection()
        {
            items = new List<HierarchicalData>();
        }

        public HierarchicalDataCollection(IEnumerable<HierarchicalData> collection)
        {
            items = new List<HierarchicalData>(collection);
        }

        public HierarchicalDataCollection(int capacity)
        {
            items = new List<HierarchicalData>(capacity);
        }

        #region IList<HierarchicalData>

        public HierarchicalData this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }

        public int Count => items.Count;

        public bool IsReadOnly => items.IsReadOnly;

        public void Add(HierarchicalData item) => items.Add(item);

        public void Clear() => items.Clear();

        public bool Contains(HierarchicalData item) => items.Contains(item);

        public void CopyTo(HierarchicalData[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        public IEnumerator<HierarchicalData> GetEnumerator() => items.GetEnumerator();

        public int IndexOf(HierarchicalData item) => items.IndexOf(item);

        public void Insert(int index, HierarchicalData item) => items.Insert(index, item);

        public bool Remove(HierarchicalData item) => items.Remove(item);

        public void RemoveAt(int index) => items.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

        #endregion

        #region IList

        object IList.this[int index]
        {
            get => items[index];
            set => ((IList)items).Add(value);
        }

        public bool IsFixedSize => ((IList)items).IsFixedSize;

        public object SyncRoot => ((IList)items).SyncRoot;

        public bool IsSynchronized => ((IList)items).IsSynchronized;

        public int Add(object value) => ((IList)items).Add(value);

        public bool Contains(object value) => ((IList)items).Contains(value);

        public void CopyTo(Array array, int index) => ((IList)items).CopyTo(array, index);

        public int IndexOf(object value) => ((IList)items).IndexOf(value);

        public void Insert(int index, object value) => ((IList)items).Insert(index, value);

        public void Remove(object value) => ((IList)items).Remove(value);

        #endregion
    }
}
