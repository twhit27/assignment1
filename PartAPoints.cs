/*
 * Assignment 3 Part A: Point Me in the Right Direction
 * Jamie Le Neve, Camryn Moerchen, Victoria Whitworth
 * COIS 3020H
 * Brian Patrick
 * April 7, 2024
 */

public class PointQuadtree<T>
{

    //Node class for PointQuadtree
    public class Node<T>
    {
        public int Value { get; set; }  // Used to store the coordinate of each point in the node

        // Constructor Node
        public Node(int value)
        {
            Value = value; //Value of the point
        }
    }

    public int Dimension { get; set; }     //Dimension of the points in the tree
    public Node<T>[] Point { get; set; }    //Array of branches/nodes for each point


    //PointQuadTree Constructor
    public PointQuadtree(int[] coords)
    {

        Dimension = coords.Length;
        Point = new Node<T>[Dimension]; //Initializing the point which will contain the array of coordinates but stored them as nodes

        //Store the value of each coordinate in the point
        for (int i = 0; i < Dimension; i++)
        {
            Point[i] = new Node<T>(coords[i]);
        }

        Console.WriteLine("Point Quadtree was successfully created!");

    }


    //Maps
    //Compares two points, the point being mapped and the point in the tree, and maps the result of the comparison to an index of B.
    //If successful, return the index the point is mapped to.
    //If unsuccessful, print an error message and return the index as -1.
    public int Maps(int[] newPoint)
    {
        int index = 0;

        //Check if the point has the correct dimensions
        //If it doesn't, print an error
        if (newPoint.Length != Dimension)
        {
            Console.WriteLine("Point could not be compared because it does not have the correct dimensions.");
            return -1;
        }

        //Check if the points are equal
        //If they are, print an error
        for (int i = 0; i < Dimension; i++)
        {
            if (newPoint[i] != Point[i].Value)
            {
                i = Dimension;
            }

            else if (newPoint[i] == Point[i].Value && i == Dimension - 1)
            {
                Console.WriteLine("The point is equal to the current point.");
                return -1;
            }
        }

        //Building the binary number for the index
        for (int i = 0; i < Dimension; i++)
        {
            //If the new coordinate is greater than the coordinate, add a '1' to the binary number of the index
            if (newPoint[i] > Point[i].Value)
            {
                index += (int)Math.Pow(2, i); //Add 2^i to the index (which could be represented as '1' in a binary number)
            }
        }

        return index;
    }
}


//Main program where Map is tested
//It also contains the method printPoint for printing out the points being mapped
public class PartAPoints
{
    static void Main(string[] args)
    {
        //Method used to print out the coordinates of each point 
        //Loops through the point and prints out each coordinate with formatting
        void printPoint(int[] point)
        {
            for (int i = 0; i < point.Length; i++)
            {
                Console.Write(" " + point[i] + " ");
            }

            Console.Write(").");

            Console.WriteLine();
        }

        //Testing for 1-dimensional points
        //int[] point1 = [2];
        //int[] point2 = [1];

        //Testing for 2-dimensional points
        //int[] point1 = [2,2];
        //int[] point2 = [3,2];

        //Testing for 3-dimensional points
        int[] point1 = [2,2,9];
        int[] point2 = [2,5,12];

        //Testing for 10-dimensional points
        //int[] point1 = [8,-8,1,90,20,34,58,62,12,12];
        //int[] point2 = [1,2,2,70,90,34,44,44,100,22];

        //Printing out each coordinate in the points
        Console.Write("The point in the tree is (");
        printPoint(point1);

        Console.Write("The point being mapped is (");
        printPoint(point2);

        PointQuadtree<int> quadTree = new PointQuadtree<int>(point1);
        
        Console.WriteLine("The index is {0}.", quadTree.Maps(point2));

        Console.ReadLine();
    }
}
