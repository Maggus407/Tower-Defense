using System.Threading;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;
using System.IO;
using System.Threading.Tasks;
using System;
using UnityEngine.SceneManagement;

public class Tile
{
    private GameObject tile;
    private string[] sockets = new string[4];
    private float probability;
    public int pathsides;
    public string name;

    public float GetSetProbability
    {
        get { return probability; }
        set { probability = value; }
    }

    public GameObject GetSetTile
    {
        get { return tile; }
        set { tile = value; }
    }

    public string[] GetSetSocket
    {
        get { return sockets; }
        set { sockets = value; }
    }

    public Tile(GameObject tile, string[] sockets, float probability, int pathsides, string name)
    {
        this.tile = tile;
        this.sockets = sockets;
        this.probability = probability;
        this.pathsides = pathsides;
        this.name = name;
    }

}

public class Cell
{
    public bool collapsed;
    public List<Tile> placeableTiles;

    public int height;
    public int width;

    public bool path;

    public Cell(bool collapsed, List<Tile> placeableTiles, int height, int width, bool path)
    {
        this.collapsed = collapsed;
        this.placeableTiles = placeableTiles;
        this.height = height;
        this.width = width;
        this.path = path;
    }
}

public class WaveFunction : MonoBehaviour
{
    //Assets tiles
    public List<GameObject> assets = new();
    //Path tiles
    public List<GameObject> paths = new();
    //Base
    public GameObject schloss;
    //Wavespawner
    public GameObject waveSpawner;
    //Waypoint
    public GameObject wayPoint;

    [Header("MapSize")]
    public static int gridHeight = 15;
    public static int gridWidth = 30;

    //Grid
    private Cell[,] grid = new Cell[gridHeight, gridWidth];

    //List of initial Tiles and their sockets
    List<Tile> tempEnviromentTiles;//Environtment
    List<Tile> tempPathTiles;//Path

    //List of all tiles
    List<Tile> allEnvTiles = new List<Tile>();
    List<Tile> allPathTiles = new List<Tile>();

    public List<Cell> entropyPerCell;

    //To Reset the Map
    List<Tile> resetTile = new();

    //Path for EnemyPathfinde.cs
    public static List<Node> pathForEnemy;

    //Hab doch selber keinen Plan mehr
    List<Node> neuerPath;

    //Loading Screen
    public GameObject loadingScreen;

    //[asset prefab, {TOP, RIGHT, BOTTOM, LEFT}, probability, # of pathsides]
    /*
        G = Green
        B = Blue
        O = Orange/Brown
        W = White
    */

