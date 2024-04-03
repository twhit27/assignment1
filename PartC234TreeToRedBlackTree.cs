using System;
using System.Collections;
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
                Print(node.Right, k + 4);
                s = node.RB == Color.RED ? "R" : "B";
                Console.WriteLine(t + node.Item.ToString() + s);
                Print(node.Left, k + 4);
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
    class TwoThreeFourTree<T> where T : IComparable
    {

        //public for testing
        public Node<T> root;//the root of our tree
        private int maxKeysPerNode;
        private int minKeysPerNode;
        private int maxChildrenPerNode;

        public TwoThreeFourTree()
        {
            //get our root.
            int t = 2;
            this.root = new Node<T>();
            this.maxKeysPerNode = 2 * t - 1;
            this.minKeysPerNode = t - 1;
            this.maxChildrenPerNode = 2 * t;

        }

        //Returns true if key k is successfully inserted; false otherwise. (6 marks)
        public bool Insert(T k)
        {
            Node<T> p = root;

            for (int i = 0; i < maxKeysPerNode; i++)
            {
                //If the key is in the node, print out an error message and return false
                if (p.keys.Contains(k))
                {
                    Console.WriteLine("Key could not be inserted as it already exists in the tree.");
                    return false;
                }

                //If node is full, split the node
                else if (p.keys.Count == maxKeysPerNode)
                {
                    Split(p, i);
                    i = -1; //Resetting i
                }

                //If the end of the node is reached and it is a leaf node, insert
                else if (p.keys.Count - i == 0 && p.isLeafNode())
                {
                    p.keys.Insert(i, k);
                    return true;
                }

                //If the key being added is greater than all of the keys in the node, but the node is not a leaf, attempt to go down
                else if (p.keys.Count - i == 0 && !p.isLeafNode())
                {
                    //Move down if the node is not full
                    if (p.children[i].keys.Count != maxKeysPerNode)
                    {
                        p = p.children[i];
                        i = -1; //Resetting i
                    }
                    //If the child node is full, split
                    else
                    {
                        Split(p, i);
                        i = -1;
                    }
                }

                //If the key being inserted is less than the next key, attempt to insert
                else if (k.CompareTo(p.keys[i]) < 0)
                {

                    //If there is room in the current node and the node is a leaf node, insert here
                    if (p.isLeafNode() && p.keys.Count < maxKeysPerNode)
                    {
                        p.keys.Insert(i, k);
                        return true;
                    }
                    //If there is not room in the current node, attempt to move down
                    else
                    {
                        //Move down if the node is not full
                        if (p.children[i].keys.Count != maxKeysPerNode)
                        {
                            p = p.children[i];
                            i = -1; //Resetting i
                        }
                        //If the child node is full, split
                        else
                        {
                            Split(p, i);
                            i = -1;
                        }

                    }
                }
            }

            return true;
        }

        //Split method for the insert
        //Splits a full node
        private void Split(Node<T> x, int i)
        {

            //Making the new root
            Node<T> newRoot = new Node<T>();

            //Making the new nodes
            Node<T> split1 = new Node<T>();
            Node<T> split2 = new Node<T>();


            //If the parent node is full
            if (x.keys.Count == maxKeysPerNode)
            {
                //Initializing the values of the new root
                newRoot.keys.Insert(0, x.keys[1]);

                //Splitting the node
                //Getting the left split's keys
                split1.keys.Insert(0, x.keys[0]);

                //Getting the left split's children
                for (int j = x.children.Count - 3; j >=0 ; j--)
                {
                    split1.children.Insert(0, x.children[j]);
                }

                //Getting the right split's keys
                split2.keys.Insert(0, x.keys[2]);

                //Getting the right split's children
                for (int j = x.children.Count-1; j >= 2; j--)
                {
                    split2.children.Insert(0, x.children[j]);
                }

                //Adding the split's to the new node
                newRoot.children.Insert(0, split1);
                newRoot.children.Insert(1, split2);

                //Setting the new root
                x.children = newRoot.children;
                x.keys = newRoot.keys;                
            }
            //Else, insert the top node of the child within the parent node
            else
            {
                //Initializing the values of the new root
                newRoot.keys.Insert(0, x.children[i].keys[1]);

                //Splitting the node
                
                //Getting the left split's keys
                split1.keys.Insert(0, x.children[i].keys[0]);

                //Getting the left split's children
                for (int j = x.children[i].children.Count - 3; j >= 0; j--)
                {
                    split1.children.Insert(0, x.children[i].children[j]);
                }

                //Getting the right split's keys
                split2.keys.Insert(0, x.children[i].keys[2]);

                //Getting the right split's children
                for (int j = x.children[i].children.Count-1; j >= 2; j--)
                {
                    split2.children.Insert(0, x.children[i].children[j]);
                }

                //Adding the split's to the new node
                newRoot.children.Insert(0, split1);
                newRoot.children.Insert(1, split2);

                //Inserting the split node into the parent
                for (int m = 0; m <= x.keys.Count; m++)
                {
                    //If the end of the node parent node is reached, insert
                    if (x.keys.Count - m == 0)
                    {
                        //Inserting the key of the split
                        x.keys.Insert(m, newRoot.keys[0]);

                        //Inserting the children of the split
                        x.children[m] = split1;
                        x.children.Insert(m + 1, split2);

                        break; //Exit the loop
                    }

                    //If the node's key is less than the current key, insert
                    else if (newRoot.keys[0].CompareTo(x.keys[m]) < 0)
                    {
                        //Inserting the key of the split
                        x.keys.Insert(m, newRoot.keys[0]);

                        //Inserting the children of the split
                        x.children[m] = split1;
                        x.children.Insert(m + 1, split2);
                       
                        break; //Exit the loop
                    }
                }
            }
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

        //Prints out the keys of the 2-3-4 tree in order. (4 marks)
        public void Print()
        {
            Print(root, 0);                // Call private, recursive Print
            Console.WriteLine();
        }

        private void Print(Node<T> node, int k)
        {
            string t = new string(' ', k);

            if (node != null)
            {

                //Printing off the nodes on the right
                for (int i = 1; i <= node.children.Count-1; i++)
                {
                    Print(node.children[i], k + 3);
                }

                Console.WriteLine();

                //Printing out the keys in the node
                for (int m = node.keys.Count - 1; m >= 0; m--)
                {
                    Console.WriteLine(t + node.keys[m]);
                }

                Console.WriteLine();

                //Printing off the nodes on the left
                for (int i = node.children.Count-2; i >= 0; i--)
                {
                    Print(node.children[i], k + 3);
                }
            }
        }
    }
    //--------------------------------------------------------------------------------------

    // Test for above classes
    public class Test
{
    public static void Main(string[] args)
    {
        //Random randomValue = new Random();       // Random number
        //Color c;

        //BSTforRBTree<int> B = new BSTforRBTree<int>();
        //for (int i = 0; i < 20; i++)
        //{
        //    c = i % 2 == 0 ? Color.RED : Color.BLACK;
        //    B.Add(randomValue.Next(90) + 10, c); // Add random integers with alternating colours
        //}
        //B.Print();                               // In order

        TwoThreeFourTree<char> tree = new TwoThreeFourTree<char>();
        tree.Insert('B');
        tree.Insert('C');
        tree.Insert('C');
        tree.Insert('G');
        tree.Insert('A');
        tree.Insert('Z');
        tree.Insert('X');
        tree.Insert('F');
        tree.Insert('E');
        tree.Insert('Y');
        tree.Insert('H');
        tree.Insert('L');
        tree.Insert('O');
        
        tree.Print();
        
        Console.ReadLine();
    }
}
}
