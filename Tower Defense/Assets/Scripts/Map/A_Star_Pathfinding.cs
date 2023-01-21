using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class A_Star_Pathfinding
{
    private int height;
    private int width;
    private Node[,] grid;

    public List<Node> openList = new();
    public List<Node> closedList = new();

    public Node startNode;
    public Node endNode;

    public List<Node> finalPath = new();

    public List<Node> Start()
    {
        ResetGrid();
        height = WaveFunction.gridHeight;
        width = WaveFunction.gridWidth;
        grid = new Node[height, width];
        //Initialize every grid Cell with a node
        SetGrid(height, width, grid);
        //Initialize the start and end node
        startNode = grid[7, 0];
        startNode.wall = false;
        int waypoints = 4;
        for (int i = 0; i < waypoints; i++)
        {
            //Initialize the open and closed list
            if (i == waypoints - 1)
            {
                int rand = Random.Range(0, height - 1);
                endNode = grid[rand, width - 1];
                endNode.wall = false;
            }
            else
            {
                endNode = grid[Random.Range(1, height - 1), Random.Range(1, width - 1)];
                endNode.wall = false;

            }
            //Add the start node to the open list
            openList.Add(startNode);
            //start the Pathfinding
            PathFinding();
            //Reset the open and closed list
            openList.Clear();
            closedList.Clear();
            //Reset the parent of every node
            ResetGrid();
            //Reset the Grid
            SetGrid(height, width, grid);
            //PrintGrid();
            //Set new start node
            startNode = grid[endNode.Height, endNode.Width];
            startNode.wall = false;
        }
        Debug.Log("EndNode: " + endNode.Height + " " + endNode.Width);
        return finalPath;
    }

    void ResetGrid()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[i, j] = null;
            }
        }
    }
    void SetGrid(int height, int width, Node[,] grid)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                float rand = Random.Range(0f, 1f);
                if((i > 1 && i<height-2 && j > 1 && j < width - 2 && rand < 0.2f) || j == width-1 )
                {
                    grid[i, j] = new Node(0, 0, 0, i, j, true);
                }
                else
                {
                    grid[i, j] = new Node(0, 0, 0, i, j, false);
                }
            }
        }
        //Add the neighbours to every node
        for (int k = 0; k < height; k++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[k, j].AddNeighbours(grid, k, j);
            }
        }
    }

    void PathFinding()
    {
        while (openList.Count > 0)
        {
            int lowestF = 0;
            //Get the node with the lowest F cost
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].F_cost < openList[lowestF].F_cost)
                {
                    lowestF = i;
                }
            }
            Node currentNode = openList[0];
            //Check if the current node is the end node
            if (currentNode == endNode)
            {
                Debug.Log("Path found");
                //Backtrack to find the path
                Node temp = currentNode;
                List<Node> tempPath = new();
                while (temp != null)
                {
                    tempPath.Add(temp);
                    temp = temp.Parent;
                }
                tempPath.Reverse();
                tempPath.RemoveAt(tempPath.Count-1);
                foreach (Node n in tempPath)
                {
                    finalPath.Add(n);
                }
                //finalPath.RemoveAt(finalPath.Count-1);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            for (int i = 0; i < currentNode.Neighbours.Count; i++)
            {
                Node neighbour = currentNode.Neighbours[i];
                if (!closedList.Contains(neighbour) && !neighbour.wall)
                {
                    int g_cost = currentNode.G_cost + 1;
                    if (openList.Contains(neighbour))
                    {
                        if (g_cost < neighbour.G_cost)
                        {
                            neighbour.G_cost = g_cost;
                        }
                    }
                    else
                    {
                        neighbour.G_cost = g_cost;
                        openList.Add(neighbour);
                    }
                    //Calculate the H cost of the neighbour -- Mathf.Abs returns the absolute value of the
                    neighbour.H_cost = Mathf.Abs(neighbour.Height - endNode.Height) + Mathf.Abs(neighbour.Width - endNode.Width);
                    neighbour.F_cost = neighbour.G_cost + neighbour.H_cost;
                    neighbour.Parent = currentNode;
                }
            }
        }
    }
}
