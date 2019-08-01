using System;
using System.Collections;
using System.Collections.Generic;

namespace Infra.Wpf.Controls
{
    public class FieldCollection : IList<IField>, IList
    {
        private IList<IField> fields;

        public FieldCollection()
        {
            fields = new List<IField>();
        }

        public FieldCollection(IEnumerable<IField> collection)
        {
            fields = new List<IField>(collection);
        }

        public FieldCollection(int capacity)
        {
            fields = new List<IField>(capacity);
        }

        #region IList<IField>

        public IField this[int index]
        {
            get => fields[index];
            set => fields[index] = value;
        }

        public int Count => fields.Count;

        public bool IsReadOnly => fields.IsReadOnly;

        public void Add(IField item) => fields.Add(item);

        public void Clear() => fields.Clear();

        public bool Contains(IField item) => fields.Contains(item);

        public void CopyTo(IField[] array, int arrayIndex) => fields.CopyTo(array, arrayIndex);

        public IEnumerator<IField> GetEnumerator() => fields.GetEnumerator();

        public int IndexOf(IField item) => fields.IndexOf(item);

        public void Insert(int index, IField item) => fields.Insert(index, item);

        public bool Remove(IField item) => fields.Remove(item);

        public void RemoveAt(int index) => fields.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => fields.GetEnumerator();

        #endregion

        #region IList

        object IList.this[int index]
        {
            get => fields[index];
            set => ((IList)fields).Add(value);
        }

        public bool IsFixedSize => ((IList)fields).IsFixedSize;

        public object SyncRoot => ((IList)fields).SyncRoot;

        public bool IsSynchronized => ((IList)fields).IsSynchronized;

        public int Add(object value) => ((IList)fields).Add(value);

        public bool Contains(object value) => ((IList)fields).Contains(value);

        public void CopyTo(Array array, int index) => ((IList)fields).CopyTo(array, index);

        public int IndexOf(object value) => ((IList)fields).IndexOf(value);

        public void Insert(int index, object value) => ((IList)fields).Insert(index, value);

        public void Remove(object value) => ((IList)fields).Remove(value);

        #endregion
    }
}
