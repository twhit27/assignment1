/*
 * Assignment 2: Ropes
 * Jamie Le Neve, Camryn Moerchen, Victoria Whitworth
 * COIS 3020H
 * Brian Patrick
 * March 10, 2024
 */

using System;
using System.Data.SqlTypes;
using System.Reflection;


public class Rope<T>
    {
        //Node class for Rope
        public class Node<T>
        {
            public string Item { get; set; }     // Item stored in the Node
            public int Length { get; set; }     // Used to store the char size of the subtree rooted here
            public Node<T> Left { get; set; }     // Left subtree
            public Node<T> Right { get; set; }     // Right subtree

            // Constructor Node

            public Node(string item, int length, Node<T> L, Node<T> R)
            {
                Item = item;
                Length = length;
                Left = L;
                Right = R;
            }

            //just helps identify nodes in the Debugging panel in Visual Studio
            public override string ToString()
            {
                //it's a leaf node
                if (Item != null)
                    return Item.ToString();
                else
                    return Length.ToString();
            }
        }

        private Node<T> root = new Node<T>("", 0, null, null);
        private const int MAX_LENGTH = 10;


        //Rope Constructor 
        //Create a balanced rope from a given string S (5 marks).
        // Public Constructor
        // Updates the root by calling Build
        public Rope(string S)
        {
            Node<T> current = new Node<T>("", 0, null, null);
            root = current;
            root.Length = S.Length;
            int mid = S.Length / 2;
            if (S.Length <= MAX_LENGTH)
            {
                current.Length = S.Length;
                current.Item = S;
            }
            else
            {
                current.Left = Build(S, 0, mid);
                current.Right = Build(S, mid, S.Length - mid);
                //current.Left.Length = S.Substring(0, mid).Length;
                //current.Right.Length = S.Substring(mid, S.Length-mid).Length;
                current.Length = current.Left.Length + current.Right.Length; // S.Substring(0, mid).Length; //current.Left.Length + current.Right.Length;
            }
            //root = Build(S, 0, S.Length);
        }

        //Insert Method
        //Insert string S at index i (5 marks).
        public void Insert(string S, int i)
        {

        }

        //Delete Method
        //Delete the substring S[i, j] (5 marks).
        public void Delete(int i, int j)
        {

        }

        //Substring Method
        //Return the substring S[i, j] (6 marks).
        public string Substring(int i, int j)
        {
            return "string";
        }

        //Find Method
        //Return the index of the first occurrence of S; -1 otherwise (9 marks).
        public int Find(string S)
        {
            return -1;
        }

        //CharAt Method
        //Return the character at index i (3 marks).
        public char CharAt(int i)
        {
            return 'i';
        }

        //IndexOf Method
        //Return the index of the first occurrence of character c (4 marks).
        public int IndexOf(char c)
        {
            return -1;
        }

        //Reverse Method
        //Reverse the string represented by the current rope (5 marks).
        public void Reverse()
        {

        }

        //Length Method
        //Return the length of the string (1 mark).
        public int Length()
        {
            return root.Length;
        }

        //ToString Method
        //Return the string represented by the current rope (4 marks).
        public string ToString()
        {
            string rope = "";
            rope = GetString(root, rope);
            return rope;
        }

        private string GetString(Node<T> current, string rope)
        {
            if (current.Left != null)
                rope = GetString(current.Left, rope);
            if (current.Right != null)
                rope = GetString(current.Right, rope);
            if (current.Item != null)
                rope += current.Item;
            else
                return rope;
            return rope;
        }

        //PrintRope Method
        //Print the augmented binary tree of the current rope (4 marks).
        public void PrintRope()
        {
            PrintRope(root, 0); //Calls the private print rope method, giving the root of the rope
        }

        // Private PrintRope
        // Recursively implements the public PrintRope
        // Heavily inspired by Prof. Patricks COIS 2020 Print method for Binary Trees

        private void PrintRope(Node<T> root, int indent)
        {
            if (root != null)
            {
                PrintRope(root.Right, indent + 3);
                //Leaf nodes will not have non-empty items, so print their items 
                //Printing out the items and char length of each leaf node
                if (root.Item != "")
                {
                    Console.Write(new String(' ', indent) + root.Length);
                    Console.WriteLine(new String(' ', indent) + root.Item);
                }

                //Only print the char length for parent nodes
                else
                    Console.WriteLine(new String(' ', indent) + root.Length);
                PrintRope(root.Left, indent + 3);
            }
        }

        //Build Method
        //Recursively build a balanced rope for S[i, j] and return its root (part of the constructor).
        // Build
        // Recursively builds a balanced rope based on the length of the string with placeholder values
        // Inspired by Prof. Patricks binary tree constructor from COIS 2020
        private Node<T> Build(string s, int i, int j)
        {
            string sub = s.Substring(i, j);
            Node<T> current = new Node<T>("", 0, null, null);
            // this if block identifies and updates leaf nodes
            if (sub.Length <= MAX_LENGTH)
            {
                // return a new leaf node to hook onto the parent node being passed
                //string sub = s.Substring(i, j - i);
                current.Length = sub.Length;
                current.Item = sub;
                //return new Node<T>(sub, sub.Length, null, null);
            }
            // this block handles parent nodes and drives the recursion
            // essentially, it will continously split the string into 2 until the sections are at or below max node length
            else
            {
                int mid = sub.Length / 2;   //mid will provide the middle point value that allows the following code to split the string in half
                current.Left = Build(sub, 0, mid);    // create the left subtree by using new indexes to pass the first half of the substring
                current.Right = Build(sub, mid, sub.Length - mid);   // create the right subtree by using indexes to pass the second half
                current.Length = current.Left.Length + current.Right.Length; // sub.Substring(0, mid).Length; // current.Left.Length + current.Right.Length;
                //int len = left.Length + right.Length;   // update parent node length accordingly
                //return new Node<T>("", len, left, right);   // return parent node
            }
            return current;

        }

        //Concatenate Method
        //Return the root of the rope constructed by concatenating two ropes with roots p and q (3 marks).
        private Node<T> Concatenate(Node<T> p, Node<T> q)
        {
            Node<T> newRoot = new Node<T>("", (p.Length + q.Length), p, q);
            //newRoot.Rebalance()
            return newRoot;
        }

        public void SplitRope(int i)
        {
            Node<T> rightRoot = Split(root, i);
            Console.WriteLine("Printing RightRope");
            PrintRope(rightRoot, 0);
            Console.WriteLine();
            Console.WriteLine("Printing Original Rope");
            PrintRope(root, 0);
        }

        //Split Method
        //Split the rope with root p at index i and return the root of the right subtree (9 marks).
        private Node<T> Split(Node<T> p, int i)
        {
            /* Getting a status update on the variables
            Console.Write("i: ");
            Console.WriteLine(i);
            Console.Write("root.Left - i: ");
            Console.WriteLine(root.Left.Length - i);
            Console.Write("Root.Left: ");
            Console.WriteLine(root.Left.Length);*/
            Node<T> rightRoot = new Node<T>("", 0, null, null);
            if (i > p.Length)
                Console.WriteLine("The rope cannot be split at this location");
            // Trying to split the rope on the right side of the root
            else if (i > p.Left.Length)
            {
                // Rope will be split on the left side of the current node
                if (i - p.Right.Length < p.Right.Length)
                {
                    /* Getting a status update on the variables
                    Console.Write("i: ");
                    Console.WriteLine(i);
                    Console.Write("i - p.Right: ");
                    Console.WriteLine(i - p.Right.Length);
                    Console.Write("p.Right: ");
                    Console.WriteLine(p.Right.Length);*/

                    rightRoot.Left = root.Left;
                    rightRoot.Right = SplitRopeRight(p.Right, i, 0, new Node<T>("", 0, null, null));
                    rightRoot.Length = rightRoot.Left.Length;
                    root.Left = null;
                }
                // Rope will be split on the right side of the current node
                else if (i - p.Right.Length > p.Right.Length)
                {
                    /* Getting a status update on the variables
                    Console.Write("i: ");
                    Console.WriteLine(i);
                    Console.Write("i - p.Right: ");
                    Console.WriteLine(i - p.Right.Length);
                    Console.Write("p.Right: ");
                    Console.WriteLine(p.Right.Length);*/
                    rightRoot.Left = root.Left;
                    rightRoot.Right = SplitRopeRight(p.Right, i, 3, new Node<T>("", 0, null, null));
                    rightRoot.Length = rightRoot.Left.Length;
                    root.Left = null;
                }
                // Rope will be split at the current node
                else if (i - p.Right.Length == 0)
                {
                    rightRoot.Left = p.Right.Left;
                    rightRoot.Length = p.Length - p.Right.Length;
                    p.Right.Left = null;
                    p.Length = p.Right.Length;
                    p = p.Right;
                }
            }
            // Trying to split the rope on the left side of the root
            else if (i < p.Left.Length)
            {
                Console.WriteLine("i < p.Left.Length");
                // Rope will be split on the left side of the current node
                if (p.Left.Length - i < p.Left.Length)
                {
                    Console.WriteLine("p.Left.Length - i < p.Left.Length");
                    /* Getting a status update on the variables
                    Console.Write("i: ");
                    Console.WriteLine(i);
                    Console.Write("p.Left - i: ");
                    Console.WriteLine(p.Left.Length - i);
                    Console.Write("p.Left: ");
                    Console.WriteLine(p.Left.Length);*/

                    rightRoot.Left = SplitRopeLeft(p.Left, i, 0, new Node<T>("", 0, null, null));
                }
                // rope will be split on the right side of the current node
                else if (i - p.Left.Length > p.Left.Length)
                {
                    Console.WriteLine("i - p.Left.Length > p.Left.Length");
                    /* Getting a status update on the variables
                    Console.Write("i: ");
                    Console.WriteLine(i);
                    Console.Write("p.Left - i: ");
                    Console.WriteLine(p.Left.Length - i);
                    Console.Write("p.Left: ");
                    Console.WriteLine(p.Left.Length);*/
                    rightRoot.Left = SplitRopeLeft(p.Left, i, 3, new Node<T>("", 0, null, null));
                }
                // rope will be split on the current node
                else if (i - p.Left.Length == 0)
                {
                    Console.WriteLine("i - p.Left.Length == 0");
                    rightRoot.Left = p.Left.Right;
                    rightRoot.Length = p.Length - p.Left.Length;
                    p.Left.Right = null;
                    p.Length = p.Left.Length;
                    p = p.Left;
                }
            }
            return rightRoot;
            // Method to assist in the split of the string
            // Traverses the rope to find the node that contains the index
            Node<T> SplitRopeRight(Node<T> current, int i, int directions, Node<T> currRoot)
            {
                // Trying to split the rope on the right side
                if (current.Length < i)
                {
                    // Rope will be split on the left side of the current node
                    if (i - current.Length < 0 && current.Left != null)
                    {
                        /* Getting a status update on the variables
                        Console.Write("i: ");
                        Console.WriteLine(i);
                        Console.Write("current.Length - i: ");
                        Console.WriteLine(current.Length - i);
                        Console.Write("Current.Left: ");
                        Console.WriteLine(current.Length);*/
                        if (directions < 2)
                        {
                            if (directions == 0)
                                currRoot = LinkRoot(0);
                            currRoot = SplitRopeRight(current.Left, i, directions, currRoot);
                        }
                        else
                        {
                            if (directions == 3)
                                currRoot = LinkRoot(2);
                            else
                                currRoot = LinkNewRope(1);
                            currRoot = SplitRopeRight(current.Left, i, 2, currRoot);
                        }
                    }
                    // rope will be split on the current node
                    else if (i - current.Length > 0 && current.Right != null)
                    {
                        /* Getting a status update on the variables
                        Console.Write("i: ");
                        Console.WriteLine(i);
                        Console.Write("i - current.Length: ");
                        Console.WriteLine(i - current.Length);
                        Console.Write("Current.Left: ");
                        Console.WriteLine(current.Length);*/
                        if (directions < 2)
                        {
                            if (directions == 0)
                                currRoot = LinkRoot(1);
                            else
                                currRoot = LinkNewRope(0);
                            currRoot = SplitRopeRight(current.Right, i - current.Length, 1, currRoot);
                        }
                        else
                        {
                            if (directions == 3)
                                currRoot = LinkRoot(3);
                            currRoot = SplitRopeRight(current.Right, i - current.Length, directions, currRoot);
                        }
                    }
                    // split will occur in between the right and left side of the current node
                    else if (i - current.Length == 0)
                    {
                        /* Getting a status update on the variables
                        Console.Write("i: ");
                        Console.WriteLine(i);
                        Console.Write("i - current.Length: ");
                        Console.WriteLine(i - current.Length);
                        Console.Write("Current.Left: ");
                        Console.WriteLine(current.Length);*/
                        if (directions < 2)
                        {
                            if (directions == 0)
                                currRoot = LinkRoot(1);
                            else if (directions == 1)
                                currRoot = LinkNewRope(0);
                        }
                        else
                        {
                            if (directions == 3)
                                currRoot = LinkRoot(2);
                            else if (directions == 2)
                                currRoot = LinkNewRope(1);
                        }
                    }
                    // rope will be split on the current node
                    else
                    {
                        current.Left = Build(current.Item, 0, i);
                        current.Right = Build(current.Item, i, i - current.Length);
                        current.Length = current.Left.Length;
                        current.Item = "";
                        currRoot = SplitRopeRight(current, i, directions, currRoot);
                    }
                }
                // Trying to split the rope on the left side
                else if (current.Length > i)
                {
                    Console.WriteLine("current.Length > i");
                    // Rope will be split on the left side of the current node
                    if (current.Length - i < 0 && current.Left != null)
                    {
                        Console.WriteLine("current.Length - i < 0 && current.Left != null");
                        /* Getting a status update on the variables
                        Console.Write("i: ");
                        Console.WriteLine(i);
                        Console.Write("current.Length - i: ");
                        Console.WriteLine(current.Length - i);
                        Console.Write("Current.Left: ");
                        Console.WriteLine(current.Length);*/
                        if (directions < 2)
                        {
                            if (directions == 0)
                                currRoot = LinkRoot(0);
                            currRoot = SplitRopeRight(current.Left, i, directions, currRoot);
                        }
                        else
                        {
                            if (directions == 3)
                                currRoot = LinkRoot(2);
                            else
                                currRoot = LinkNewRope(1);
                            currRoot = SplitRopeRight(current.Left, i, 2, currRoot);
                        }
                    }
                    // Rope will be split on the right side of the current node
                    else if (current.Length - i > 0 && current.Right != null)
                    {
                        Console.WriteLine("current.Length - i > 0 && current.Right != null");
                        /* Getting a status update on the variables
                        Console.Write("i: ");
                        Console.WriteLine(i);
                        Console.Write("current.Length - i: ");
                        Console.WriteLine(current.Length - i);
                        Console.Write("Current.Left: ");
                        Console.WriteLine(current.Length);
                        Console.WriteLine(directions);*/
                        if (directions < 2)
                        {
                            if (directions == 0)
                                currRoot = LinkRoot(1);
                            else
                                currRoot = LinkNewRope(0);
                            currRoot = SplitRopeRight(current.Right, current.Length - i, 1, currRoot);
                        }
                        else
                        {
                            if (directions == 3)
                                currRoot = LinkRoot(3);
                            currRoot = SplitRopeRight(current.Right, current.Length - i, directions, currRoot);
                        }
                    }
                    // split will occur in between the right and left side of the current node
                    else if (current.Length - i == 0)
                    {
                        Console.WriteLine("current.length - i == 0");
                        /* Getting a status update on the variables
                        Console.Write("i: ");
                        Console.WriteLine(i);
                        Console.Write("i - current.Length: ");
                        Console.WriteLine(i - current.Length);
                        Console.Write("Current.Left: ");
                        Console.WriteLine(current.Length);*/
                        if (directions < 2)
                        {
                            if (directions == 0)
                                currRoot = LinkRoot(1);
                            else if (directions == 1)
                                currRoot = LinkNewRope(0);
                        }
                        else
                        {
                            if (directions == 3)
                                currRoot = LinkRoot(2);
                            else if (directions == 2)
                                currRoot = LinkNewRope(1);
                        }
                    }
                    // rope will be split on the current node
                    else
                    {
                        Console.WriteLine("else");
                        current.Left = Build(current.Item, 0, current.Length - i);
                        current.Right = Build(current.Item, current.Length - i, current.Length - i);
                        current.Length = current.Left.Length;
                        current.Item = "";
                        currRoot = SplitRopeRight(current, i, directions, currRoot);
                    }
                }

                return currRoot;
                // Supporting method for split rope that links nodes to the proper root
                Node<T> LinkRoot(int linkLocation)
                {
                    // Linking left node to new root
                    if (linkLocation < 2)
                    {
                        currRoot.Left = current.Left;
                        currRoot.Length = current.Length;
                        if (linkLocation == 1)
                            current.Left = null;
                    }
                    // Linking right node to new root
                    else
                    {
                        currRoot.Right = current.Right;
                        currRoot.Length = current.Length;
                        if (linkLocation == 2)
                            current.Right = null;
                    }
                    return currRoot;
                }
                // Linking nodes to new rope
                Node<T> LinkNewRope(int linkLocation)
                {
                    Node<T> newRope = currRoot;
                    // Link nodes to the right side of the new rope
                    if (linkLocation == 0)
                    {
                        if (newRope.Right == null)
                            newRope.Right = current.Left;
                        else
                        {
                            while (newRope.Right.Right != null)
                                newRope = newRope.Right;
                            newRope.Right = Concatenate(newRope.Right, current.Left);
                        }
                        current.Left = null;
                    }
                    // Link nodes to the left side of the new rope
                    else if (linkLocation == 1)
                    {
                        if (newRope.Left == null)
                            newRope.Left = current.Right;
                        else
                        {
                            while (newRope.Left.Left != null)
                                newRope = newRope.Left;
                            newRope = Concatenate(current.Right, newRope.Left);
                        }
                        current.Right = null;
                    }
                    return currRoot;
                }
            }
            Node<T> SplitRopeLeft(Node<T> current, int i, int directions, Node<T> currRoot)
            {

                Console.WriteLine("current.Length > i");
                // Rope will be split on the left side of the current node
                if (current.Length - i < 0 && current.Left != null)
                {
                    /* Getting a status update on the variables
                    Console.WriteLine("i < current.Length: ");
                    Console.Write("i: ");
                    Console.WriteLine(i);
                    Console.Write("current.Length - i: ");
                    Console.WriteLine(current.Length - i);
                    Console.Write("Current.Left: ");
                    Console.WriteLine(current.Length);*/
                    if (directions < 2)
                    {
                        if (directions == 0)
                            currRoot = LinkRoot(0);
                        currRoot = SplitRopeLeft(current.Left, i, directions, currRoot);
                    }
                    else
                    {
                        if (directions == 3)
                            currRoot = LinkRoot(2);
                        else
                            currRoot = LinkRoot(1);
                        currRoot = SplitRopeLeft(current.Left, i, 2, currRoot);
                    }
                }
                // rope will be split on the current node
                else if (current.Length - i > 0 && current.Right != null)
                {
                    /* Getting a status update on the variables
                    Console.WriteLine("i > current.Length");
                    Console.Write("i: ");
                    Console.WriteLine(i);
                    Console.Write("current.Length - i: ");
                    Console.WriteLine(current.Length - i);
                    Console.Write("Current.Left: ");
                    Console.WriteLine(current.Length);*/
                    if (directions < 2)
                    {
                        if (directions == 0)
                            currRoot = LinkRoot(1);
                        else
                            currRoot = LinkNewRope(0);
                        currRoot = SplitRopeLeft(current.Right, current.Length - i, 1, currRoot);
                    }
                    else
                    {
                        if (directions == 3)
                            currRoot = LinkRoot(3);
                        currRoot = SplitRopeLeft(current.Right, current.Length - i, directions, currRoot);
                    }
                }
                // rope will be split on the current node
                else if (current.Length - i == 0)
                {
                    /* Getting a status update on the variables
                    Console.WriteLine("i == current.Length");
                    Console.Write("i: ");
                    Console.WriteLine(i);
                    Console.Write("i - current.Length: ");
                    Console.WriteLine(i - current.Length);
                    Console.Write("Current.Left: ");
                    Console.WriteLine(current.Length);*/
                    if (directions < 2)
                    {
                        if (directions == 0)
                            currRoot = LinkRoot(1);
                        else if (directions == 1)
                            currRoot = LinkNewRope(0);
                    }
                    else
                    {
                        if (directions == 3)
                            currRoot = LinkRoot(2);
                        else if (directions == 2)
                            currRoot = LinkNewRope(1);
                    }
                }
                // rope will be split on the current node
                else
                {
                    Console.WriteLine("SplitRope else");
                    current.Left = Build(current.Item, 0, i);
                    current.Right = Build(current.Item, i, current.Length - i);
                    current.Length = current.Left.Length;
                    current.Item = "";
                    currRoot = SplitRopeLeft(current, i, directions, currRoot);
                }


                return currRoot;
                // Supporting method for split rope that links nodes to the proper root
                Node<T> LinkRoot(int linkLocation)
                {
                    // Linking left node to new root
                    if (linkLocation < 2)
                    {
                        currRoot.Left = current.Left;
                        currRoot.Length = current.Length;
                        if (linkLocation == 1)
                            current.Left = null;
                    }
                    // Linking right node to new root
                    else
                    {
                        currRoot.Right = current.Right;
                        currRoot.Length = current.Length;
                        if (linkLocation == 2)
                            current.Right = null;
                    }
                    return currRoot;
                }
                // Linking nodes to new rope
                Node<T> LinkNewRope(int linkLocation)
                {
                    Node<T> newRope = currRoot;
                    // Link nodes to the right side of the new rope
                    if (linkLocation == 0)
                    {
                        if (newRope.Right == null)
                            newRope.Right = current.Left;
                        else
                        {
                            while (newRope.Right.Right != null)
                                newRope = newRope.Right;
                            newRope.Right = Concatenate(newRope.Right, current.Left);
                        }
                        current.Left = null;
                    }
                    // Link nodes to the left side of the new rope
                    else if (linkLocation == 1)
                    {
                        if (newRope.Left == null)
                            newRope.Left = current.Right;
                        else
                        {
                            while (newRope.Left.Left != null)
                                newRope = newRope.Left;
                            newRope = Concatenate(current.Right, newRope.Left);
                        }
                        current.Right = null;
                    }
                    return currRoot;
                }
            }
        }


        //Rebalance Method
        //Rebalance the rope using the algorithm found on pages 1319-1320 of Boehm et al. (9 marks).
        //Note: Will be switched to private once done testing
        public Node<T> Rebalance()
        {
            int nodeDepth = 0;
            //int[] fibSeq = [1, 1, 2, 3, 5, 8, 13, 21, 34, 55];
            Node<T>[] minLength = new Node<T>[32];


            Console.WriteLine(minLength);

            //If the root node has no children, the tree is balanced
            if ((root.Left == null) && (root.Right == null))
            {
                Console.WriteLine("Rope is balanced");
                return root;
            }


            if (root.Left != null)
            {
                Rebalance(root.Left, nodeDepth + 1, minLength);
            }

            if (root.Right != null)
            {
                Rebalance(root.Right, nodeDepth + 1, minLength);
            }



            return root;
        }

        //Calls Rebalance() recursively
        private void Rebalance(Node<T> curr, int nodeDepth, Node<T>[] minLength)
        {
            //Maybe increase the size of minLength here as the depth increases

            //If the node is a leaf node, insert the node into the appropriate sequence position
            if (curr.Length <= MAX_LENGTH)
            {
                //If there is no node currenlty in the sequence position, add it to that position
                if (minLength[curr.Length] != null)
                {
                    minLength[curr.Length] = curr; //Adding the current node to the sequence
                }

                //If there is a node in the position, concatenate it with the node at that position.
                else
                {
                    Node<T> next = Concatenate(curr, minLength[curr.Length]);
                    //minLength[curr.Length] = new Node<T>; //Setting the position the node was at to null
                    Rebalance(next, nodeDepth, minLength);
                }

            }

            //If the node is not a leaf node, keep going down
            if (curr.Left != null)
            {
                Rebalance(curr.Left, nodeDepth + 1, minLength);
            }

            if (curr.Right != null)
            {
                Rebalance(curr.Right, nodeDepth + 1, minLength);
            }

        }


    }

    // NOTES 
    // My code will favour putting the bigger substring in the right subtree, whereas his example favours the left, I don't think it matters
    // I just stored the text file on my desktop, but I've included the text I used, it's from the assignment
    public class Program
    {
        static void Main(string[] args)
        {
            // TEST CASES
            //Console.WriteLine("Rope 1: 6 char");
            //String s = "abcdef";
            //Rope<string> rope = new Rope<string>(s);
            //rope.PrintRope();

            //Console.WriteLine("\nRope 2: 7 char");
            //s = "abcdefg";
            //rope = new Rope<string>(s);
            //rope.PrintRope();

            //Console.WriteLine("\nRope 3: 8 char");
            //s = "abcdefgh";
            //rope = new Rope<string>(s);
            //rope.PrintRope();

            //Console.WriteLine("Rope 3: 27 char");
            //string s = "This is an easy assignment.";
            //Rope<string> rope = new Rope<string>(s);
            //rope.PrintRope();

            //Console.WriteLine("Rope 4: 20 char");
            string s = "I am currently coding a program using --";
            Rope<string> rope = new Rope<string>(s);
            rope.PrintRope();
            Console.WriteLine();
            Console.WriteLine("Printing Rope as a String");
            Console.WriteLine(rope.ToString());
            Console.WriteLine();
            Console.WriteLine("Spliting the Rope");
            rope.SplitRope(15);

            //Reading in the file

            StreamReader reader = new StreamReader("../../../test.txt");
            s = "";
            string line = "";

            while ((line = reader.ReadLine()) != null)
            {
                s += " " + line;
            }


            Rope<string> ropeFile = new Rope<string>(s);
            //ropeFile.PrintRope();

            Console.ReadLine();
            reader.Close();
        }
    }
