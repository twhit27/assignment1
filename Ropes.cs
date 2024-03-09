/*
 * Assignment 2: Ropes
 * Jamie Le Neve, Camryn Moerchen, Victoria Whitworth
 * COIS 3020H
 * Brian Patrick
 * March 10, 2024
 */

using System;
using System.IO;
using System.Collections.Generic;

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
                current.Length = current.Left.Length + current.Right.Length;
            }
            Console.WriteLine("The rope was successfully created!");
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
            String subString = ""; //Declaring an empty string to build the substring in
            int subLength = j - i + 1; //Calculating the resulting length of the substring
    
    
            //Checking if indices are valid
            //If they are not, print an error message and return an empty string
            if (i < 0 || i >= root.Length || j < i || j >= root.Length)
            {
                Console.WriteLine("Invalid indices, substring could not be found.");
                return "";
            }
    
            subString = Substring(i, j, subLength, root, subString);
            Console.WriteLine("The substring was successfully found!");
            return subString;
        }
    
        //Private Substring Method
        //Used to traverse the rope and concatenate the strings at each node according to the given indices
        private string Substring(int i, int j, int subLength, Node<T> curr, string subString)
        {
    
            //If a leaf node has not been reached yet, traverse
            if (curr.Left != null && curr.Right != null)
            {
                //If the index is less than or equal to the length of the left side, go left
                if (i <= curr.Left.Length - 1)
                {
                    subString = Substring(i, j, subLength, curr.Left, subString);
                }
    
    
                //If the index is greater than the length of the left side, go right
                else
                {
                    i -= curr.Left.Length; //Decrementing the index i based on the length of the left branch
                    subString = Substring(i, j, subLength, curr.Right, subString);
                }
            }
    
            //Once the string is found, start concatenating and return the substring created so far
            else
            {
                while (i < curr.Length && subString.Length < subLength)
                {
                    subString = String.Concat(subString, curr.Item[i]);
                    i++;
                }
    
                return subString;
            }
    
            //If there are still nodes left to explore, keep going
            if (subString.Length < subLength && curr.Right != null)
            {
                //Evaluating the updated value of i depending on how close it is to the end (j)
                if (i + subString.Length < j)
                {
                    i = j - subLength + 1  + subString.Length;
                }
                else
                {
                    i = j;
                }
                
                subString = Substring(i, j, subLength, root, subString);
            }
    
            return subString;
    
        }

        //Find Method
        //Return the index of the first occurrence of S; -1 otherwise (9 marks).
        public int Find(string S)
        {
            int i = -1;
            int rIndex = 0;
            int sIndex = 0;
            string buffer = "";
            bool found = false;

            if (root != null && S != null)
                Find(root, ref S);

            void Find(Node<T> current, ref string s)
            {
                if (!found)
                {
                    if (current.Left != null)
                        Find(current.Left, ref s);
                    if (s.Length > 0)
                        if (current.Item.Contains(s[0]))
                        {
                            if (buffer.Length == 0)
                                i = current.Item.IndexOf(s[0]) + rIndex;
                            for (int j = current.Item.IndexOf(s[0]); j < current.Item.Length && sIndex < s.Length; j++)
                            {
                                if (current.Item[j] == s[sIndex])
                                {
                                    buffer += s[sIndex];
                                    sIndex++;
                                }
                                else
                                {
                                    buffer = "";
                                    sIndex = 0;
                                    i = -1;
                                }
                            }
                            s = s.Substring(sIndex);
                            sIndex = 0;
                            if (s.Length == 0)
                                found = true;
                        }
                    rIndex += current.Item.Length;
                    i += current.Item.Length;
                    if (current.Right != null && s.Length > 0)
                        Find(current.Right, ref s);
                }
            }
            if (!found){
                Console.WriteLine("The substring at the given index was not found.");   
                i = -1;
            }
                
            Console.WriteLine("The substring at the given index was found!");    
            return i;
        }

        //CharAt Method
        //Return the character at index i (3 marks).
        //Inspired by Brian Patrick's augmented treap rank method
        public char CharAt(int i)
        {
            Node<T> p = root;
            bool found = false;
    
            int strIndex = i; //Index of the character within the tree
    
            //Checking that the index given is less than the root
            //If the index is not less than the root's length, the character does not exist in the rope
            if (p.Length-1 < i)
            {
                Console.WriteLine("The index does not exist in the rope.");
                return ' ';
            }
                
    
            //Move through the tree until the leaf node containing the character is found
            while (!found)
            {
                //Once at the leaf node containing the character, set found to true
                if (p.Right == null && p.Left == null)
                    found = true;
    
                //If the index is greater than half of the length of the current node, move right
                else if (p.Length/2 <= strIndex) 
                {
                    p = p.Right;              
                    strIndex -= p.Length; //Adjust the index to account for moving down the rope
                }
                //If not, move down the left
                else
                    p = p.Left;
        
            }
            Console.WriteLine("The character was successfully found!");
            return p.Item[strIndex]; //Return the character at the given index     
        }

        //IndexOf Method
        //Return the index of the first occurrence of character c (4 marks).
        public int IndexOf(char c)
        {
            int i = -1;
            int index = 0;
            bool found = false;
            if (root != null)
                IndexOf(root, c);

            void IndexOf(Node<T> current, char c)
            {
                if (!found)
                {
                    if (current.Left != null)
                        IndexOf(current.Left, c);
                    if (root.Item.Contains(c))
                    {
                        found = true;
                        i = current.Item.IndexOf(c) + index;
                    }
                    index += current.Item.Length;
                    if (current.Right != null)
                        IndexOf(current.Right, c);
                }
            }

            return i;
        }


        //Reverse Method
        //Reverse the string by swapping children recursivley
        public void Reverse()
        {
            Reverse(root);
            Console.WriteLine("The rope was successfully reveresed!"); 
        }

        private Node<T> Reverse(Node<T> parent)
        {
            Node<T> temp = new Node<T>(parent.Item, parent.Length, parent.Left, parent.Right);

            if (parent.Item != "")
            {
                //reverse string approach from https://www.educative.io/answers/how-to-reverse-a-string-in-c-sharp
                char[] sArray = temp.Item.ToCharArray();
                Array.Reverse(sArray);
                string reversed = new string(sArray);
                parent.Item = reversed;
                return parent;
            }
            else
            {
                parent.Left = Reverse(temp.Right);
                parent.Right = Reverse(temp.Left);
            }

            return parent;
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
            if (root != null)
            {
                //If the rope only has one node in it, only print out the content for that node
                if (root.Left == null && root.Right == null)
                {
                    Console.WriteLine(root.Length + "   " + root.Item);
                }
                else
                {
                    PrintRope(root, 0); //Calls the private print rope method, giving the root of the rope
                }
            }


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
                current.Length = sub.Length;
                current.Item = sub;
            }
            // this block handles parent nodes and drives the recursion
            // essentially, it will continously split the string into 2 until the sections are at or below max node length
            else
            {
                int mid = sub.Length / 2;   //mid will provide the middle point value that allows the following code to split the string in half
                current.Left = Build(sub, 0, mid);    // create the left subtree by using new indexes to pass the first half of the substring
                current.Right = Build(sub, mid, sub.Length - mid);   // create the right subtree by using indexes to pass the second half
                current.Length = current.Left.Length + current.Right.Length; // update parent node length accordingly
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
            PrintRope();
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
                    rightRoot.Right = SplitRope(p.Right, i - p.Right.Length, 0, new Node<T>("", 0, null, null));
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
                    rightRoot.Right = SplitRope(p.Right, i - p.Right.Length, 3, new Node<T>("", 0, null, null));
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
                //Console.WriteLine("i < p.Left.Length");
                // Rope will be split on the left side of the current node
                if (p.Left.Length - i < p.Left.Length)
                {
                    //Console.WriteLine("p.Left.Length - i < p.Left.Length");
                    /* Getting a status update on the variables
                    Console.Write("i: ");
                    Console.WriteLine(i);
                    Console.Write("p.Left - i: ");
                    Console.WriteLine(p.Left.Length - i);
                    Console.Write("p.Left: ");
                    Console.WriteLine(p.Left.Length);*/

                    rightRoot.Left = SplitRope(p.Left, i, 0, new Node<T>("", 0, null, null));
                }
                // rope will be split on the right side of the current node
                else if (i - p.Left.Length > p.Left.Length)
                {
                    //Console.WriteLine("i - p.Left.Length > p.Left.Length");
                    /* Getting a status update on the variables
                    Console.Write("i: ");
                    Console.WriteLine(i);
                    Console.Write("p.Left - i: ");
                    Console.WriteLine(p.Left.Length - i);
                    Console.Write("p.Left: ");
                    Console.WriteLine(p.Left.Length);*/
                    rightRoot.Left = SplitRope(p.Left, i, 3, new Node<T>("", 0, null, null));
                }
                // rope will be split on the current node
                else if (i - p.Left.Length == 0)
                {
                    //Console.WriteLine("i - p.Left.Length == 0");
                    rightRoot.Left = p.Left.Right;
                    rightRoot.Length = p.Length - p.Left.Length;
                    p.Left.Right = null;
                    p.Length = p.Left.Length;
                    p = p.Left;
                }
            }
            ReassignLength(rightRoot);
            compressRope(rightRoot);
            ReassignLength(root);
            compressRope(root);
            return rightRoot;

            Node<T> SplitRope(Node<T> current, int i, int directions, Node<T> currRoot)
            {
                //Console.WriteLine("current.Length > i");
                // Rope will be split on the left side of the current node
                if (current.Length - i > MAX_LENGTH && current.Left != null)
                {
                    /* Getting a status update on the variables
                    Console.WriteLine("i > current.Length: ");
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
                        currRoot = SplitRope(current.Left, i, directions, currRoot);
                    }
                    else
                    {
                        if (directions == 3)
                            currRoot = LinkRoot(2);
                        else
                            currRoot = LinkRoot(1);
                        currRoot = SplitRope(current.Left, i, 2, currRoot);
                    }
                }
                // rope will be split on the left side of the current node
                else if (current.Length - i > 0 && current.Right != null)
                {
                    /* Getting a status update on the variables
                    Console.WriteLine("i < current.Length");
                    Console.Write("i: ");
                    Console.WriteLine(i);
                    Console.Write("current.Length - i: ");
                    Console.WriteLine(current.Length - i);
                    Console.Write("Current.Left: ");
                    Console.WriteLine(current.Length);*/
                    //Console.WriteLine("Current:");
                    //PrintRope(current, 0);
                    if (directions < 2)
                    {
                        if (directions == 0)
                            currRoot = LinkRoot(1);
                        else
                            currRoot = LinkNewRope(0);
                        currRoot = SplitRope(current.Right, i - MAX_LENGTH, 1, currRoot);
                    }
                    else
                    {
                        if (directions == 3)
                            currRoot = LinkRoot(3);
                        currRoot = SplitRope(current.Right, i - MAX_LENGTH, directions, currRoot);
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
                    /*Console.WriteLine("i == current.Length");
                    Console.Write("i: ");
                    Console.WriteLine(i);
                    Console.WriteLine("SplitRope else");*/
                    current.Left = Build(current.Item, 0, i);
                    current.Right = Build(current.Item, i, current.Length - i);
                    current.Length = current.Left.Length;
                    current.Item = "";
                    currRoot = SplitRope(current, i, directions, currRoot);
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

        // Helper method for the split method to recalculate the lengths of the nodes after the rope is split 
        private void ReassignLength(Node<T> currRoot)
        {
            if (currRoot != null)
            {
                // If there is a node on the left
                if (currRoot.Left != null)
                {
                    // If there is a node on the right
                    if (currRoot.Right != null)
                    {
                        ReassignLength(currRoot.Left);
                        ReassignLength(currRoot.Right);
                        // Update the length of the parent node
                        currRoot.Length = currRoot.Left.Length + currRoot.Right.Length;
                    }
                    // If there is no right node
                    else
                    {
                        ReassignLength(currRoot.Left);
                        // Updating the length of parent node
                        currRoot.Length = currRoot.Left.Length;
                    }
                }
                // If there is a node on the right 
                else if (currRoot.Right != null)
                {
                    // If there is a node on the left
                    if (currRoot.Left != null)
                    {
                        ReassignLength(currRoot.Right);
                        ReassignLength(currRoot.Left);
                        currRoot.Length = currRoot.Left.Length + currRoot.Right.Length;
                    }
                    // If there is no node on the left
                    else
                    {
                        ReassignLength(currRoot.Right);
                        // Update the length of the parent node
                        currRoot.Length = currRoot.Right.Length;
                    }
                }
            }
        }

        // Additional Optimizations 1
        // After a split, compress the path back to the root to ensure that binary tree is full, i.e. each non-leaf node has two non-empty children
        private void compressRope(Node<T> currRoot)
        {
            
            Node<T> temp = new Node<T>("", 0, null, null);
            if (currRoot != null)
            {
                //PrintRope(currRoot, 0);
                // If there is a node on the left
                if (currRoot.Left != null)
                {
                    Console.WriteLine(currRoot.Left.Left == null);
                    // If there is a node on the right
                    if (currRoot.Right != null)
                    {
                        compressRope(currRoot.Left);
                        compressRope(currRoot.Right);
                    }
                    // If the right node is empty
                    else
                    {
                        Console.WriteLine("Hi_0");
                        // If the left node has two non-empty chidlren
                        if (currRoot.Left.Left != null && currRoot.Left.Left.Length != 0 && currRoot.Left.Right != null && currRoot.Left.Right.Length != 0)
                        {
                            temp = currRoot.Left;
                            currRoot.Right = currRoot.Left.Right;
                            currRoot.Left = currRoot.Left.Left;
                            currRoot.Length = temp.Length;
                            currRoot.Item = temp.Item;
                            compressRope(currRoot.Right);
                            compressRope(currRoot.Left);
                        }
                        else if (currRoot.Left.Left != null && currRoot.Left.Left.Length != 0)
                        {
                            temp = currRoot.Left;
                            currRoot.Left = currRoot.Left.Left;
                            currRoot.Length = temp.Length;
                            currRoot.Item = temp.Item;
                            compressRope(currRoot.Left);
                        }
                        else if (currRoot.Left.Right != null && currRoot.Left.Right.Length != 0)
                        {
                            temp = currRoot.Left;
                            currRoot.Right = currRoot.Left.Right;
                            currRoot.Length = temp.Length;
                            currRoot.Item = temp.Item;
                            compressRope(currRoot.Right);
                        }
                        // If the left node has an empty child
                        else if (currRoot.Left.Left == null)
                        {
                            Console.WriteLine("Hi_1");
                            temp = currRoot.Left;
                            currRoot.Left = null;
                            currRoot.Right = null;
                            currRoot.Length = temp.Length;
                            currRoot.Item = temp.Item;
                        }
                    }
                }
                // If there is a node on the right side
                else if (currRoot.Right != null)
                {
                    // If there is a node on the left side
                    if (currRoot.Left != null)
                    {
                        compressRope(currRoot.Right);
                        compressRope(currRoot.Left);
                    }
                    // If there is an empty child node
                    else
                    {
                        // If the right node has two non-empty children
                        if (currRoot.Right.Right != null && currRoot.Right.Right.Length != 0 && currRoot.Right.Left != null && currRoot.Right.Left.Length != 0)
                        {
                            temp = currRoot.Right;
                            currRoot.Left = currRoot.Right.Left;
                            currRoot.Right = currRoot.Right.Right;
                            currRoot.Length = temp.Length;
                            currRoot.Item = temp.Item;
                            compressRope(currRoot.Right);
                            compressRope(currRoot.Left);
                        }
                        else if (currRoot.Right.Right != null && currRoot.Right.Right.Length != 0)
                        {
                            temp = currRoot.Right;
                            currRoot.Right = currRoot.Right.Right;
                            currRoot.Length = temp.Length;
                            currRoot.Item = temp.Item;
                            compressRope(currRoot.Right);
                        }
                        else if (currRoot.Right.Left != null && currRoot.Right.Left.Length != 0)
                        {
                            temp = currRoot.Right;
                            currRoot.Left = currRoot.Right.Left;
                            currRoot.Length = temp.Length;
                            currRoot.Item = temp.Item;
                            compressRope(currRoot.Right);
                        }
                        // If the right node has an empty child
                        else if (currRoot.Right.Right == null)
                        {
                            temp = currRoot.Right;
                            currRoot.Left = null;
                            currRoot.Right = null;
                            currRoot.Length = temp.Length;
                            currRoot.Item = temp.Item;
                        }
                    }
                }
                if ((currRoot.Right != null && currRoot.Length == currRoot.Right.Length) || (currRoot.Left != null && currRoot.Length == currRoot.Left.Length))
                    compressRope(currRoot);
            }
        }



        //Rebalance Method
        //Rebalance the rope using the algorithm found on pages 1319-1320 of Boehm et al. (9 marks).
        //Note: Will be switched to private once done testing
        public Node<T> Rebalance()
        {
            List<int> fibSeq = new List<int> { 1, 2 };
            Node<T>[] minLength;
            Node<T> branch1 = new Node<T>("", 0, null, null);

            //Building the fibonnaci sequence up until the total length of the rope
            for (int i = 1; fibSeq[i] <= root.Length; i++)
            {
                fibSeq.Add(fibSeq[i] + fibSeq[i - 1]);
            }

            // Reversing the list to match the paper's implementation better
            fibSeq.Reverse();

            minLength = new Node<T>[fibSeq.Count]; //Creating an array of nodes the size of the sequence to store nodes in

            //If the root node has no children, the tree is balanced
            if ((root.Left == null) && (root.Right == null))
            {
                return root;
            }



            //Move through the tree until you get through all the leaves, starting on the left
            if (root.Left != null)
            {
                Rebalance(root.Left, fibSeq, minLength);
            }

            if (root.Right != null)
            {
                Rebalance(root.Right, fibSeq, minLength);
            }

            //Once you've finished adding all the nodes to minLength, concatenate them all together into one tree

            for (int n = 0; n < minLength.Length; n++)
            {
                if (minLength[n] != null)
                {
                    if (minLength[n].Right == null && minLength[n].Left == null)
                    {
                        int strLength = minLength[n].Length;
                        int middle = Convert.ToInt32(Math.Floor(strLength / 2.0));

                        //Getting the first and second strings
                        string firstString = minLength[n].Item.Substring(0, middle);
                        string secondString = minLength[n].Item.Substring(middle);

                        //Making the nodes
                        Node<T> node1 = new Node<T>(firstString, firstString.Length, null, null);
                        Node<T> node2 = new Node<T>(secondString, secondString.Length, null, null);

                        minLength[n] = Concatenate(node1, node2);
                    }
                }

                //If a rope is found in minLength, save it
                if (minLength[n] != null && branch1.Length == 0)
                {
                    branch1 = minLength[n];
                }

                //If another rope is found in minLength, concatenate with the previously saved rope
                else if (minLength[n] != null && branch1.Length != 0)
                {
                    branch1 = Concatenate(branch1, minLength[n]);
                }

            }

            //root = Concatenate(branch1, branch2); //Changing the root to the newly modified tree

            root = branch1;
            return root;
        }

        //Rebalance II
        //Calls itself recursively to move through the tree
        private void Rebalance(Node<T> curr, List<int> fibSeq, Node<T>[] minLength)
        {
            bool added = false, altCase = false;
            Node<T> conNode;

            //If the node is a leaf node, insert the node into the appropriate position
            if (curr.Left == null && curr.Right == null)
            {
                //Moving through the array until the length of the string fits in the interval
                for (int i = 0; curr.Length < fibSeq[i]; i++)
                {
                    if (curr.Length < fibSeq[i] && curr.Length >= fibSeq[i + 1] && added == false)
                    {
                        //Checking to see if that position in the minLength array is empty
                        if (minLength[i + 1] == null)
                        {
                            for (int j = i + 2; fibSeq.Count > j; j++)
                            {
                                //If there is a node in the smaller numbers of the fibonnaci sequence, concatenate the two nodes together
                                if (minLength[j] != null)
                                {
                                    conNode = Concatenate(minLength[j], curr);
                                    minLength[j] = null; //Set it to null after moving it

                                    //If the concatenation causes the length to increase over the interval, put in the correct position
                                    if (fibSeq[i] <= conNode.Length && fibSeq[i - 1] > conNode.Length)
                                    {
                                        minLength[i] = conNode;
                                        added = true;
                                    }
                                    //If not, put it in the orignal spot
                                    else
                                    {
                                        minLength[i + 1] = conNode;
                                        added = true;
                                    }
                                }
                            }

                            //If not, directly add the node to minLength
                            if (added == false)
                            {
                                minLength[i + 1] = curr;
                                added = true;
                            }


                        }
                        //If that position is not empty, concatenate the two nodes and call rebalance again to find the proper position
                        else
                        {
                            //If the sum of the lengths is smaller than the max length, concatenate the two strings into one node
                            if (curr.Length + minLength[i + 1].Length <= 10)
                            {
                                curr.Length = curr.Length + minLength[i + 1].Length;
                                curr.Item = String.Concat(minLength[i + 1].Item, curr.Item);
                                minLength[i + 1] = null;
                                i = -1; //Move back to the start of the sequence to find the correct position
                            }

                            //If concatenating two nodes, make sure the other node is not a leaf node
                            //If it is, split it into two
                            else
                            {
                                //Only split if one of the nodes is a leaf node, and the other node is not a leaf node
                                if (curr.Right == null && curr.Left == null && minLength[i + 1].Left != null && minLength[i + 1].Right != null)
                                {
                                    int strLength = curr.Length;
                                    int middle = Convert.ToInt32(Math.Floor(strLength / 2.0));

                                    //Getting the first and second strings
                                    string firstString = curr.Item.Substring(0, middle);
                                    string secondString = curr.Item.Substring(middle);

                                    //Making the nodes
                                    Node<T> node1 = new Node<T>(firstString, firstString.Length, null, null);
                                    Node<T> node2 = new Node<T>(secondString, secondString.Length, null, null);

                                    curr = Concatenate(node1, node2);
                                }


                                curr = Concatenate(minLength[i + 1], curr);
                                minLength[i + 1] = null;
                                i = -1; //Move back to the start of the sequence to find the correct position
                                altCase = true; //Added logic to ensure the program returns back to the main rope
                            }



                        }
                    }
                }


            }

            //If the node is not a leaf node, keep going down
            if (curr.Left != null && altCase != true)
            {
                Rebalance(curr.Left, fibSeq, minLength);
            }

            //Once done going down the left, go down the right
            if (curr.Right != null && altCase != true)
            {
                Rebalance(curr.Right, fibSeq, minLength);
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
            Console.Write("Character at index 43: ");
            Console.WriteLine(rope.CharAt(43));
            Console.Write("Character at index 30: ");
            Console.WriteLine(rope.CharAt(30));
            Console.Write("Character at index 22: ");
            Console.WriteLine(rope.CharAt(22));
            Console.Write("Character at index 15: ");
            Console.WriteLine(rope.CharAt(15));
            Console.Write("Character at index 3: ");
            Console.WriteLine(rope.CharAt(3));
            Console.WriteLine("Printing Rope as a String");
            Console.WriteLine(rope.ToString());
            Console.WriteLine();
            Console.WriteLine("Spliting the Rope");
            rope.SplitRope(19);
            Rope<string> rope2 = new Rope<string>(s);
            rope2.Rebalance();
            Console.WriteLine("Printing the Rebalanced Rope");
            rope.PrintRope();
            Console.Write("Index of first occurrence of ing: ");
            Console.WriteLine(rope.Find("ing"));
            Console.Write("Index of first occurrence of c: ");
            Console.WriteLine(rope.Find("c"));
            Console.Write("Index of first occurrence of z: ");
            Console.WriteLine(rope.Find("z"));

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
