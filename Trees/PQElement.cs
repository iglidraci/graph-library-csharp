using System;

namespace Trees
{
    public class PQElement<T,U> where T: IComparable<T> where U : IComparable<U>
    {
        public T Element { get; set; }
        public U Priority { get; set; }
        public PQElement()
        {

        }
        public PQElement(T element,U priority)
        {
            Element = element;
            Priority = priority;
        }

    }
}
