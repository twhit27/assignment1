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
            int slot = highestPriority.Degree;
            BinomialNode<T> p, q;

            q = heap[slot];
            p = null;

            //Check 1: if there are 2 or more trees in the list you will need to navigate to one before highest
            if (q.RightSibling.RightSibling != null)
            {
                while (q != null)
                {
                    if (q.RightSibling == highestPriority)
                    {
                        p = q.RightSibling;
                        break;
                    }
                    else
                        q = q.RightSibling;
                }
            }

            //Check 2: if it's a single node you don't need left child, just skip over the node and go straight to coalesce 
            if (slot == 0)
            {
                if (p == null || p.RightSibling == null)
                    q.RightSibling = null;
                else
                    q.RightSibling = p.RightSibling;

            }

            //Check 3: (main block) else the trees have 2 or more items
            else
            {
                p = q.RightSibling;

                // remove root approach if at end of list
                if (p.RightSibling == null)
                {
                    q.RightSibling = null;
                }
                // remove root approach otherwise   
                else
                {
                    q.RightSibling = p.RightSibling;
                }

                // move p and q into the fragmented tree
                q = p.LeftMostChild;
                p = q.RightSibling;

                //this loop will move the fragments into their proper degree slot
                while (q != null)
                {
                    //add the sub trees to the front of their degree slot
                    moveDown(q.Degree, ref q);

                    //iterate down the fragmented tree
                    q = p;
                    if (p != null)
                        p = p.RightSibling;
                }

            }
            //rest highest to be found again in coalesce 
            highestPriority = null;
            size--;
            //clean up!
            Coalesce();
        }

        public void Coalesce()
        {
            if (Empty())
            {
                Console.WriteLine("Empty tree!");
                return;
            }


            //main outer loop that lets us traverse down the heap array to each Bk type
            for (int i = 0; i < heap.Length; i++)
            {
                BinomialNode<T> prev, curr, next;

                prev = heap[i];

                // Check 1: empty slot
                if (prev.RightSibling == null)
                    continue;

                curr = prev.RightSibling;

                // Check 2: checking for a slot with only 1 item (no need to consolidate, but still need to check if the new highest value is there)
                if (curr.RightSibling == null)
                {
                    if (highestPriority == null || curr.Item.CompareTo(highestPriority.Item) > 0)
                        highestPriority = curr;
                    next = null;
                }
                else
                    next = curr.RightSibling;

                // Check 3: (main block) consolidate within the slot
                while (next != null)
                {
                    //keep highest priority valid 
                    if (highestPriority == null || curr.Item.CompareTo(highestPriority.Item) > 0)
                        highestPriority = curr;

                    // Cases 1 and 2 don't strictly exist anymore, but it's still useful to move the pointers to the end of the slot
                    if (curr.Degree != next.Degree)
                    {
                        prev = curr;
                        curr = next;
                    }

                    // Case 3: same degree, merge next under current
                    else if (curr.Item.CompareTo(next.Item) >= 0)
                    {
                        // merge trees
                        curr.RightSibling = next.RightSibling;
                        BinomialLink(next, curr);

                        // move down
                        prev.RightSibling = curr.RightSibling;
                        moveDown(i + 1, ref curr);

                        //restore curr in the list
                        curr = prev.RightSibling;

                    }

                    // Case 4: same degree, merge current under next
                    else
                    {
                        // merge trees
                        prev.RightSibling = next;
                        BinomialLink(curr, next);
                        curr = next;

                        // move down
                        prev.RightSibling = next.RightSibling;
                        moveDown(i + 1, ref curr);

                        //restore curr in the list
                        curr = prev.RightSibling;

                    }

                    // move next down the list
                    if (curr != null)
                        next = curr.RightSibling;
                    else
                        next = null;
                }
            }
        }

        // Helper Method: moveDown
        // Will move a binomial tree down in the heap array

        private void moveDown(int slot, ref BinomialNode<T> tree)
        {
            tree.RightSibling = heap[slot].RightSibling;
            heap[slot].RightSibling = tree;
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
            return priorityValue.ToString();
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
