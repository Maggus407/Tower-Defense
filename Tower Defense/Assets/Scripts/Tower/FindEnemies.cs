using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEnemies : MonoBehaviour
{
    private List<GameObject> enemies;
    private Vector3 currentEnemie;

    public float turnSpeed = 10;

    void Start(){
        enemies = new List<GameObject>();
        currentEnemie = new Vector3();
    }

    void Update(){
        if(enemies.Count > 0){
            AttackFirstEnemie();
        }
        Debug.Log(enemies.Count);
    }

    //Target locks on first Enemie in the List
    private void AttackFirstEnemie(){
        currentEnemie = enemies[0].transform.position;
        Vector3 dir = currentEnemie - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rot = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rot.y, 0f);
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag =="Enemy"){
            enemies.Add(collider.gameObject);
            Debug.Log("HALL");
        }
    }

    void OnTriggerExit(Collider collider){
        enemies.Remove(collider.gameObject);
    }

}
