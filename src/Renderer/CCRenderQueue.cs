using System;
using System.Collections.Generic;

namespace CocosSharp
{

    internal class CCRenderQueue<TPriority, TItem>
    {
        readonly SortedList<TPriority, Queue<TItem>> priorityQueues;

        #region Properties

        public bool HasItems
        {
            get { return priorityQueues.Count > 0; }
        }

        public int Count
        {
            get 
            { 
                int count = 0;
                foreach (var subqs in priorityQueues)
                    count += subqs.Value.Count;
                
                return count;
            }
        }

        #endregion Properties


        #region Constructors

        public CCRenderQueue(IComparer<TPriority> priorityComparer)
        {
            priorityQueues = new SortedList<TPriority, Queue<TItem>>(priorityComparer);
        }

        public CCRenderQueue() : this(Comparer<TPriority>.Default) { }

        #endregion Constructors


        public void Enqueue(TPriority priority, TItem item)
        {
            if (!priorityQueues.ContainsKey(priority))
                priorityQueues.Add(priority, new Queue<TItem>());

            priorityQueues[priority].Enqueue(item);
        }

        public TItem Peek()
        {
            if (HasItems)
                return priorityQueues[priorityQueues.Keys[0]].Peek();
            else
                throw new InvalidOperationException("The queue is empty");
        }

        public TItem Dequeue(TPriority priority)
        {
            if (HasItems)
            {
                var queue = priorityQueues[priority];
                TItem nextItem = queue.Dequeue();

                if (queue.Count == 0)
                    priorityQueues.Remove(priority);
                
                return nextItem;
            }
            else
                throw new InvalidOperationException("The queue is empty");
        }

        public TItem Dequeue()
        {
            if (HasItems)
                return Dequeue(priorityQueues.Keys[0]);
            else
                throw new InvalidOperationException("The queue is empty");
        }
    }

}

