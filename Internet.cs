﻿public class ServerGraph
{
    //WebServer constructor
    private class WebServer
    {
        public string name;
        public List<WebPage> P;
    }

    private WebServer[] V;
    private bool[,] E;
    private int NumServers;

    //ServerGraph constructor
    //create an empty server graph
    public ServerGraph()
    {
        //create empty V 
        //create truly empty multi dm array
        //NumServers = 0
    }

    // Return the index of the server with the given name; otherwise return -1
    private int FindServer(string name)
    {
        //code...
        return -1;
    }

    // Double the capacity of the server graph with the respect to web servers

    private void DoubleCapacity()
    {
        //double V and E? maybe just V
    }

    // Add a server (vertex) with the given name and connect it to the other server
    // Return true if successful; otherwise return false

    public bool AddServer (string name, string other)
    {
        //false case: server w/ same name exists, possibly others
        return false;
    }

    // Add a webpage to the server with the given name
    // Return true if successful; other return false
    public bool AddWebPage(WebPage w, string name)
    {
        //code..
        return false;
    }

    // Remove the server with the given name by assigning its connections
    // and webpages to the other server
    // Return true if successful; otherwise return false
        //some type of cloning to check for success?
    public bool RemoveServer(string name, string other)
    {
        //code...
        return false;
    }

    // Add a connection from one server to another
    // Return true if successful; otherwise return false
    // Note that each server is connected to at least one other server

    public bool AddConnection(string from, string to)
    {
        //code...
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
    }

    public int FindLink(string name)
    {
        return -1;
    }

}

public class WebGraph
{
    private List<WebPage> P;

    // Create an empty WebGraph
    public WebGraph()
    {

    }

    // Return the index of the webpage with the given name; otherwise return -1
    private int FindPage(string name)
    {
        return -1;
    }

    // Add a webpage with the given name and store it on the host server
    // Return true if successful; otherwise return false

    public bool AddPage (string name, string host)
    {
        return false;
    }

    // Remove the webpage with the given name, including the hyperlinks
    // from and to the webpage
    // Return true if successful; otherwise return false

    public bool RemovePage(string name)
    {
        return false;
    }

    // Add a hyperlink from one webpage to another
    // Return true if successful; otherwise return false
    public bool AddLink(string from, string to)
    {
        return false;
    }

    // Remove a hyperlink from one webpage to another
    // Return true if successful; otherwise return false
    public bool RemoveLink(string from, string to)
    {
        return false;
    }

    // Return the average length of the shortest paths from the webpage with
    // given name to each of its hyperlinks
    // Hint: Use the method ShortestPath in the class ServerGraph
    public float AvgShortestPaths(string name)
    {
        return (float)1.0;
    }

    // Print the name and hyperlinks of each webpage
    public void PrintGraph()
    {

    }

}