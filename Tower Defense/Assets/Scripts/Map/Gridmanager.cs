using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridmanager : MonoBehaviour
{
    private int gridWidth = 25;
    private int gridHeight = 15;
    public int minPathLength = 40;

    private Pathgenerator pathgenerator;
    [Header("Path")]
    public GameObject firstTilefromSpawn;
    public GameObject pathTileStraight;
    public GameObject pathTileCorner;

    [Header("EnvironmentGround")]
    public GameObject buildTile;

    [Header("Base")]
    public GameObject Base;

    [Header("Waypoint")]
    public GameObject waypoint;
    public static List<GameObject> enemiePath;

    [Header("WaveSpawner")]
    public GameObject waveSpawner;

    [Header("Accessoires")]
    public GameObject Kiste;
    public GameObject[] Tree;
    public GameObject grass;

    //Lässt eine Path erstellen, welche mindesten so lang wie minPathLenght ist
    void Start(){
        pathgenerator = new Pathgenerator(gridWidth, gridHeight);
        List<Vector2Int> pathcells = pathgenerator.GeneratePath();
        int pathSize = pathcells.Count;
        while(minPathLength > pathSize){
            pathcells = pathgenerator.GeneratePath();
            pathSize = pathcells.Count;
        }
        List<Vector2Int> buildcells = pathgenerator.BuildCells();
        StartCoroutine(BauePath(pathcells));
        StartCoroutine(BaueRest(buildcells));
        StartCoroutine(BuildEnvironment(buildcells));
        StartCoroutine(BaueSpawnUndBase(pathcells));
    }

    //Spawnt den Wavespawner am ersten Block
    private IEnumerator BaueSpawnUndBase(List<Vector2Int> pathcells){
        Instantiate(waveSpawner, new Vector3(pathcells[0].x, 1f, pathcells[0].y), Quaternion.identity);
        yield return null;
    }

    //Spawnt die Path Blöcke + Waypoints
    private IEnumerator BauePath(List<Vector2Int> pathcells){
        enemiePath = new List<GameObject>();

            for(int i = 0; i < pathcells.Count; i++){
                GameObject path = new GameObject();
                //Der erste Block immer Straight richtung X
                if(i == 0){
                Instantiate(firstTilefromSpawn, new Vector3(pathcells[i].x, 0f, pathcells[i].y), transform.rotation * Quaternion.Euler(0f, 90f,0f));
                continue;
                }
                if(i == pathcells.Count - 1){
                    Instantiate(pathTileStraight, new Vector3(pathcells[i].x, 0f, pathcells[i].y), transform.rotation * Quaternion.Euler(0f, 90f,0f));
                    Instantiate(Base, new Vector3(pathcells[i].x, 0.2f, pathcells[i].y), Quaternion.identity);
                    path = Instantiate(waypoint, new Vector3(pathcells[i].x, 0.6f, pathcells[i].y), Quaternion.identity);
                    enemiePath.Add(path);
                    yield return new WaitForSeconds(0.02f);
                    break;
                }
                
                if(pathcells[i+1].x == i+1){
                    Instantiate(pathTileStraight, new Vector3(pathcells[i].x, 0f, pathcells[i].y), transform.rotation * Quaternion.Euler(0f, 90f,0f));
                }
                else if(pathcells[i+1].y < pathcells[i].y && pathcells[i-1].x < pathcells[i].x){
                    Instantiate(pathTileCorner, new Vector3(pathcells[i].x, 0f, pathcells[i].y), transform.rotation * Quaternion.Euler(0f, 270f,0f));
                }
                else if(pathcells[i+1].x > pathcells[i].x && pathcells[i-1].y > pathcells[i].y){
                    Instantiate(pathTileCorner, new Vector3(pathcells[i].x, 0f, pathcells[i].y), transform.rotation * Quaternion.Euler(0f, 90f,0f));
                }
                else if(pathcells[i+1].y > pathcells[i].y && pathcells[i].x > pathcells[i-1].x){
                    Instantiate(pathTileCorner, new Vector3(pathcells[i].x, 0f, pathcells[i].y), transform.rotation * Quaternion.Euler(0f, 0f,0f));
                }else if(pathcells[i+1].x > pathcells[i].x && pathcells[i-1].y < pathcells[i].y){
                    Instantiate(pathTileCorner, new Vector3(pathcells[i].x, 0f, pathcells[i].y), transform.rotation * Quaternion.Euler(0f, 180f,0f));
                }
                else if(pathcells[i+1].x == pathcells[i].x && (pathcells[i+1].y > pathcells[i].y || pathcells[i-1].y > pathcells[i].y)){
                    Instantiate(pathTileStraight, new Vector3(pathcells[i].x, 0f, pathcells[i].y), transform.rotation * Quaternion.Euler(0f, 0f,0f));
                }else{
                    Instantiate(pathTileStraight, new Vector3(pathcells[i].x, 0f, pathcells[i].y), transform.rotation * Quaternion.Euler(0f, 90f,0f));
                }
        path = Instantiate(waypoint, new Vector3(pathcells[i].x, 0.6f, pathcells[i].y), Quaternion.identity);
        enemiePath.Add(path);
        yield return new WaitForSeconds(0.02f);
        }
    yield return null;
    }
    //Spawnt die Blöcke auf den man Bauen kann
    private IEnumerator BaueRest(List<Vector2Int> bcells){
        foreach(Vector2Int cell in bcells){
            Instantiate(buildTile, new Vector3(cell.x, 0f, cell.y), Quaternion.identity);
            yield return new WaitForSeconds(0.002f);
        }
        yield return null;
    }

    //Spawnt die Umgebung
    private IEnumerator BuildEnvironment(List<Vector2Int> Ecells){
        foreach(Vector2Int cell in Ecells){
            float index = Random.value;
            if(index >= 0.0 && index < 0.02f && cell.x != 0 && cell.y != 0 && cell.x < gridWidth-1 && cell.y < gridHeight-1){
                Instantiate(Kiste, new Vector3(cell.x, 0.55f, cell.y), Quaternion.identity);
            }else if(index >= 0.1 && index < 0.2 && cell.x != 0 && cell.y != 0 && cell.x < gridWidth-1 && cell.y < gridHeight-1){
                int i = Random.Range(0, Tree.Length-1);
                Instantiate(Tree[i], new Vector3(cell.x, 0.75f, cell.y), Quaternion.identity);
            }else if(index >= 0.15 && index < 0.5){
                Instantiate(grass, new Vector3(cell.x, 0.3f, cell.y), Quaternion.identity);
            }else{
                continue;
            }
            yield return new WaitForSeconds(0.007f);
        }
        yield return null;
    }
}
