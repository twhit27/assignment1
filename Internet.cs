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
                //Ensuring that both servers are connected to each other
                E[NumServers, end] = true;
                E[end, NumServers] = true;
                
                NumServers++;
            }

            Console.WriteLine("Server {0} successfully added.", name);
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
            //Checking if the webpage is already hosted by the server
            //Go through all of the webpages and check if they are equal to it
            //I will simplify this if I can find a way - Cam
            for (int i = 0; i < V[find].P.Count; i++)
            {

                if (V[find].P[i].Equals(w.Name, w.Server))
                {
                    //webpage already exists
                    Console.WriteLine("The server already hosts that webpage.");
                    return false;
                }
            }

            //Add webpage if it does not yet exist on the server
            V[find].P.Add(w);
            Console.WriteLine("Webpage {0} is now hosted on server {1}.", w.Name, name);
            return true;
           
        }
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
                    //Ensuring not change any of the true values for the other server
                    if (E[i, end] == false)
                        E[i, end] = E[i, start];
                }
    
                //traversing across a row, regardless of value reassign optic cables
                for (int i = 0; i < NumServers; i++)
                {
                    //Ensuring not change any of the true values for the other server
                    if (E[end, i] == false)
                     E[end, i] = E[start, i];
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
            //Ensuring that both servers are connected to each other
            if (E[i, j] == false || E[j, i] == false)
            {
                E[i, j] = true;
                E[j, i] = true;
                return true;
            }
                
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
        // intialization
        int count = 0;
        string path = "";
        bool[] visited = new bool[NumServers];
        Queue<int> Q = new Queue<int>();

        // visited will be used to mark nodes as visited
        for (int i = 0; i < NumServers; i++)
            visited[i] = false;

        // if both servers exist...
        if (FindServer(from) != -1 && FindServer(to) != -1)
        {
            // since we din't need to traverse the entire graph, start the process at from
            Q.Enqueue(FindServer(from));
            visited[FindServer(from)] = true;

            // loop until we arrive at the 'to' server
            while(Q.Peek() != FindServer(to))
            {
                // these 3 lines 'process' the server
                int i = Q.Dequeue();
                path = path + V[i].name +" ";
                count++;

                // from prof's code, move down the line in the matrix checking for server connections
                for (int j = 0; j < NumServers; j++)
                {
                    if (!visited[j] && E[i, j])
                    {
                        Q.Enqueue(j);
                        visited[j] = true;           // Mark vertex as visited
                    }
                }
            }

            Console.WriteLine("The shortest path from {0} to {1} is {2}", from, to, path);
            return count;

        }

        Console.WriteLine("Either 1 or both server names are invalid");
        return -1;
    }

    // Print the name and connections of each server as well as
    // the names of the webpages it hosts
    public void PrintGraph()
    {
        for (int i = 0; i < NumServers; i++)
        {
            //Printing out the server's connections
            Console.WriteLine("\n**Server {0}**\nConnections to: ", V[i].name);
            for (int j = 0; j < NumServers; j++)
            {
                if (E[i, j] != false)
                {
                    Console.WriteLine("{0}", V[j].name);
                }
        
            }
        
            //Printing out the webpages the server hosts
            Console.WriteLine("Hosted web pages: ", V[i].name);
            for (int j = 0; j < V[i].P.Count; j++)
            {
          
                Console.WriteLine("{0}", V[i].P[j].Name);
        
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

    //Evaluates if two webpages are the same / 'equal to each other'
    //Returns true if they are equal, and false if they are not equal
    public bool Equals(string name, string host)
    {
        //If the names and hosts of both of the webpages are the same, the webpages are the same
        if ((Name == name) && (Server == host))
        {
            return true;
        }

        //If they are not return false
        return false;
    }
}

public class WebGraph
{
    private List<WebPage> P;

    // Create an empty WebGraph
    public WebGraph()
    {
        P = new List<WebPage>(); // Initializing the web graph to have an empty list for webpages
        Console.WriteLine("The webgraph was created!");
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
        // If the page already exists in the web graph, don't add it
        if (FindPage(name) == -1)
        {
            WebPage p = new WebPage(name, host);

            // Attempt to add the webpage to the server
            // If it is successfully added, add it to the webpage graph
            // This allows us to ensure that the host exists in the server graph
            // Also allows us to check if the webpage is a duplicate for that server
            if (S.AddWebPage(p, host) == true)
            {
                P.Add(p); // Add the webpage to the webpage graph
                Console.WriteLine("Page was successfully added!");
                return true; // Return true if the webpage was successfully added 
            }

            //Error message will be printed out in AddWebPage if this fails
        }

        Console.WriteLine("Page was not added.");
        return false; // Return false if the webpage was not added
    }

    // Remove the webpage with the given name, including the hyperlinks
    // from and to the webpage
    // Return true if successful; otherwise return false

    public bool RemovePage(string name, ServerGraph S)
    {
        //Find the location of the page being deleted in the WebGraph
        int pageIndex = FindPage(name);

        //Only remove page if it does not exist
        if (pageIndex != -1)
        {
            //Go to each page of the WebGraph to check if it has hyperlinks that point to the page being removed
            for (int j = 0; j < P.Count; j++)
            {
                //Go through all of the hyperlinks on the page and delete the hyperlink if it points to the page being removed
                //Since there are no duplicate hyperlinks, we only have to check for one hyperlink
                int hypIndex = P[j].FindLink(name);
                if (hypIndex != -1)
                {
                    RemoveLink(P[j].Name, name);
                }
            }
            //Removing the page
            P.RemoveAt(pageIndex);
            Console.WriteLine("The page '{0}' was successfully deleted!", name);
            return true;
        }

        Console.WriteLine("Page was not removed since it does not exist.");
        return false;
    }

    // Add a hyperlink from one webpage to another
    // Return true if successful; otherwise return false
    public bool AddLink(string from, string to)
    {
        // Checking that both of the pages exist
        // If they dont, return false
        if ((FindPage(from) == -1) || (FindPage(to) == -1))
        {
            Console.WriteLine("Link was not added because at least one of the pages do not exist.");
            return false;
        }

        // Checking that the webpage doesn't already have a hyperlink to the webpage
        // If it does, return false
        if (P[FindPage(from)].FindLink(to) != -1)
        {
            Console.WriteLine("Link was not added because {0} already has a hyperlink to {1}.", from, to);
            return false;
        }

        // Adding the hyperlink
        P[FindPage(from)].E.Add(P[FindPage(to)]);
        Console.WriteLine("Hyperlink was successfully added!");
        return true;
    }

    // Remove a hyperlink from one webpage to another
    // Return true if successful; otherwise return false
    public bool RemoveLink(string from, string to)
    {
        // Checking that the webpage has a hyperlink to the webpage
        // If it doesn't, return false
        if (P[FindPage(from)].FindLink(to) == -1)
        {
            Console.WriteLine("Link was not removed because it does not exist.");
            return false;
        }
            

        // Removing the hyperlink
        P[FindPage(from)].E.Remove(P[FindPage(to)]);
        Console.WriteLine("Hyperlink was successfully removed!");
        return true;
    }

    // Return the average length of the shortest paths from the webpage with
    // given name to each of its hyperlinks
    // Hint: Use the method ShortestPath in the class ServerGraph
    public float AvgShortestPaths(string name, ServerGraph S)
    {
        int pageIndex = FindPage(name);
        int avgShortPath, pathSum = 0;
        string mainHost, linkHost;

        //Checking that the webpage exists
        if (pageIndex == -1)
        {
            Console.WriteLine("AvgShortestPath was not calculated because page does not exist.");
            return 0;
        }

        //Checking that the webpage has hyperlinks
        if (P[pageIndex].E.Count() == 0)
        {
            Console.WriteLine("AvgShortestPath was not calculated because page does not have any hyperlinks.");
            return 0;
        }

        mainHost = P[pageIndex].Server;
        Console.WriteLine(mainHost);

        //Find the shortest path to each hyperlink
        for (int j = 0; j < P[pageIndex].E.Count; j++)
        {
            linkHost = P[pageIndex].E[j].Server;
            pathSum += S.ShortestPath(mainHost, linkHost);
        }

        //Calculate the average shortest path
        avgShortPath = pathSum / P[pageIndex].E.Count();

        Console.WriteLine("The average shortest path was successfully calculated and is {0}.", avgShortPath);

        return avgShortPath;
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
    }

}
class Program
{
    static void Main(string[] args)
    {
        //we may want to add a condition here (if we decided on user input and not hard coding) for adding the very first graph (i.e, connect it to itself)
        ServerGraph foo = new ServerGraph();
        WebGraph foo2 = new WebGraph();
        foo.AddServer("Canada", "Canada");
        foo.AddServer("Europe", "Canada");
        foo.AddServer("Asia", "Europe");
        foo.AddServer("Africa", "Asia");
        foo.AddServer("Australia", "Asia");
        foo.AddServer("Antarctica", "Canada");
        foo.AddServer("Canada", "Europe");
        foo.AddServer("US", "Peru");
        foo.AddConnection("Canada", "Asia");
        foo.AddConnection("Asia", "Africa");
        foo.AddConnection("Africa", "Canada");
        foo.AddConnection("America", "Canada");
        //WebPage wiki = new WebPage("Wikipedia", "Canada");
        //foo.AddWebPage(wiki, "Canada");
        foo2.AddPage("Wikipedia", "Canada", foo);
        foo2.AddPage("Trent", "Canada", foo);
        foo2.AddPage("Google", "Europe", foo);
        foo2.AddPage("YouTube", "Asia", foo);
        foo2.AddPage("GitHub", "America", foo);
        foo2.AddLink("Google", "Wikipedia");
        foo2.AddLink("Google", "Trent");
        foo2.AddLink("Wikipedia", "YouTube");
        foo2.AddLink("Google", "GitHub");
        foo.PrintGraph();
        foo2.PrintGraph();
        foo2.RemoveLink("YouTube", "Wikipedia");
        foo2.RemoveLink("Google", "Wikipedia");
        foo2.RemoveLink("Trent", "Wikipedia");
        foo2.RemovePage("Wikipedia", foo);
        foo2.RemovePage("YouTube", foo);
        foo2.RemovePage("GitHub", foo);
        foo.RemoveServer("Asia", "Europe");
        foo.RemoveServer("Africa", "Canada");
        foo.RemoveServer("Antarctica", "Australia");

        foo.PrintGraph();
        foo2.PrintGraph();

        Console.ReadLine();
    }
}
