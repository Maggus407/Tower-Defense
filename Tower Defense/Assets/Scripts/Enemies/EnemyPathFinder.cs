using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinder : MonoBehaviour
{

   private Transform target;
   private int wavepointIndex = 0;
   List<GameObject> pathfinder = Gridmanager.enemiePath;
    private Enemy enemy;

   void Start(){

        enemy = GetComponent<Enemy>();
        
        target = pathfinder[wavepointIndex].transform;
        //Ignoriert alle Collisions --> Damit Enemies sich auch überholen können
        Physics.IgnoreLayerCollision(0, 6, true);
   }



   void Update(){
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        if(Vector3.Distance(transform.position, target.position) <= 0.1f){
            GetNextWaypoint();
        }
   }

     //Wenn der Enemie den letzten Block erreicht hat zerstört er sich
   void GetNextWaypoint(){
        if(wavepointIndex >= pathfinder.Count - 1){
            EndPath();
            return;
        }
        wavepointIndex++;
        target = pathfinder[wavepointIndex].transform;
   }

    void EndPath()
    {
        if(PlayerStats.Lives > 0)
        {
            PlayerStats.Lives--;
        }
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }

}
