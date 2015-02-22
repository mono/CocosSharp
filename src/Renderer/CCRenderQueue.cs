using System;
using System.Collections.Generic;

namespace CocosSharp
{

    internal class RenderQueue<TPriority, TItem>
    {
        readonly SortedList<TPriority, Queue<TItem>> subQueues;

        public RenderQueue(IComparer<TPriority> priorityComparer)
        {
            subQueues = new SortedList<TPriority, Queue<TItem>>(priorityComparer);
        }

        public RenderQueue() : this(Comparer<TPriority>.Default) { }

        public void Enqueue(TPriority priority, TItem item)
        {
            if (!subQueues.ContainsKey(priority))
            {
                AddQueueOfPriority(priority);
            }

            subQueues[priority].Enqueue(item);
        }

        private void AddQueueOfPriority(TPriority priority)
        {
            subQueues.Add(priority, new Queue<TItem>());
        }

        public TItem Peek()
        {
            if (HasItems)
            {
                return subQueues[subQueues.Keys[0]].Peek();
            }
            else
                throw new InvalidOperationException("The queue is empty");
        }

        public bool HasItems
        {
            get {

                return subQueues.Count > 0;
            }
        }


        public TItem Dequeue(TPriority priority)
        {

            if (HasItems)
                return DequeueFromPriorityQueue();
            else
                throw new InvalidOperationException("The queue is empty");
        }

        public TItem Dequeue()
        {
            if (HasItems)
                return DequeueFromPriorityQueue();
            else
                throw new InvalidOperationException("The queue is empty");
        }

        private TItem DequeueFromPriorityQueue()
        {

            var key = subQueues.Keys[0];
            var first = subQueues[key]; 

            TItem nextItem = first.Dequeue();

            // if our subqueu is empty then we need to remove it
            if (first.Count == 0)
            {
                subQueues.Remove(key);
            }
            return nextItem;
        }

        public int Count
        {

            get { 
                int count = 0;
                foreach (var subqs in subQueues)
                {
                    count += subqs.Value.Count;
                }
                return count;
            }
        }
    }

}