    void Awake()
    {

        tempEnviromentTiles = new List<Tile>
        {
            new Tile(assets[0], new string[] { "GGG", "GGG", "GGG", "GGG" }, 60,0, "Grass"),  //Grass - TileVariant
            new Tile(assets[1], new string[] { "GGG", "GGG", "GGG", "GGG" }, 10,0, "Trees"),  //BigTree           
            new Tile(assets[2], new string[] { "GGG", "GGG", "GGG", "GGG" }, 10,0, "Crystals"),  //Crystals
            new Tile(assets[3], new string[] { "GBG", "GGG", "GBG", "GGG" }, 2,0, "RiverStraight"),  //RiverStraight
            new Tile(assets[4], new string[] { "GBG", "GGG", "GGG", "GBG" }, 2,0, "RiverCorner"),  //RiverCorner
            new Tile(assets[5], new string[] { "GDG", "GGG", "GBG", "GGG" }, 1,0, "RiverTransition"), //RiverTransition
        };

        tempPathTiles = new List<Tile>
        {
            new Tile(paths[0], new string[] { "GOG", "GGG", "GOG", "GGG" }, 20,2, "TileBump"),  //TileBump
            new Tile(paths[1], new string[] { "GOG", "GGG", "GGG", "GOG" }, 20,2, "PathCorner"),  //PathCorner
            new Tile(paths[2], new string[] { "GOG", "GOG", "GOG", "GOG" }, 1,4, "CrossingTile"),  //CrossingTile
            new Tile(paths[3], new string[] { "GBG", "GOG", "GBG", "GOG" }, 1,2, "RiverBridge"),  //RiverBridge
            new Tile(paths[4], new string[] { "GOG", "GGG", "GOG", "GGG" }, 40,2, "PathStraight"),  //PathStraight 
            new Tile(paths[5], new string[] { "GOG", "GGG", "GGG", "GGG" }, 1,1, "TileEndRoundSystem"), //TileEndRoundSpawn
            new Tile(paths[6], new string[] { "GOG", "GOG", "GGG", "GOG" }, 1,3, "T-Corner"), //T-Kreuzung
        };

        RotateTiles(tempEnviromentTiles, 1); //Rotate all Environment Tiles by 90° 180° 270°
        RotateTiles(tempPathTiles, 2); //Rotate all Path Tiles by 90° 180° 270°

        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                grid[i, j] = new Cell(false, null, i, i, false);
            }
        }
       pathForEnemy = new();
        A_Star_Pathfinding a = new();
        List<Node> EnemiePath = a.Start();
        pathForEnemy = EnemiePath;

        neuerPath = new();
        neuerPath = EnemiePath;
        InitWaypoints(EnemiePath);
    }

    //Rotate Tile to get every possible possition
    void RotateTiles(List<Tile> t, int tileList)
    {
        //Rotate Tiles and Sockets to get all Possible positions
        for (int i = 0; i < t.Count; i++)
        {
            Tile tile = t[i];
            int R = 90;
            for (int j = 0; j < 4; j++)
            {
                GameObject a = Instantiate(tile.GetSetTile, new Vector3(-100, 0, 0), Quaternion.identity);
                a.transform.localEulerAngles = new Vector3(0, R * j, 0);
                string[] copy = new string[4];
                for (int k = 0; k < 4; k++)
                {
                    copy[k] = tile.GetSetSocket[(3 + k - j + 1) % 4];
                }
                a.SetActive(false);
                if (tileList == 1)
                {
                    allEnvTiles.Add(new Tile(a, copy, tile.GetSetProbability, tile.pathsides, tile.name));
                }
                if (tileList == 2)
                {
                    allPathTiles.Add(new Tile(a, copy, tile.GetSetProbability, tile.pathsides, tile.name));
                }
            }
        }
        //Remove double tiles 
        if (tileList == 1)
        {
            var b = allEnvTiles.GroupBy(x => new { x.GetSetTile.name, V = x.GetSetSocket[0], C = x.GetSetSocket[1], R = x.GetSetSocket[2], T = x.GetSetSocket[3] }).Select(x => x.First()).ToList();
            allEnvTiles = b;
        }
        if (tileList == 2)
        {
            var b = allPathTiles.GroupBy(x => new { x.GetSetTile.name, V = x.GetSetSocket[0], C = x.GetSetSocket[1], R = x.GetSetSocket[2], T = x.GetSetSocket[3] }).Select(x => x.First()).ToList();
            allPathTiles = b;
        }
    }

    void Start()
    {
        List<Node> path = new();
       List<Node> path100 = new();
        path100 = neuerPath;

       var b = path100.GroupBy(x => new { H = x.Height, W = x.Width }).Select(x => x.First()).ToList();
       path = b;

       var dic = PathGraph(path100);
       MapSetup(path, dic);
       StartRandom(path[0]);
    }

    void InitWaypoints(List<Node> EnemiePath)
    {
        foreach(Node n in EnemiePath)
        {           
            Instantiate(wayPoint, new Vector3(n.Width, 0.6f, n.Height), Quaternion.identity);
        }
    }

    
 //Counts the amount of edges for every Node
    public Dictionary<(int,int), int> PathGraph(List<Node> EnemiePath)
    {
        var edgeCount = new Dictionary<(int,int), List<Node>>();
        var nodeCount = new Dictionary<(int,int), int>();
        for (int i = 0; i < EnemiePath.Count; i++)
        {
            if(!edgeCount.ContainsKey((EnemiePath[i].Height, EnemiePath[i].Width))) { 
                edgeCount.Add((EnemiePath[i].Height, EnemiePath[i].Width), new List<Node>());
                nodeCount.Add((EnemiePath[i].Height, EnemiePath[i].Width),0);
            }
        }
        for (int i = 0; i < EnemiePath.Count; i++)
        {
           if(i == 0)
            {
                edgeCount[(EnemiePath[i].Height, EnemiePath[i].Width)].Add(EnemiePath[i + 1]);
            }
            else if (i == EnemiePath.Count - 1)
            {
                edgeCount[(EnemiePath[i].Height, EnemiePath[i].Width)].Add(EnemiePath[i - 1]);
            }
            else
            {
                edgeCount[(EnemiePath[i].Height, EnemiePath[i].Width)].Add(EnemiePath[i - 1]);
                edgeCount[(EnemiePath[i].Height, EnemiePath[i].Width)].Add(EnemiePath[i + 1]);
            }
        }

        //Remove double values from every Key
        for (int i = 0; i < EnemiePath.Count; i++)
        {
            var b = edgeCount[(EnemiePath[i].Height, EnemiePath[i].Width)].GroupBy(x => new { H = x.Height, W = x.Width }).Select(x => x.First()).ToList();
            edgeCount[(EnemiePath[i].Height, EnemiePath[i].Width)] = b;

            nodeCount[(EnemiePath[i].Height, EnemiePath[i].Width)] = b.Count;

        }
        return nodeCount;
    }

