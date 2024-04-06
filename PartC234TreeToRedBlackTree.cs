Fusing System;
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

                //Returns true if key k is successfully deleted; false otherwise. (10 marks)
        public bool Delete(T k)
        {
            return Delete(root, k);
        }
        
        //Private Delete method that is called recursively
        private bool Delete(Node<T> p, T k)
        {
            int keyPos = 0;

            if (p.isLeafNode())
            {
                //If a leaf node is reached and the key is in it, delete it and return true
                if (p.keys.Contains(k))
                {
                    p.keys.Remove(k);
                    Console.WriteLine("Key was successfully deleted.");
                    return true;
                }
                //If a leaf node is reached but the key is not in it, print an error and return false
                else
                {
                    Console.WriteLine("Key could not be found and was not deleted.");
                    return false;
                }

            }   

            //If the key is found in an internal node
            else if (p.keys.Contains(k))
            {
                keyPos = p.keys.IndexOf(k);//Getting the position of the key to be deleted in the node

                //Determine which case to follow
                //1. The child node that precedes k has at least 2 keys
                if (p.children[keyPos].keys.Count >= 2)
                {
                    //Take the last key of the child, add it to the current node, then delete k
                    p.keys.Insert(0, p.children[keyPos].keys[p.children[keyPos].keys.Count-1]);

                    Delete(p.children[keyPos], p.children[keyPos].keys[p.children[keyPos].keys.Count-1]); //Recursively delete

                    p.keys.Remove(k);
                    return true;
                }
                //2. The child node that succeeds k has at least 2 keys (and check if it exists)
                else if (keyPos + 1 < p.children.Count && p.children[keyPos+1].keys.Count >= 2)
                {
                    //Take the first key of the child, add it to the current node, then delete k
                    p.keys.Add(p.children[keyPos+1].keys[0]);

                    Delete(p.children[keyPos+1], p.children[keyPos+1].keys[0]); //Recursively delete

                    p.keys.Remove(k);
                    return true;
                }
                //3. Neither children have more than 2 nodes
                else
                {
                    //Merge the two children and move down
                    p.children[keyPos].keys.Add(k); //Adding the key from the parent to the child
                    
                    //If the other child exists, add it as well
                    if (keyPos + 1 < p.children.Count)
                    {
                        p.children[keyPos].keys.Add(p.children[keyPos + 1].keys[0]); //Adding the key from the other child

                        //Moving the children from the other child
                        for (int j = 0; j < p.children[keyPos+1].children.Count - 1; j--)
                        {
                            p.children[keyPos].children.Add(p.children[keyPos + 1].children[j]);
                        }

                        //Removing the other child from the current node
                        p.children.Remove(p.children[keyPos + 1]);
                    }

                    p.keys.Remove(k);

                    //If the root becomes empty in the process set the merged node to be the root
                    if (p.keys.Count == 0)
                    {
                        root = p.children[keyPos];
                    }

                    Delete(p.children[keyPos], k) ; //Recursively deleting k from the new subtree
                    
                    return true;
                }
            }

            //If the key is not found and a leaf node has not been reached, attempt to move down
            else
            {
                for (int i = 0; i <= p.keys.Count; i++)
                {
                    //Attempt to move down if the current key is less than the next key or if the last key is reached
                    if (i+1 == p.children.Count || k.CompareTo(p.keys[i]) < 0)
                    {
                        //If the child node has at least 2 (t) keys, move down
                        if (p.children[i].keys.Count >= maxKeysPerNode - 1)
                        {
                            return Delete(p.children[i], k);
                        }

                        //If it doesn't, modify the nodes
                        else
                        {
                            //Attempt to borrow a key from a sibling
                            //Move through siblings and check if they have more than one key
                            for (int j = 0; j < p.children.Count; j++)
                            {
                                if (i != 0)
                                {
                                    keyPos = i - 1;
                                }
                                //When looking at the current child node, ignore
                                //As well, check if the sibling node has more than 1 key
                                //If it does, 'move' that key to the child node
                                if (j != i && p.children[j].keys.Count > 1)
                                {
                                    //Insert the key of the parent node down to the child we want to move down, and take a key from the sibling and put it in the root
                                    //If j is less than i, the child is inserted on the left of the key
                                    if (j < i)
                                    {

                                        p.children[i].keys.Insert(0, p.keys[keyPos]); //Inserting the key into the current child
                                        p.keys.Remove(p.keys[keyPos]); //Removing the key from the parent

                                        p.keys.Insert(keyPos, p.children[j].keys[p.children[j].keys.Count - 1]); //Inserting the last key from the sibling into the parent
                                        p.children[j].keys.Remove(p.children[j].keys[p.children[j].keys.Count - 1]); //Removing the key from the sibling

                                        //Move the first child from the sibling to the current child
                                        if (p.children[j].children.Count > 0)
                                        {
                                            p.children[i].children.Insert(0, p.children[j].children[0]);
                                            p.children[j].children.Remove(p.children[j].children[0]);
                                        }

                                        break; //Exit the loop
                                    }

                                    //If j is greater than i, the child is inserted on the right of the key
                                    else
                                    {
                                        p.children[i].keys.Insert(1, p.keys[keyPos]); //Inserting the key into the current child
                                        p.keys.Remove(p.keys[keyPos]); //Removing the key from the parent

                                        p.keys.Insert(keyPos, p.children[j].keys[0]); //Inserting the first key of the sibling into the parent
                                        p.children[j].keys.Remove(p.keys[0]); //Removing the key from the sibling

                                        //Move the first child from the sibling to the current child
                                        if (p.children[j].children.Count > 0)
                                        {
                                            p.children[i].children.Add(p.children[j].children[0]);
                                            p.children[j].children.Remove(p.children[j].children[0]);
                                        }


                                        break; //Exit the loop
                                    }

                                }
                            }

                            //If the node cannot borrow from a sibling, merge with an adjacent node and use a node from the parent
                            if (p.children[i].keys.Count == 1)
                            {
                                for (int j = 0; j < p.children.Count; j++)
                                {
                                    //When looking at the current child node, ignore
                                    //However, merge with an adjacent node once found
                                    if (j != i)
                                    {
                                        //Insert the key of the parent node down to the child we want to move down, and take a key from the sibling and put it in the root
                                        //If j is less than i, the child is inserted on the left of the key
                                        if (j < i)
                                        {
                                            //Inserting the adjacent node's key
                                            p.children[i].keys.Insert(0, p.children[j].keys[0]);

                                            //Inserting the children of the adjacent node's children
                                            for (int m = 0; m < p.children[j].children.Count; m++)
                                            {
                                                p.children[i].children.Insert(m, p.children[j].children[m]);
                                            }

                                            p.children.Remove(p.children[j]); //Removing the adjacent node from the parent

                                            //Inserting a key and a child from the parent into the child node
                                            p.children[i].keys.Insert(2, p.keys[i]);
                                            p.children[i].children.Insert(3, p.children[i]);

                                            //Removing the key and child from the parent node
                                            p.keys.Remove(p.keys[i]);
                                            p.children.Remove(p.children[i]);

                                            break; //Exit the loop
                                        }

                                        //If j is greater than i, the child is inserted on the right of the key
                                        else
                                        {
                                            //Inserting the adjacent node's key
                                            p.children[i].keys.Insert(1, p.children[j].keys[0]);

                                            //Inserting the children of the adjacent node's children
                                            for (int m = 0; m < p.children[j].children.Count; m++)
                                            {
                                                p.children[i].children.Insert(m+2, p.children[j].children[m]);
                                            }

                                            p.children.Remove(p.children[j]); //Removing the adjacent node from the parent


                                            //Inserting a key from the parent into the child node
                                            p.children[i].keys.Insert(1, p.keys[i]);
                       

                                            //Removing the key from the parent node
                                            p.keys.Remove(p.keys[i]);
                                            

                                            break; //Exit the loop
                                        }

                                    }
                                }

                                //If the parent no longer has any keys, make the merged node the root
                                if (p.keys.Count == 0)
                                {
                                    root = p.children[i];
                                    return Delete(root, k);
                                }
                            }

                            return Delete(p.children[i], k);
                        }

                    }

                }
   
             }

            return true; //If the key is not found, return false
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

        // Adapted print helper method to handle 3-nodes 
        private void Print(Node<T> node, int k)
        {
            string t = new string(' ', k);
            string s = t;
            if (node != null)
            {
                //Printing off the nodes on the right
                for (int j = node.children.Count - 1; j <= node.children.Count - 1 && j > 0; j++)
                {
                    Print(node.children[j], k + 5);
                }

                Console.WriteLine();

                //Printing out the keys in the node
                for (int m = node.keys.Count - 1; m >= 0; m--)
                {
                    s += node.keys[m];
                }
                if (node.children.Count == 3){
                    s += new string(' ', 3);
                    for (int m = node.children[1].keys.Count - 1; m >= 0; m--)
                    {
                        s += node.children[1].keys[m];
                    }
                    Console.WriteLine(s);
                    Console.WriteLine();

                    //Printing off the nodes on the left
                    for (int j = node.children.Count - 3; j >= 0; j--)
                    {
                        Print(node.children[j], k + 5);
                    }
                }
                else
                {
                    Console.WriteLine(s);
                    Console.WriteLine();

                    //Printing off the nodes on the left
                    for (int j = node.children.Count - 2; j >= 0; j--)
                    {
                        Print(node.children[j], k + 5);
                    }
                }

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
       // tree.Delete('E');
       // tree.Delete('A');
       // tree.Delete('F');
       // tree.Delete('D');
       // tree.Delete('H');
       // tree.Delete('C');
       // tree.Delete('Z');
       // tree.Delete('X');
       // tree.Delete('L');
        //tree.Delete('O');
        //tree.Delete('Y');
        //tree.Delete('G');
        
        tree.Print();

        Console.WriteLine("Is 'F' in the tree?");
        Console.WriteLine(tree.Search('F'));

        Console.WriteLine("Is 'J' in the tree?");
        Console.WriteLine(tree.Search('J'));

        BSTforRBTree<char> B = tree.Convert();
        B.Print();
        
        Console.ReadLine();
    }
}
}
