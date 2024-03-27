using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jamieleneveCOIS3020Assignment3
{
    public class BinomialNode<T>
    {
        public T Item { get; set; }
        public int Degree { get; set; }
        public BinomialNode<T> LeftMostChild { get; set; }
        public BinomialNode<T> RightSibling { get; set; }

        // Constructor

        public BinomialNode(T item)
        {
            Item = item;
            Degree = 0;
            LeftMostChild = null;
            RightSibling = null;
        }
    }

    //--------------------------------------------------------------------------------------

    // Common interface for all non-linear data structures

    public interface IContainer<T>
    {
        void MakeEmpty();  // Reset an instance to empty
        bool Empty();      // Test if an instance is empty
        int Size();        // Return the number of items in an instance
    }

    //--------------------------------------------------------------------------------------

    public interface IBinomialHeap<T> : IContainer<T> where T : IComparable
    {
        void Add(T item);               // Add an item to a binomial heap
        void Remove();                  // Remove the item with the highest priority
        T Front();                      // Return the item with the highest priority
    }

    //--------------------------------------------------------------------------------------

    // Binomial Heap
    // Implementation:  Leftmost-child, right-sibling

    public class BinomialHeap<T> : IBinomialHeap<T> where T : IComparable
    {
        private BinomialNode<T> head;  // Head of the root list
        private int size;              // Size of the binomial heap
        private BinomialNode<T>[] heap;
        private BinomialNode<T> highestPriority;
        // Contructor
        // Time complexity:  O(1)

        public BinomialHeap()
        {
            heap = new BinomialNode<T>[15];
            for (int i = 0; i < heap.Length; i++)
                heap[i] = new BinomialNode<T>(default(T));

            head = new BinomialNode<T>(default(T));   // Header node
            size = 0;
        }

        // Add
        // Inserts an item into the binomial heap
        // Time complexity:  O(log n)

        public void Add(T item)
        {
            BinomialNode<T> H = new BinomialNode<T>(item);

            H.RightSibling = heap[0].RightSibling;
            heap[0].RightSibling = H;
            if (highestPriority == null)
                highestPriority = heap[0].RightSibling;
            if (highestPriority != null && highestPriority.Item.CompareTo(heap[0].RightSibling.Item) < 0)
                highestPriority = heap[0].RightSibling;
            size++;
        }

        // Remove
        // Removes the item with the highest priority from the binomial heap
        // Time complexity:  O(log n)

        public void Remove()
        {
            /*
            if (!Empty())
            {
                BinomialHeap<T> H = new BinomialHeap<T>();
                BinomialNode<T> p, q;

                // Get the reference to the preceding node with the highest priority
                q = FindHighest();

                // Remove binomial tree p from root list
                p = q.RightSibling;
                q.RightSibling = q.RightSibling.RightSibling;

                // Add binomial subtrees of p in reverse order into H
                p = p.LeftMostChild;
                while (p != null)
                {
                    q = p.RightSibling;

                    // Splice p into H as the first binomial tree
                    p.RightSibling = H.head.RightSibling;
                    H.head.RightSibling = p;

                    p = q;
                }
                size--;
                Merge(H);
            }*/
        }

        // Degrees
        // Prints the degree for each binomial tree in the root list
        // Time complexity:  O(log n)

        public void Degrees()
        {
            BinomialNode<T> p = head.RightSibling;

            while (p != null)
            {
                Console.WriteLine(p.Degree);
                p = p.RightSibling;
            }
        }

        // Front
        // Returns the item with the highest priority
        // Time complexity:  O(log n)

        public T Front()
        {
            if (!Empty())
            {
                return highestPriority.Item;
            }
            else
                return default(T);
        }

        // BinomialLink
        // Makes child the leftmost child of root
        // Time complexity:  O(1)

        private void BinomialLink(BinomialNode<T> child, BinomialNode<T> root)
        {
            child.RightSibling = root.LeftMostChild;
            root.LeftMostChild = child;
            root.Degree++;
        }

        public void print()
        {
            for (int i = 0; i < heap.Length; i++)
            {
                if (heap[i].RightSibling == null)
                {
                    Console.WriteLine("2^{0} : Empty", i);
                    Console.WriteLine();
                    continue;  // skip the iteration
                }

                // else call private print to print the binomial tree of size 2^i.
                Console.WriteLine("2^{0}", i);
                print(heap[i].RightSibling, 0);
                Console.WriteLine("\n");
            }
        }


        private void print(BinomialNode<T> tree, int indent)
        {
            if (tree == null)  //if the tree is null
                return;

            else
            {
                if (tree.RightSibling != null)   // if a rightsibling does exist
                    print(tree.RightSibling, indent);  //recursively visit it

                Console.WriteLine(new String(' ', indent) + tree.Item);     // print the current item in order

                if (tree.LeftMostChild != null)   //if a leftmostchild does exist
                    print(tree.LeftMostChild, indent + 5);  //recusrively visit it.
            }

        }

        // MakeEmpty
        // Creates an empty binomial heap
        // Time complexity:  O(1)

        public void MakeEmpty()
        {
            heap = null;
            size = 0;
        }

        // Empty
        // Returns true is the binomial heap is empty; false otherwise
        // Time complexity:  O(1)

        public bool Empty()
        {
            return size == 0;
        }

        // Size
        // Returns the number of items in the binomial heap
        // Time complexity:  O(1)

        public int Size()
        {
            return size;
        }
    }

    //--------------------------------------------------------------------------------------

    // Used by class BinomailHeap<T>
    // Implements IComparable and overrides ToString (from Object)

    public class PriorityClass : IComparable
    {
        private int priorityValue;
        private char letter;

        public PriorityClass(int priority, char letter)
        {
            this.letter = letter;
            priorityValue = priority;
        }

        public int CompareTo(Object obj)
        {
            PriorityClass other = (PriorityClass)obj;   // Explicit cast
            return priorityValue - other.priorityValue;  // High values have higher priority
        }

        public override string ToString()
        {
            return letter.ToString() + " with priority " + priorityValue;
        }
    }

    //--------------------------------------------------------------------------------------

    // Test for above classes

    public class Test
    {
        public static void Main(string[] args)
        {
            int i;
            Random r = new Random();

            BinomialHeap<PriorityClass> BH = new BinomialHeap<PriorityClass>();

            for (i = 0; i < 6; i++)
            {
                BH.Add(new PriorityClass(r.Next(50), (char)('a')));
            }
            Console.WriteLine("Highest Priority Item is : {0}", BH.Front());
            BH.print();
            Console.ReadLine();
        }
    }
}
