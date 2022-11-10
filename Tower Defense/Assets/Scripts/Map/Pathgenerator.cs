using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathgenerator : MonoBehaviour
{
    private int width, height;
    private List<Vector2Int> pathcells;
    private List<Vector2Int> buildcells;

    public Pathgenerator(int width, int height){
        this.width = width;
        this.height = height;
    }

    //Erstellt einen Random Path
    public List<Vector2Int> GeneratePath(){
        pathcells = new List<Vector2Int>();
        int y = (int)(height / 2);
        int x = 0;
        //Creates Random Path in one Direction without loops
        while(x<width){
            bool validMoves = false;
            pathcells.Add(new Vector2Int(x, y));
            
            while(!validMoves){
                int move = Random.Range(0,3);
                //Gerade aus
                if(move == 0 || x % 2 == 0 || x > (width-2)){
                    x++;
                    validMoves = true;
                //Nach oben
                }else if(move == 1 && CellisFree(x, y+1) && y < (height-3)){
                    y++;
                    validMoves = true;
                //Nach unten
                }else if(move == 2 && CellisFree(x, y-1) && y > 2){
                    y--;
                    validMoves = true;
                }
            }
        }

        return pathcells;
    }

    //Füllt das Grid mit den restlichen Blöcken auf
    public List<Vector2Int> BuildCells(){
        buildcells = new List<Vector2Int>();
        for (int x = 0; x < width; x++){
            for (int y = 0; y < height; y++){
                if(CellisFree(x,y)){
                    buildcells.Add(new Vector2Int(x, y));
                }
            }            
        }
        return buildcells;
    }

    //Überprüft ob die Celle zum Bauen schon belegt ist
    private bool CellisFree(int x, int y){
        return !pathcells.Contains(new Vector2Int(x,y));
    }
}
