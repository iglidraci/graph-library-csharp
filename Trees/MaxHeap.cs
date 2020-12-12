using System;
using System.Collections.Generic;

namespace Trees
{
    public class MaxHeap<T> where T: IComparable<T>
    {
        private const int DefaultCapacity = 4;
        private T[] heap;
        public int Count { get; set; }
        public bool IsFull => Count == heap.Length;
        public bool IsEmpty => Count == 0;
        public MaxHeap():this(DefaultCapacity)
        {

        }
        private MaxHeap(int capacity)
        {
            heap = new T[capacity];
        }
        public void Insert(T value)
        {
            if (IsFull)
            {
                var newHeap = new T[heap.Length * 2];
                Array.Copy(heap, 0, newHeap, 0, heap.Length);
                heap = newHeap;                
            }
            heap[Count] = value;
            Swim(Count);
            Count++;
        }
        public IEnumerable<T> Values()
        {
            for(int i=0;i<Count;i++)
            {
                yield return heap[i];
            }
        }

        private void Swim(int index)
        {
            T newValue = heap[index];
            while(index>0 && IsParentLess())
            {
                heap[index] = GetParent(index);
                index = ParentIndex(index);
            }
            heap[index] = newValue;
            bool IsParentLess()
            {
                return newValue.CompareTo(GetParent(index))>0;
            }
        }

        private T GetParent(int index)
        {
            return heap[ParentIndex(index)];
        }

        private int ParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException();
            return heap[0];
        }
        //we remove the root
        public T Remove()
        {
            return Remove(0);
        }
        //remove at a specific index
        public T Remove(int index)
        {
            if (IsEmpty)
                throw new InvalidOperationException();
            T removedValue = heap[index];
            heap[index] = heap[Count - 1];
            if(index==0 || heap[index].CompareTo(GetParent(index)) < 0)
            {
                Sink(index,Count-1);
            }
            else
            {
                Swim(index);
            }
            Count--;
            return removedValue;
        }

        private void Sink(int index, int leastHeapIndex)
        {
            
            while (index<=leastHeapIndex)
            {
                int leftChildIndex = LeftChildIndex(index);
                int rightChildIndex = RightChildIndex(index);
                if (leftChildIndex > leastHeapIndex)
                    break;
                int childIndexToSwap = GetChildIndexToSwap(leftChildIndex, rightChildIndex);
                if (SinkingIsLessThan(childIndexToSwap))
                {
                    Exchange(index, childIndexToSwap);
                }
                else
                {
                    break;
                }

                index = childIndexToSwap;
            }

            int GetChildIndexToSwap(int left,int right)
            {
                int childToSwap;
                if (right > leastHeapIndex)
                {
                    childToSwap = left;
                }
                else
                {
                    int compareTo = heap[left].CompareTo(heap[right]);
                    childToSwap = compareTo > 0 ? left : right;
                }
                return childToSwap;
            }
            bool SinkingIsLessThan(int childToSwap)
            {
                return heap[index].CompareTo(heap[childToSwap]) < 0;
            }
            
        }
        private void Exchange(int leftIndex, int rightIndex)
        {
            T tmp = heap[leftIndex];
            heap[leftIndex] = heap[rightIndex];
            heap[rightIndex] = tmp;
        }

        private int RightChildIndex(int index)
        {
            return 2 * index + 2;
        }

        private int LeftChildIndex(int index)
        {
            return 2 * index + 1;
        }
        public void Sort()
        {
            int lastHeapIndex = Count - 1;
            for (int i = 0; i < lastHeapIndex; i++)
            {
                Exchange(0, lastHeapIndex - i);
                Sink(0, lastHeapIndex - i - 1);
            }
        }
    }
}
