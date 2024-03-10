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
            int size = root.Left.Length;
            Node<T> temp = new Node<T>("", 0, null, null);
            Node<T> splitTemp = new Node<T>("", 0, null, null);
            Rope<T> R1 = new Rope<T>(S);
            if (i > root.Length)
                Console.WriteLine("This string cannot be added since you are trying to add it past the end of the other string.");
            else if (i == 0)
                root = Concatenate(R1.root, root);
            else if (i == root.Length)
                root = Concatenate(root, R1.root);
            else
            {
                splitTemp = Split(root, i);
                if (i < size)
                {
                    temp = Concatenate(splitTemp, R1.root);
                    temp.Length = splitTemp.Length + R1.root.Length;
                    root = Concatenate(temp, root);
                }
                else if (i >= size)
                {
                    temp = Concatenate(splitTemp, R1.root);
                    temp.Length = splitTemp.Length + R1.root.Length;
                    root = Concatenate(temp, root);
                }
            }
            Rebalance();
            Console.WriteLine("String was successfully inserted!");
        }

        //Delete Method
        //Delete the substring S[i, j] (5 marks).
        public void Delete(int i, int j)
        {
            // create a copy of root before split's for reference later
            Node<T> original = new Node<T>(root.Item, root.Length, root.Left, root.Right);
    
            // case where user wants to chop off a front portion of the rope
            if (i == 0)
            {
                Split(root, j);
            }
    
            // case where user wants to chop off a back portion of the rope
            else if (j == root.Length)
            {
                root = Split(root, i - 1);
            }
    
            // case for when the user wants to remove from somehwere in the middle of the rope
            else
            {
                // if split occurs on the left side of the tree
                if (i > root.Left.Length)
                {
                    Rope<T> R1 = new Rope<T>(this.ToString());  //duplicate the current rope 
                    Node<T> segment1 = Split(root, j);          // call split a store the result to set up the rope for the following operations
                    Node<T> keep = new Node<T>(root.Item, root.Length, root.Left, root.Right);      // stores the current split version (second half) before the next split
                    root = new Node<T>(original.Item, original.Length, original.Left, original.Right);  // restore the root to its state before a split occurred
                    Node<T> segment2 = Split(R1.root, i); // using the restored root, split root yo obtain the first segement of the result 
                    root = Concatenate(segment2, keep); // concatenate the first and second halves together to get the result
                }
                // if split occurs on the right side  of the tree
                else if (i <= root.Left.Length)
                {
                    // similar approach as left
                    Node<T> segment1 = Split(root, j);
                    Node<T> keep = root;
                    root = new Node<T>(original.Item, original.Length, original.Left, original.Right);
                    Node<T> segment2 = Split(original, i);
                    root = Concatenate(segment2, keep);
                }
            }
    
            Rebalance();
            combineSibs(root);
            Console.WriteLine("String was successfully deleted!");
        }

        //Optimization: Combine Siblings
        // When there are 2 nodes whose combined length < 5, this method will combine them into 1
        public Node<T> combineSibs(Node<T> parent)
        {
            //if both children are not null
            if (parent.Left != null && parent.Right != null)
            {
                // condition needing combination is found
                if (parent.Length <= 5)
                {
                    // create a new node with the contents of both nodes
                    Node<T> youngest = new Node<T>((parent.Left.Item + parent.Right.Item), (parent.Left.Length + parent.Right.Length), null, null);
                    // assign this new node to the left child of the parent
                    parent.Left = youngest;
                    // update right child to be null
                    parent.Right = null;
                }
                else
                {
                    // continue searching
                    parent.Left = combineSibs(parent.Left);
                    parent.Right = combineSibs(parent.Right);
                }
            }

            return parent;
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
                    i = j - subLength + 1 + subString.Length;
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

            void Find(Node<T> current, ref string S)
            {
                if (!found)
                {
                    if (current.Left != null)
                        Find(current.Left, ref S);
                    if (S.Length > 0)
                        if (current.Item.Contains(S[0]))
                        {
                            if (buffer.Length == 0)
                                i = current.Item.IndexOf(S[0]) + rIndex;
                            for (int j = current.Item.IndexOf(S[0]); j < current.Item.Length && sIndex < S.Length; j++)
                            {
                                if (current.Item[j] == S[sIndex])
                                {
                                    buffer += S[sIndex];
                                    sIndex++;
                                }
                                else
                                {
                                    found = false;
                                    buffer = "";
                                    sIndex = 0;
                                    i = -1;
                                }
                            }
                            S = S.Substring(sIndex);
                            sIndex = 0;
                            if (S.Length == 0)
                                found = true;
                        }
                    rIndex += current.Item.Length;
                    if (current.Right != null && S.Length > 0)
                        Find(current.Right, ref S);
                }
            }
            if (!found)
            {
                Console.WriteLine("The substring was not found.");
                return -1;
            }

            Console.WriteLine("The substring was found!");
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
            if (p.Length - 1 < i)
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
                else if (p.Length / 2 <= strIndex)
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
                    if (current.Item.Contains(c))
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
        //Public version: Reverse the string by swapping children recursivley
        public void Reverse()
        {
            Reverse(root);

            //If the rope is empty, print an error message
            if (root.Length == 0)
            {
                Console.WriteLine("The rope does not have any characters to reverse.");
            }
            else
            {
                Console.WriteLine("The rope was successfully reveresed!");
            }

        }
        
        //Reverse Method
        //Private version: Reverse the string by swapping children recursivley
        private Node<T> Reverse(Node<T> parent)
        {
            //If the rope is empty, return immediately
            if (root.Length == 0)
            {
                return root;
            }
            // store the current parents state before the following operations
            Node<T> temp = new Node<T>(parent.Item, parent.Length, parent.Left, parent.Right);

            // if the current node is a parent 
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
                // keep searching
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
            Node<T> rightRoot = new Node<T>("", 0, null, null);
            if (i > p.Length || i == 0)
                Console.WriteLine("The rope cannot be split at this location");
                

            // Trying to split the rope on the right side of the root
            else if (i > p.Left.Length)
            {
                // Rope will be split on the left side of the current node
                if (i - p.Right.Length < p.Right.Length)
                {
                    rightRoot.Left = root.Left;
                    rightRoot.Right = SplitRope(p.Right, i - p.Right.Length, 0, new Node<T>("", 0, null, null));
                    rightRoot.Length = rightRoot.Left.Length;
                    root.Left = null;
                }
                // Rope will be split on the right side of the current node
                else if (i - p.Right.Length > p.Right.Length)
                {
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
                // Rope will be split on the left side of the current node
                if (p.Left.Length - i < p.Left.Length)
                    rightRoot.Left = SplitRope(p.Left, i, 0, new Node<T>("", 0, null, null));
                // rope will be split on the right side of the current node
                else if (i - p.Left.Length > p.Left.Length)
                    rightRoot.Left = SplitRope(p.Left, i, 3, new Node<T>("", 0, null, null));
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
            // if split occurs at p
            else if (i == p.Left.Length)
            {
                Console.WriteLine("i - p.Left.Length == 0");
                if (p.Left.Left != null && p.Left.Right != null)
                {
                    rightRoot.Left = p.Left.Left;
                    rightRoot.Right = p.Left.Right;
                    rightRoot.Length = p.Length;
                    p.Left = null;
                }
                else
                {
                    rightRoot.Left = p.Left;
                    rightRoot.Length = p.Length;
                    p.Left = null;
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
                if (current.Left != null && current.Length - i > current.Left.Length)
                {
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
            //PrintRope(currRoot, 0);
            Node<T> temp = new Node<T>("", 0, null, null);
            if (currRoot != null)
            {
                //PrintRope(currRoot, 0);
                // If there is a node on the left
                if (currRoot.Left != null && currRoot.Left.Length != 0)
                {
                    // If there is a node on the right
                    if (currRoot.Right != null && currRoot.Right.Length != 0)
                    {
                        compressRope(currRoot.Left);
                        compressRope(currRoot.Right);
                    }
                    // If the right node is empty
                    else
                    {
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
                            temp = currRoot.Left;
                            currRoot.Left = null;
                            currRoot.Right = null;
                            currRoot.Length = temp.Length;
                            currRoot.Item = temp.Item;
                        }
                    }
                }
                // If there is a node on the right side
                else if (currRoot.Right != null && currRoot.Right.Length != null)
                {
                    // If there is a node on the left side
                    if (currRoot.Left != null && currRoot.Left.Length != null)
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
                else if (currRoot.Right == null && currRoot.Left == null)
                    return;
                if ((currRoot.Right != null && currRoot.Length == currRoot.Right.Length) || (currRoot.Left != null && currRoot.Length == currRoot.Left.Length))
                    compressRope(currRoot);
            }
        }



        //Rebalance Method
        //Rebalance the rope using the algorithm found on pages 1319-1320 of Boehm et al. (9 marks).
        private Node<T> Rebalance()
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

            root = branch1; //Changing the root to the newly modified tree
            combineSibs(root);
            return root;
        }

        //Rebalance II
        //Calls itself recursively to move through the tree
        private void Rebalance(Node<T> curr, List<int> fibSeq, Node<T>[] minLength)
        {
            bool added = false, altCase = false;
            Node<T> conNode;
            int j;

            //If the node is a leaf node, insert the node into the appropriate position
            if (curr.Left == null && curr.Right == null)
            {
                //Moving through the array until the length of the string fits in the interval
                for (int i = 0; curr.Length < fibSeq[i]; i++)
                {
                    //If the node belongs in the interval
                    if (curr.Length < fibSeq[i] && curr.Length >= fibSeq[i + 1] && added == false)
                    {
                        //Checking to see if that position in the minLength array is empty
                        for (j = fibSeq.Count - 1; j > i; j--)
                        {
                            //If there is a node in the smaller numbers of the fibonnaci sequence, concatenate the two nodes together
                            if (minLength[j] != null)
                            {
                                //If the two strings can be compressed into 1 (both leaf nodes with a summed length less than 10)
                                if (curr.Left == null && curr.Right == null && minLength[j].Left == null && minLength[j].Right == null && (curr.Length + minLength[j].Length <= 10))
                                {
                                    curr.Length = curr.Length + minLength[j].Length;
                                    curr.Item = String.Concat(minLength[j].Item, curr.Item);
                                    minLength[j] = null;
                                    //Resetting i and j to go through the loops again
                                    i = -1;
                                    j = -1;
                                }

                                else
                                {
                                    conNode = Concatenate(minLength[j], curr);
                                    minLength[j] = null; //Set it to null after moving it

                                    //If the concatenation causes the length to increase over the interval, call rebalance to put it in the correct position
                                    if ((fibSeq[i] <= conNode.Length && fibSeq[i - 1] > conNode.Length) || minLength[j - 1] != null)
                                    {
                                        //Rebalance(conNode, fibSeq, minLength);
                                        curr = conNode;


                                        //Resetting i and j to go through the loops again
                                        i = -1;
                                        j = -1;

                                    }
                                    //If not, put it in the orignal spot
                                    else
                                    {
                                        minLength[i + 1] = conNode;
                                        added = true;
                                    }
                                }

                            }
                        }

                        //If not, directly add the node to minLength
                        if (added == false && j >= 0)
                        {
                            minLength[i + 1] = curr;
                            added = true;
                        }

                    }
                }


            }

            //If the node is not a leaf node, keep going down
            if (curr.Left != null && altCase != true && added != true)
            {
                Rebalance(curr.Left, fibSeq, minLength);
            }

            //Once done going down the left, go down the right
            if (curr.Right != null && altCase != true && added != true)
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
            // TEST CASES //
            //string s = ""; //Empty string test case
            //string s = "0123456789"; //String of length 10 test case 
            //string s = "I"; //String of length 1 test case
            string s = "I am currently coding a program using --"; //String of length greater than 10 test case
            Rope<string> rope = new Rope<string>(s);
            rope.PrintRope();
            Console.WriteLine();

            // Testing Insert //
            //rope.Insert("new", 0);
            //rope.Insert("new", 40);
            //rope.Insert("new", 12);
            //rope.Insert("new", 213);
            //rope.PrintRope();

            // Testing Delete //
            //rope.Delete(0,4);
            //rope.Delete(10, 20);
            rope.Delete(21, 33);
            //rope.Delete(0, 39);
            //rope.Delete(-1, 123);
            //rope.PrintRope();

            // Testing Substring //
            //Console.WriteLine("The substring from 0 to 5 is: {0}", rope.Substring(0, 5));
            //Console.WriteLine("The substring from 9 to 30 is: {0}", rope.Substring(9, 30));
            //Console.WriteLine("The substring from 30 to 39 is: {0}", rope.Substring(30, 39));
            //Console.WriteLine("The substring from 0 to 39 is: {0}", rope.Substring(0, 39));
            //Console.WriteLine("The substring from -1 to 123 is: {0}", rope.Substring(-1, 123));

            // Testing Find //
            //Console.WriteLine("Index of the first occurence of 'I am': {0}", rope.Find("I am"));
            //Console.WriteLine("Index of the first occurence of '--': {0}", rope.Find("--"));
            //Console.WriteLine("Index of the first occurence of 'prog': {0}", rope.Find("prog"));
            //Console.WriteLine("Index of the first occurence of 'in': {0}", rope.Find("in"));
            //Console.WriteLine("Index of the first occurence of 'I am currently coding a program using --': {0}", rope.Find("I am currently coding a program using --"));
            //Console.WriteLine("Index of the first occurence of 'hello': {0}", rope.Find("hello"));

            // Testing CharAt //
            //Console.WriteLine("Character at index 0: {0}", rope.CharAt(0));
            //Console.WriteLine("Character at index 39: {0}", rope.CharAt(39));
            //Console.WriteLine("Character at index 22: {0}", rope.CharAt(22));
            //Console.WriteLine("Character at index 123: {0}", rope.CharAt(123));

            // Testing IndexOf //
            //Console.WriteLine("Index of the first occurence of 'I': {0}", rope.IndexOf('I'));
            //Console.WriteLine("Index of the first occurence of '-': {0}", rope.IndexOf('-'));
            //Console.WriteLine("Index of the first occurence of 'i': {0}", rope.IndexOf('i'));
            //Console.WriteLine("Index of the first occurence of 'c': {0}", rope.IndexOf('c'));
            //Console.WriteLine("Index of the first occurence of 'z': {0}", rope.IndexOf('z'));

            // Testing Reverse //
            //rope.PrintRope();
            //rope.Reverse();
            //rope.PrintRope();
            //rope.Reverse();
            //rope.PrintRope();

            // Testing Length //
            //rope.PrintRope();
            //Console.WriteLine("The length of the rope is {0}.", rope.Length());

            // Testing ToString //
            //Console.WriteLine("The entire string of the rope is: {0}", rope.ToString());

            // Testing PrintRope //
            //rope.PrintRope();


            Console.ReadLine();
        }
    }
