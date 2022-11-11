using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridmanager : MonoBehaviour
{
    private int gridWidth = 30;
    private int gridHeight = 15;
    public int minPathLength = 30;

    private Pathgenerator pathgenerator;
    [Header("Blöcke")]
    public GameObject pathTile;
    public GameObject buildTile;

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

    //Spawnt die Path Blöcke + Wavespawner am ersten Block
    private IEnumerator BauePath(List<Vector2Int> pathcells){
        enemiePath = new List<GameObject>();
            foreach(Vector2Int cell in pathcells){
                Instantiate(pathTile, new Vector3(cell.x, 0f, cell.y), Quaternion.identity);
                GameObject path = Instantiate(waypoint, new Vector3(cell.x, 1f, cell.y), Quaternion.identity);
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
            if(index >= 0.0 && index < 0.05f){
                Instantiate(Kiste, new Vector3(cell.x, 0.85f, cell.y), Quaternion.identity);
            }else if(index >= 0.1 && index < 0.17){
                int i = Random.Range(0, Tree.Length-1);
                Instantiate(Tree[i], new Vector3(cell.x, 0.5f, cell.y), Quaternion.identity);
            }else if(index >= 0.2 && index < 0.5){
                Instantiate(grass, new Vector3(cell.x, 0.7f, cell.y), Quaternion.identity);
            }else{
                continue;
            }
            yield return new WaitForSeconds(0.007f);
        }
        yield return null;
    }
}
