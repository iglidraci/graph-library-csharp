using System;
using System.Collections.Generic;
using System.Linq;

namespace Trees
{
    public class PriorityQueue<T,U> where T : IComparable<T> where U : IComparable<U>
    {
        private readonly List<PQElement<T, U>> items;

        public bool IsEmpty => items.Count == 0;

        public PriorityQueue()
        {
            items = new List<PQElement<T, U>>();
        }
        public void Enqueue(T element,U priority)
        {
            var pqElement = new PQElement<T, U>(element, priority);
            bool contains = false;
            // iterating through the entire 
            // item array to add element at the 
            // correct location of the Queue            
            for (int i = 0; i < items.Count; i++)
            {
                int compare = items[i].Priority.CompareTo(pqElement.Priority);
                if (compare > 0)
                {
                    items.Insert(i, pqElement);
                    contains = true;
                    break;
                }
            }
            // if the element have the highest priority 
            // it is added at the end of the queue
            if (!contains)
                items.Add(pqElement);
        }
        public PQElement<T,U> Dequeue()
        {
            if (IsEmpty)
                return default;
            var elementToRemove = items[0];
            items.RemoveAt(0);
            return elementToRemove;
        }
        public T Front()
        {
            if (IsEmpty)
                return default;
            return items[0].Element;
        }
        public T Rear()
        {
            if (IsEmpty)
                return default;
            return items[items.Count-1].Element;
        }
        public List<T> GetAllElementsSortedDecreasingly()
        {
            var list = new List<T>();
            for (int i = items.Count - 1; i >= 0; i--)
            {
                list.Add(items[i].Element);
            }
            return list;
        }
        public string GenerateString()
        {
            string str = "";
            for (int i = items.Count - 1; i >= 0; i--)
            {
                
                str += items[i].Element;
            }
            return str;
        }
        public List<PQElement<T, U>> GetPQ()
        {
            return items;
        }
        public Queue<PQElement<T,U>> GetQueue()
        {
            var queue = new Queue<PQElement<T, U>>();
            foreach (var item in items)
            {
                queue.Enqueue(item);
            }
            return queue;
        }
    }
}
