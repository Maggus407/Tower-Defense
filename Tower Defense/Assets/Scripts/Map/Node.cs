using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private int g_cost;
    private int h_cost;
    private int f_cost;
    private int height;
    private int width;
    private Node parent;
    public int edges;

    public bool wall;

    private List<Node> neighbours;

    public List<Node> Neighbours{
        get{return neighbours;}
        set{neighbours = value;}
    }

    public Node Parent{
        get{return parent;}
        set{parent = value;}
    }

    public int G_cost{
        get{return g_cost;}
        set{g_cost = value;}
    }

    public int H_cost{
        get{return h_cost;}
        set{h_cost = value;}
    }

    public int F_cost{
        get{return f_cost;}
        set{f_cost = value;}
    }

    public int Height{
        get{return height;}
        set{height = value;}
    }

    public int Width{
        get{return width;}
        set{width = value;}
    }

    public Node(int g_cost, int h_cost, int f_cost, int height, int width, bool wall){
        this.g_cost = g_cost;
        this.h_cost = h_cost;
        this.f_cost = f_cost;
        this.height = height;
        this.width = width;
        this.wall = wall;
    }

    public List<Node> AddNeighbours(Node[,] grid, int height, int width){
        int gridHeight = WaveFunction.gridHeight;
        int gridWidth = WaveFunction.gridWidth;
        neighbours = new List<Node>();
        if(height+1 < gridHeight){
            neighbours.Add(grid[height+1, width]);
        }
        if(height-1 >= 0){
            neighbours.Add(grid[height-1, width]);
        }
        if(width+1 < gridWidth){
            neighbours.Add(grid[height, width+1]);
        }
        if(width-1 >= 0){
            neighbours.Add(grid[height, width-1]);
        }
        return neighbours;
    }

}
