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

public class Rope
{
    private Node Root;  // Reference to the root of the Rope

    // Node Class for Ropes

    public class Node { 
        public string RopeString { get; set; }   //String contained in the node
        public int NumChars { get; set; }   //Number of characters
        public Node Left { get; set; }
        public Node Right { get; set; }

        // Node constructor
        public Node(string ropeString)
        {
            RopeString = ropeString; //Setting the item in the node as the passed item
            NumChars = ropeString.Length;
            Left = Right = null; //Setting the left and right of the node to null
        }
    }

    //Rope Constructor 
    //Create a balanced rope from a given string S (5 marks).
    public Rope(string S)
    {

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

    }

    //Build Method
    //Recursively build a balanced rope for S[i, j] and return its root (part of the constructor).
    private Node Build(string s, int i, int j)
    {
        return Root;
    }

    //Concatenate Method
    //Return the root of the rope constructed by concatenating two ropes with roots p and q (3 marks).
    private Node Concatenate(Node p, Node q)
    {
        return Root;
    }

    //Split Method
    //Split the rope with root p at index i and return the root of the right subtree (9 marks).
    private Node Split(Node p, int i)
    {
        return Root;
    }

    //Rebalance Method
    //Rebalance the rope using the algorithm found on pages 1319-1320 of Boehm et al. (9 marks).
    private Node Rebalance()
    {
        return Root;
    }
}
