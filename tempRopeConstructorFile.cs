using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Joke of the assignment: Why did the rope get promoted? It knew how to pull strings!

namespace COIS3020Assignment2
{
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

        public override string ToString()
        {
            //it's a leaf node
            if (this.Item != null)
                return Item.ToString();
            else
                return Length.ToString();
        }
    }

    public class Rope<T>
    {
        private Node<T> root = new Node<T>("", 0, null, null);
        private const int MAX_LENGTH = 10;

        // Public Constructor
        // Updates the root by calling Build
        public Rope(string S)
        {
            root = Build(S, 0, S.Length);
        }

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
                int mid = (i + j)/ 2;   //mid will provide the middle point value that allows the following code to split the string in half
                Node<T> left = Build(s, i, mid);    // create the left subtree by using new indexes to pass the first half of the substring
                Node<T> right = Build(s, mid, j);   // create the right subtree by using indexes to pass the second half
                int len = left.Length + right.Length;   // update parent node length accordingly
                return new Node<T>("", len, left, right);   // return parent node
            }     

        }

        // Public PrintItems that will pass the root to...
        public void PrintItems()
        {
            PrintItems(root, 0);
        }

        // Private PrintItems
        // Recursively implements the public PrintItems
        // Heavily inspired by Prof. Patricks COIS 2020 Print method

        private void PrintItems(Node<T> root, int indent)
        {
            if (root != null)
            {
                PrintItems(root.Right, indent + 3);
                // Leaf nodes will not have non-empty items, so print their items 
                if (root.Item != "")
                    Console.WriteLine(new String(' ', indent) + root.Item);
                // whereas for parents I want to print their length
                else
                    Console.WriteLine(new String(' ', indent) + root.Length);
                PrintItems(root.Left, indent + 3);
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
            //Console.WriteLine("Rope 1: 6 char");
            //String s = "abcdef";
            //Rope<string> rope = new Rope<string>(s);
            //rope.PrintItems();

            //Console.WriteLine("\nRope 2: 7 char");
            //s = "abcdefg";
            //rope = new Rope<string>(s);
            //rope.PrintItems();

            //Console.WriteLine("\nRope 3: 8 char");
            //s = "abcdefgh";
            //rope = new Rope<string>(s);
            //rope.PrintItems();

            //Console.WriteLine("Rope 3: 27 char");
            string s = "This is an easy assignment.";   
            Rope<string> rope = new Rope<string>(s);
            //rope.PrintItems();

            StreamReader reader = new StreamReader("C:\\Users\\toryw\\OneDrive\\Desktop\\test.txt");
            s = "";
            string line = "";

            while ((line = reader.ReadLine()) != null)
            {
                s += " " + line;
            }

            Rope<string> ropeFile = new Rope<string>(s);
            ropeFile.PrintItems();

            Console.ReadLine();
            reader.Close();
        }
    }
}


