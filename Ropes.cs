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
        root = Build(S, 0, S.Length);
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
    public void Reverse() { 
    
    }

    //Length Method
    //Return the length of the string (1 mark).
    public int Length()
    {
        return 0;
    }

    //ToString Method
    //Return the string represented by the current rope (4 marks).
    public string ToString()
    {
        return "string";
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
        // this if block identifies and updates leaf nodes
        if (j - i <= MAX_LENGTH)
        {
            // return a new leaf node to hook onto the parent node being passed
            string sub = s.Substring(i, j - i);
            return new Node<T>(sub, sub.Length, null, null);
        }
        // this block handles parent nodes and drives the recursion
        // essentially, it will continously split the string into 2 until the sections are at or below max node length
        else
        {
            int mid = (i + j) / 2;   //mid will provide the middle point value that allows the following code to split the string in half
            Node<T> left = Build(s, i, mid);    // create the left subtree by using new indexes to pass the first half of the substring
            Node<T> right = Build(s, mid, j);   // create the right subtree by using indexes to pass the second half
            int len = left.Length + right.Length;   // update parent node length accordingly
            return new Node<T>("", len, left, right);   // return parent node
        }

    }

    //Concatenate Method
    //Return the root of the rope constructed by concatenating two ropes with roots p and q (3 marks).
    private Node<T> Concatenate(Node<T> p, Node<T> q)
    {
        return root;
    }

    //Split Method
    //Split the rope with root p at index i and return the root of the right subtree (9 marks).
    private Node<T> Split(Node<T> p, int i)
    {
        return root;
    }

    //Rebalance Method
    //Rebalance the rope using the algorithm found on pages 1319-1320 of Boehm et al. (9 marks).
    private Node<T> Rebalance()
    {
        return root;
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

        //Reading in the file

        StreamReader reader = new StreamReader("C:\\Users\\Camry\\OneDrive\\Desktop\\test.txt");
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
