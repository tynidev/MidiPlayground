using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keys
{
    public class CircularList<T> : IList<T>
    {
        public List<T> list = new List<T>();
        protected int position = 0;
        public int Position
        {
            get
            {
                return position;
            }
        }

        public virtual T this[int index]
        {
            get
            {
                return GetItem(index);
            }
            set
            {
                list[GetIndex(index)] = value;
            }
        }

        public T Rotate(int index)
        {
            position = GetIndex(index);
            return list[position];
        }

        public T GetItem(int index)
        {
            return list[GetIndex(index)];
        }

        public int GetIndex(int index)
        {
            if(index < 0)
                index = list.Count() - ((-1 * index) % list.Count());
            return (position + index) % list.Count();
        }

        public int Count => list.Count();

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(GetIndex(index), item);
        }

        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(GetIndex(index));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
