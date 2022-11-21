using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinder : MonoBehaviour
{

     [Header("Enemie-Speed")]
   public float speed = 2f;

   private Transform target;
   private int wavepointIndex = 0;

   List<GameObject> pathfinder = Gridmanager.enemiePath;

   void Start(){
        target = pathfinder[wavepointIndex].transform;
        //Ignoriert alle Collisions --> Damit Enemies sich auch überholen können
        Physics.IgnoreLayerCollision(0, 6, true);
   }

   void Update(){
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if(Vector3.Distance(transform.position, target.position) <= 0.1f){
            GetNextWaypoint();
        }
   }

     //Wenn der Enemie den letzten Block erreicht hat zerstört er sich
   void GetNextWaypoint(){
        if(wavepointIndex >= pathfinder.Count - 1){
            Destroy(gameObject);
            return;
        }
        wavepointIndex++;
        target = pathfinder[wavepointIndex].transform;
   }
}
