public class ServerGraph
{
    //WebServer constructor
    private class WebServer
    {
        public string name;
        //might not be allowed
        public List<WebPage> P = new List<WebPage>();

        public override string ToString()
        {
            return name;
        }

    }

    private WebServer[] V;
    private bool[,] E;
    private int NumServers;

    //ServerGraph constructor
    //create an empty server graph
    public ServerGraph()
    {
        //create empty V
        V = new WebServer[5];

        //create truly empty multi dm array
        E = new bool[5, 5];

        //NumServers = 0
        NumServers = 0;

    }

    // Return the index of the server with the given name; otherwise return -1
    private int FindServer(string name)
    {
        //special case for empty list
        if (V[0] == null)
            return -1;

        for (int i = 0; i < V.Length; i++)
        {
            //if you get to a null position, it means you've looped through whole V list w/o finding it
            if (V[i] == null)
                break;
            else if (V[i].name == name)
                return i;
        }

        return -1;
    }

    // Double the capacity of the server graph with the respect to web servers
    //only the V list? I feel like E might be needed too

    private void DoubleCapacity()
    {
        int sizeOld = V.Length;
        int sizeNew = V.Length * 2;

        WebServer[] V2 = new WebServer[(sizeNew)];
        bool[,] E2 = new bool[(sizeNew), (sizeNew)];

        for (int i = 0; i < sizeOld; i++)
        {
            V2[i] = V[i];
        }

        V = V2;

        for (int i = 0; i < sizeOld; i++)
        {
            for(int j = 0; j < sizeOld; j++)
            {
                E2[i,j] = E[i,j];
            }
        }

        E = E2;
    }

    // Add a server (vertex) with the given name and connect it to the other server
    // Return true if successful; otherwise return false
    public bool AddServer(string name, string other)
    {
        int start = FindServer(name);
        int end = FindServer(other);

        //if the server getting added is new
        if (start == -1)
        {
            //extend server number if needed
            if (NumServers + 1 > V.Length)
                DoubleCapacity();

            WebServer server = new WebServer();
            server.name = name;

            //if the connecting server doesn't exist yet
            if (end == -1)
            {
                V[NumServers] = server;
                //simply set the diagonal (loop for now)
                E[NumServers, NumServers] = true;
                NumServers++;
            }
            else
            {
                V[NumServers] = server;
                E[NumServers, end] = true;
                NumServers++;
            }

            Console.WriteLine("Sever {0} successfully added.", name);
            return true;
        }
        //else the server alredy exists
        Console.WriteLine("Server of same name already exists.");
        return false;

    }

    // Add a webpage to the server with the given name
    // Return true if successful; other return false
    public bool AddWebPage(WebPage w, string name)
    {
        int find = FindServer(name);

        //server in list
        if (find != -1)
        {
            V[find].P.Add(w);
            Console.WriteLine("Webpage {0} was is now hosted on server {1}.", w.Name, name);
            return true;
        }

        //else host server doesn't exist
        Console.WriteLine("Could not find host.");
        return false;
    }

    // Remove the server with the given name by assigning its connections
    // and webpages to the other server
    // Return true if successful; otherwise return false
    //some type of cloning to check for success?

    //go to the row of the server, check which column has values, use the column number to reassign the webpages to the other server
    public bool RemoveServer(string name, string other)
    {
        int start = FindServer(name);
        int end = FindServer(other);

        if (start != -1)
        {
            if (end != -1)
            {
                //traversing down a column, regardless of value reassign optic cables
                for (int i = 0; i < NumServers; i++)
                {
                    E[i, end] = E[i, start];
                }

                //process of tacking on webpages to the other server
                for (int j = 0; j < V[start].P.Count; j++)
                {
                    V[end].P.Add(V[start].P[j]);
                }

                //process of moving last server up into old row & column
                NumServers--;
                V[start] = V[NumServers];
                for (int j = NumServers; j >= 0; j--)
                {
                    E[j, start] = E[j, NumServers];
                    E[start, j] = E[NumServers, j];
                }

                Console.WriteLine("Server {0} was successfully removed and it's connections moved to {1}.", name, other);
                return true;

            }

        }
        Console.WriteLine("Could not find one of the two servers");
        return false;
    }

    // Add a connection from one server to another
    // Return true if successful; otherwise return false
    // Note that each server is connected to at least one other server

    public bool AddConnection(string from, string to)
    {
        int i = FindServer(from);
        int j = FindServer(to);

        if (i > -1 && j > -1)
        {
            if (E[i, j] == false)
                E[i, j] = true;
        }


        return false;
    }

    // Return all servers that would disconnect the server graph into
    // two or more disjoint graphs if ever one of them would go down
    // Hint: Use a variation of the depth-first search

    public string[] CriticalServers()
    {
        string[] crits = new string[NumServers];
        return crits;
    }

    // Return the shortest path from one server to another
    // Hint: Use a variation of the breadth-first search
    public int ShortestPath(string from, string to)
    {
        return -1;
    }

    // Print the name and connections of each server as well as
    // the names of the webpages it hosts
    public void PrintGraph()
    {
        
        for (int i = 0; i < NumServers; i++)
        {
            Console.Write("\n**Server {0}**\nConnections to: ", V[i].name);
            for (int j = 0; j < NumServers; j++)
            {
                if (E[i, j] != false)
                {
                    Console.Write("{0}", V[j].name);
                }
                    
            }
            Console.WriteLine();
        }
    }

}

