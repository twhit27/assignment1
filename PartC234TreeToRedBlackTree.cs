using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jamieleneveCOIS3020Assignment3
{
    public enum Color { RED, BLACK };       // Colors of the red-black tree

    public interface ISearchable<T>
    {
        void Add(T item, Color rb);
        void Print();
    }

    //-------------------------------------------------------------------------

    // Implementation:  BSTforRBTree

    class BSTforRBTree<T> : ISearchable<T> where T : IComparable
    {

        // Common generic node class for a BSTforRBTree

        private class Node
        {
            // Read/write properties

            public T Item;
            public Color RB;
            public Node Left;
            public Node Right;

            public Node(T item, Color rb)
            {
                Item = item;
                RB = rb;
                Left = Right = null;
            }
        }

        private Node root;

        public BSTforRBTree()
        {
            root = null;    // Empty BSTforRBTree
        }

        // Add 
        // Insert an item into a BSTforRBTRee
        // Duplicate items are not inserted
        // Worst case time complexity:  O(log n) 
        // since the maximum depth of a red-black tree is O(log n)

        public void Add(T item, Color rb)
        {
            Node curr;
            bool inserted = false;

            if (root == null)
                root = new Node(item, rb);   // Create a root
            else 
            {
                curr = root;
                while (!inserted)
                {
                    if (item.CompareTo(curr.Item) < 0)
                    {
                        if (curr.Left == null)              // Empty spot
                        {
                            curr.Left = new Node(item, rb);
                            inserted = true;
                        }
                        else
                            curr = curr.Left;               // Move left
                    }
                    else
                        if (item.CompareTo(curr.Item) > 0)
                        {
                            if (curr.Right == null)         // Empty spot
                            {
                                curr.Right = new Node(item, rb);
                                inserted = true;
                            }
                            else
                                curr = curr.Right;          // Move right
                        }
                        else
                            inserted = true;                // Already inserted
                }
            }
        }

        public void Print()
        {
            Print(root, 0);                // Call private, recursive Print
            Console.WriteLine();
        }

        // Print
        // Inorder traversal of the BSTforRBTree
        // Time complexity:  O(n)

        private void Print(Node node, int k)
        {
            string s;
            string t = new string(' ', k);

            if (node != null)
            {
                Print(node.Right, k+4);
                s = node.RB == Color.RED ? "R" : "B" ;
                Console.WriteLine(t + node.Item.ToString() + s);
                Print(node.Left, k+4);
            }
        }
    }
    class Node<T>
    {
        public List<Node<T>> children;
        public List<T> keys;

        public Node()
        {
            //set our min number of child nodes 
            children = new List<Node<T>>();
            keys = new List<T>();
        }//end constructor

        /*
         * Checks the number of children to report if we are a leaf node.
         */
        public bool isLeafNode()
        {
            return children.Count == 0;
        }




    }
    class Tree<T> where T : IComparable
    {
        
        //public for testing
        public Node<T> root;//the root of our tree
        private int maxKeysPerNode;
        private int minKeysPerNode;
        private int maxChildrenPerNode;

        public Tree()
        {
            //get our root.
            int t = 2;
            this.root = new Node<T>();
            this.maxKeysPerNode = 2 * t - 1;
            this.minKeysPerNode = t - 1;
            this.maxChildrenPerNode = 2 * t;

        }
        /*
         * Search()
         * paramter: the key to search for of type T
         * returns true if key found, false otherwise.
         * 
         */
        public bool Search(T key)
        {
            return Search(this.root, key);

        }//end Contains

        /*
         * Private helper method for contains that takes a node for recursive calls.
         */
        private bool Search(Node<T> node, T key)
        {
            //if we're null, it isn't here.
            if (node == null)
            {
                return false;
            }
            //if this node has the key, then we found it
            else if (node.keys.Contains(key))
            {
                return true;
            }

            else //keep looking
            {
                //if we have no children to check, it isn't there.
                if (node.children.Count == 0)
                {
                    return false;
                }
                //else we keep looing
                int index = 0;
                //get the index of the item that is bigger than us
                //or the last index of the children because we're bigger
                for (; index < node.children.Count; index++)
                {
                    if (index == node.keys.Count ||
                        key.CompareTo(node.keys[index]) < 0)
                    {
                        break;
                    }
                }//end for
                return Search(node.children[index], key);

            }//end else we didn't find it

        }

        /*
         * Convert()
         * returns the red black tree equivalent red black tree for the current 234 tree.
         * 
         */
        public BSTforRBTree<T> Convert()
        {
            BSTforRBTree<T> result = new BSTforRBTree<T>();
            result = Convert(this.root, result);
            return result;
        }

        /*
         * Private helper method for convert that takes a node for recursive calls.
         */
        private BSTforRBTree<T> Convert(Node<T> node, BSTforRBTree<T> RBT)
        {
            if (node.keys.Count == 1)
            {
                Color c = Color.BLACK;
                RBT.Add(node.keys[0], c);
            }
            else if (node.keys.Count == 2)
            {
                Color c = Color.BLACK;
                RBT.Add(node.keys[1], c);
                c = Color.RED;
                RBT.Add(node.keys[0], c);
            }
            else if (node.keys.Count == 3)
            {
                Color c = Color.BLACK;
                RBT.Add(node.keys[1], c);
                c = Color.RED;
                RBT.Add(node.keys[0], c);
                RBT.Add(node.keys[2], c);
            }
            if (!node.isLeafNode())
                for (int index = 0; index < node.children.Count; index++)
                    Convert(node.children[index], RBT);
            return RBT;
        }
    }
        //--------------------------------------------------------------------------------------

        // Test for above classes
        /*public class Test
    {
        public static void Main(string[] args)
        {
            Random randomValue = new Random();       // Random number
            Color c;

            BSTforRBTree<int> B = new BSTforRBTree<int>();
            for (int i = 0; i < 20; i++)
            {
                c = i % 2 == 0 ? Color.RED : Color.BLACK;
                B.Add(randomValue.Next(90) + 10, c); // Add random integers with alternating colours
            }
            B.Print();                               // In order

            Console.ReadLine();
        }
    }*/
}