/*
initilize every Grid Cell
--> Add every Cell to the entropyPerCell List
*/
    void MapSetup(List<Node> path, Dictionary<(int,int),int> dic)
    {
        entropyPerCell = new List<Cell>();
        //Initilize every Path Cell
        for (int i = 0; i < path.Count; i++)
        {
            List<Tile> copy = new();
            foreach (Tile t in allPathTiles)
            {
                if(i > 0 && i < path.Count - 2)
                {
                    //Check abover and under
                    bool a = (path[i - 1].Height == path[i].Height - 1 && path[i + 1].Height == path[i].Height + 1);
                    //Check right and left
                    bool b = (path[i - 1].Width == path[i].Width - 1 && path[i + 1].Width == path[i].Width + 1);
                    if (dic[(path[i].Height, path[i].Width)] == t.pathsides)
                    {
                        if(t.pathsides == 2 && (a || b))
                        {
                            if(t.name != "PathCorner")
                            {
                                copy.Add(t);
                            }
                        }
                        else
                        {
                            copy.Add(t);
                        }
                        
                    }
                }else if(dic[(path[i].Height, path[i].Width)] == t.pathsides)
                {
                    copy.Add(t);
                }
            }
            grid[path[i].Height, path[i].Width] = new Cell(false, copy, path[i].Height, path[i].Width, true);
            entropyPerCell.Add(grid[path[i].Height, path[i].Width]);
            if (i == 0)
            {
                grid[path[i].Height, path[i].Width].placeableTiles.RemoveAll(x => x.GetSetSocket[3] == "GOG");
            }
        }

        //Initilize every Environment Cell
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                //Erstelle eine Kopie von tiles
                if (grid[i, j].path == false)
                {
                    List<Tile> copy = new List<Tile>();
                    foreach (Tile t in allEnvTiles)
                    {
                        copy.Add(t);
                    }
                    grid[i, j] = new Cell(false, copy, i, j, false);
                    entropyPerCell.Add(grid[i, j]);
                }
            }
        }
        int h = pathForEnemy[pathForEnemy.Count - 1].Height;
        int w = pathForEnemy[pathForEnemy.Count - 1].Width;
        Instantiate(schloss, new Vector3(w, 0.2f, h), Quaternion.identity);
        Instantiate(paths[2], new Vector3(w, 0, h), Quaternion.identity);
        grid[h, w].collapsed = true;
        entropyPerCell.Remove(grid[h, w]);

    }

    //Start with the first element from the Path
    void StartRandom(Node start)
    {
        int height = start.Height;
        int width = start.Width;
        int k = UnityEngine.Random.Range(0, grid[height, width].placeableTiles.Count);
        grid[height, width].collapsed = true;
        Tile t = grid[height, width].placeableTiles[k];
        Instantiate(waveSpawner, new Vector3(width, 0.6f, height), Quaternion.identity);
        Collapse(height, width, t);
    }

    void Collapse(int height, int width, Tile t)
    {
        //If the List is empty, the Map is finished
        if (entropyPerCell.Count == 0)
        {
            Debug.Log("Map Finished");
            loadingScreen.SetActive(false);
            return;
        }
        
        
        //Check if the Cell is collapsed and not out of bounds
        //Add every matching Tile to the List
        List<Tile> top = new();
        List<Tile> bottom = new();
        List<Tile> left = new();
        List<Tile> right = new();

        bool top_free = false;
        bool right_free = false;
        bool bottom_free = false;
        bool left_free = false;

        if (height + 1 < gridHeight && !grid[height + 1, width].collapsed)
        {
            top_free = true;
            try
            {
                foreach (Tile n in grid[height + 1, width].placeableTiles)
                {
                    if (n.GetSetSocket[2] == t.GetSetSocket[0])
                    {
                        top.Add(n);
                    }
                }
            }
            catch
            {
                ResetMap();
                return;
            }
        }
        if (height - 1 >= 0 && !grid[height - 1, width].collapsed)
        {
            bottom_free = true;
            try
            {
                foreach (Tile n in grid[height - 1, width].placeableTiles)
                {
                    if (n.GetSetSocket[0] == t.GetSetSocket[2])
                    {
                        bottom.Add(n);
                    }
                }
            }
            catch
            {
                ResetMap();
                return;
            }
        }
        if (width + 1 < gridWidth && !grid[height, width + 1].collapsed)
        {
            right_free = true;
            try
            {
                foreach (Tile n in grid[height, width + 1].placeableTiles)
                {
                    if (n.GetSetSocket[3] == t.GetSetSocket[1])
                    {
                        right.Add(n);
                    }
                }
            }
            catch
            {
                ResetMap();
                return;
            }
        }
        if (width - 1 >= 0 && !grid[height, width - 1].collapsed)
        {
            left_free = true;
            try
            {
                foreach (Tile n in grid[height, width - 1].placeableTiles)
                {
                    if (n.GetSetSocket[1] == t.GetSetSocket[3])
                    {
                        left.Add(n);
                    }
                }
            }
            catch
            {
                ResetMap();
                return;
            }
        }

        //Check if the one of the neighbors is empty and remove the current tile from the current Cell and start again with an other tile
        //Else replace the neighbor with the new lists

        //Debug.Log("TOP: " + top.Count + " BOTTOM: " + bottom.Count + " LEFT: " + left.Count + " RIGHT: " + right.Count + " TileName: " + t.GetSetTile.name);

        if (top_free && top.Count == 0 || bottom_free && bottom.Count == 0 || right_free && right.Count == 0 || left_free && left.Count == 0)
        {
            grid[height, width].placeableTiles.Remove(t);
            if (grid[height, width].placeableTiles.Count == 0)
            {
                ResetMap();
            }
            int k = UnityEngine.Random.Range(0, grid[height, width].placeableTiles.Count);
            Collapse(height, width, grid[height, width].placeableTiles[k]);
            return;
        }
        else
        {
            
            //Check the Neighbors from the current Neighbor to avoid dead ends
            Thread topCheck = new(() => { top = SecondCheckPhase(top_free, top, height + 1, width, grid[height, width]); });
            Thread bottomCheck = new(() => { bottom = SecondCheckPhase(bottom_free, bottom, height - 1, width, grid[height, width]); });
            Thread rightCheck = new(() => { right = SecondCheckPhase(right_free, right, height, width + 1, grid[height, width]); });
            Thread leftCheck = new(() => { left = SecondCheckPhase(left_free, left, height, width - 1, grid[height, width]); });
            //Start Threads
            topCheck.Start();
            bottomCheck.Start();
            rightCheck.Start();
            leftCheck.Start();
            //Wait to finish
            topCheck.Join();
            bottomCheck.Join();
            rightCheck.Join();
            leftCheck.Join();
            
            /*
            top = SecondCheckPhase(top_free, top, height + 1, width, grid[height, width]);
            bottom = SecondCheckPhase(bottom_free, bottom, height - 1, width, grid[height, width]);
            right = SecondCheckPhase(right_free, right, height, width + 1, grid[height, width]);
            left = SecondCheckPhase(left_free, left, height, width - 1, grid[height, width]);
            */

            //Replace the neighbors with the new lists
            if (top_free)
            {
                grid[height + 1, width].placeableTiles = top;
            }
            if (bottom_free)
            {
                grid[height - 1, width].placeableTiles = bottom;
            }
            if (right_free)
            {
                grid[height, width + 1].placeableTiles = right;
            }
            if (left_free)
            {
                grid[height, width - 1].placeableTiles = left;
            }
        }
        //Set the Base on the Last element in EnemiePath
        t.GetSetTile.SetActive(true);
        grid[height, width].collapsed = true;
        resetTile.Add(t);
        Instantiate(t.GetSetTile, new Vector3(width, 0, height), t.GetSetTile.transform.rotation);
        entropyPerCell.Remove(grid[height, width]);

        UpdateEntropy();
        SortEntropybyTotalWeight();
        if (entropyPerCell.Count == 0)
        {
            Debug.Log("Map Finished");
            loadingScreen.SetActive(false);
            return;
        }
        PlaceNexttile();
    }

    void ResetMap()
    {
        Debug.Log("Neu Laden...");
        foreach(Tile t in resetTile)
        {
            t.GetSetTile.SetActive(false);
        }
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    private List<Tile> SecondCheckPhase(bool execute, List<Tile> checkList, int height, int width, Cell parent)
    {
        if(!execute){
            return checkList;
        }
        //Check Top Side
        foreach (Tile n in checkList.ToList())
        {
            int placeableCells = 0;
            int totalWeight = 0;
            if (height + 1 < gridHeight && !grid[height + 1, width].collapsed && height + 1 != parent.height)
            {
                placeableCells++;
                foreach (Tile neighbor in grid[height + 1, width].placeableTiles)
                {
                    if (n.GetSetSocket[0] == neighbor.GetSetSocket[2])
                    {
                        totalWeight++;
                        break;
                    }
                }
            }
            //Check Bottom Side
            if (height - 1 >= 0 && !grid[height - 1, width].collapsed && height - 1 != parent.height)
            {
                placeableCells++;
                foreach (Tile neighbor in grid[height - 1, width].placeableTiles)
                {
                    if (n.GetSetSocket[2] == neighbor.GetSetSocket[0])
                    {
                        totalWeight++;
                        break;
                    }
                }
            }
            //Check Right Side
            if (width + 1 < gridWidth && !grid[height, width + 1].collapsed && width + 1 != parent.width)
            {
                placeableCells++;
                foreach (Tile neighbor in grid[height, width + 1].placeableTiles)
                {
                    if (n.GetSetSocket[1] == neighbor.GetSetSocket[3])
                    {
                        totalWeight++;
                        break;
                    }
                }
            }
            //Check Left Side
            if (width - 1 >= 0 && !grid[height, width - 1].collapsed && width - 1 != parent.width)
            {
                placeableCells++;
                foreach (Tile neighbor in grid[height, width - 1].placeableTiles)
                {
                    if (n.GetSetSocket[3] == neighbor.GetSetSocket[1])
                    {
                        totalWeight++;
                        break;
                    }
                }
            }
            if (placeableCells != totalWeight)
            {
                checkList.Remove(n);
            }
        }
    return checkList;
    }

    void PlaceNexttile()
    {
        //Pick the next tile depending on the probability
        float sum = 0;
        for (int i = 0; i < entropyPerCell[0].placeableTiles.Count; i++)
        {
            sum += entropyPerCell[0].placeableTiles[i].GetSetProbability;
        }

        //Normalize the probability for every tile
        //Choose a random number between 0 and 1
        //Check if the random number is smaller than the probability of the first tile, if yes, place the first tile
        //If not, add the probability of the first tile to the random number and check if the random number is smaller than the probability of the second tile
        float temp = 0;
        Tile nextTile = null;
        float r = UnityEngine.Random.Range(0f, 1f);
        for (int i = 0; i < entropyPerCell[0].placeableTiles.Count; i++)
        {
            float currentPosibility = entropyPerCell[0].placeableTiles[i].GetSetProbability / sum;
            temp += currentPosibility;
            if (r <= temp)
            {
                nextTile = entropyPerCell[0].placeableTiles[i];
                break;
            }
        }

        //Place the tile
        int height = entropyPerCell[0].height;
        int width = entropyPerCell[0].width;
        //Collapse the neighbors from the new placed tile

        Collapse(height,width,nextTile);
    }

    void SortEntropybyTotalWeight()
    {
        //Count the amount of Cells with the same amount of possible tiles
        int count = 0;
        for (int i = 1; i < entropyPerCell.Count-1; i++)
        {
            if (entropyPerCell[0].placeableTiles.Count == entropyPerCell[i].placeableTiles.Count)
            {
                count++;
            }
            else
            {
                break;
            }
        }

        //Sort the entropyPerCell List(only the amount of of even Cells (count)) by the sum of the tiles possiblity, ascending
        for (int i = 0; i < count; i++)
        {
            for (int j = i + 1; j < count; j++)
            {
                float sumI = 0;
                float sumJ = 0;
                foreach (Tile tile in entropyPerCell[i].placeableTiles)
                {
                    sumI += tile.GetSetProbability;
                }
                foreach (Tile tile in entropyPerCell[j].placeableTiles)
                {
                    sumJ += tile.GetSetProbability;
                }
                if (sumI < sumJ)
                {
                    (entropyPerCell[j], entropyPerCell[i]) = (entropyPerCell[i], entropyPerCell[j]);
                }
            }
        }
    }

    void UpdateEntropy()
    {
        //Sort the Entropy List by the amount of possible Tiles
        entropyPerCell.Sort(delegate (Cell x, Cell y) {
            return x.placeableTiles.Count.CompareTo(y.placeableTiles.Count);
        });
    }
}