public class WebPage
{
    //data members
    public string Name { get; set; }
    public string Server { get; set; }
    public List<WebPage> E { get; set; }

    //constructor
        //possibly need to specify something in the header, big space in the assign. doc 
    public WebPage(string name, string host)
    {
        Name = name;
        Server = host;
        E = new List<WebPage>(); // Initializing a list of webpages for the hyperlinks on the webpage
    }

    // Searching the list of webpages (hyperlinks) to find the index of a webpage 
    // Return the index if the link is found, and return -1 if the link is not found
    public int FindLink(string name)
    {    
        int i;

        // Search through the list of hyperlinks
        for (i = 0; i < E.Count; i++)
        {
            if (E[i].Name.Equals(name))
                return i;
        }
        return -1;
    }

}

public class WebGraph
{
    private List<WebPage> P;

    // Create an empty WebGraph
    public WebGraph()
    {
        P = new List<WebPage>(); // Initializing the web graph to have an empty list for webpages
    }

    // Return the index of the webpage with the given name; otherwise return -1
    private int FindPage(string name)
    {
        int i;

        // Search through the list of webpages
        for (i = 0; i < P.Count; i++)
        {
            if (P[i].Name.Equals(name))
                return i;
        }
        return -1;       
    }

    // Add a webpage with the given name and store it on the host server
    // Return true if successful; otherwise return false
    public bool AddPage (string name, string host, ServerGraph S)
    {
        // If the page does not exist yet, add it
        if (FindPage(name) == -1)
        {
            WebPage p = new WebPage(name, host);

            // Attempt to add the webpage to the server
            // If it is successfully added, add it to the webpage graph
            // This allows us to ensure that the host exists in the server graph
            if (S.AddWebPage(p, host) == true)
            {
                P.Add(p); // Add the webpage to the webpage graph
                return true; // Return true if the webpage was successfully added 
            }
                
        }

        return false; // Return false if the webpage was not added
    }

    // Remove the webpage with the given name, including the hyperlinks
    // from and to the webpage
    // Return true if successful; otherwise return false

    public bool RemovePage(string name, ServerGraph S)
    {
        return false;
    }

    // Add a hyperlink from one webpage to another
    // Return true if successful; otherwise return false
    public bool AddLink(string from, string to)
    {
            // Checking that both of the pages exist
            // If they dont, return false
            if ((FindPage(from) == -1) || (FindPage(to) == -1))
                return false;

            // Checking that the webpage doesn't already have a hyperlink to the webpage
            // If it does, return false
            if (P[FindPage(from)].FindLink(to) != -1)
                return false;

            // Adding the hyperlink
            P[FindPage(from)].E.Add(P[FindPage(to)]);

            return true;
    }

    // Remove a hyperlink from one webpage to another
    // Return true if successful; otherwise return false
    public bool RemoveLink(string from, string to)
    {
        // Checking that the webpage has a hyperlink to the webpage
        // If it doesn't, return false
        if (P[FindPage(from)].FindLink(to) == -1)
            return false;

        // Removing the hyperlink
        P[FindPage(from)].E.Remove(P[FindPage(to)]);
        return true;
    }

    // Return the average length of the shortest paths from the webpage with
    // given name to each of its hyperlinks
    // Hint: Use the method ShortestPath in the class ServerGraph
    public float AvgShortestPaths(string name, ServerGraph S)
    {
        return (float)1.0;
    }

    // Print the name and hyperlinks of each webpage
    public void PrintGraph()
    {
        //Printing the name of each webpage
        for (int i = 0; i < P.Count; i++)
        {
            Console.WriteLine(P[i].Name);
                
            //Printing out each hyperlink associated with the webpage
            for (int j = 0; j < P[i].E.Count; j++)
            {
                Console.WriteLine("    Hyperlink to: "+P[i].E[j].Name);
            }
        }
        Console.ReadLine();  
    }

}
class Program
    {
        static void Main(string[] args)
        {
            int i, j;

            Console.WriteLine("Adjacency Matrix Implemention");

            DirectedGraph<char> H = new DirectedGraph<char>(7);

            for (i = 0; i < 7; i++)
                H.AddVertex((char)(i + 'a'));

            H.PrintVertices();

            for (i = 0; i < 7; i += 2)
                for (j = 0; j < 7; j += 3)
                {
                    H.AddEdge((char)(i + 'a'), (char)(j + 'a'), 10);
                }

            H.PrintEdges();
            Console.ReadKey();

            H.RemoveVertex('c');
            H.RemoveVertex('f');

            H.PrintVertices();
            H.PrintEdges();
            Console.ReadKey();

            DirectedGraph<int> G = new DirectedGraph<int>(10);

            for (i = 0; i < 7; i++)
                G.AddVertex(i);

            G.PrintVertices();

            G.AddEdge(0, 1, 0);
            G.AddEdge(1, 3, 0);
            G.AddEdge(1, 4, 0);
            G.AddEdge(3, 0, 0);
            G.AddEdge(1, 2, 0);
            G.AddEdge(2, 5, 0);
            G.AddEdge(5, 6, 0);

            G.PrintEdges();
            Console.ReadKey();

            G.DepthFirstSearch();
            Console.ReadKey();

            G.BreadthFirstSearch();
            Console.ReadKey();
        }
    }
