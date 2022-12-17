using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEnemies : MonoBehaviour
{
    /* private List<GameObject> enemies; */
    private Vector3 currentEnemie;
    private Transform target;

    [HeaderAttribute("Attributes")]
    public float fireRate = 2f;
    private float fireCountDown = 0f;
    public float range = 10f;

    [HeaderAttribute("Unity Setup")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10;
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Start(){
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach(GameObject enemy in enemies){
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance){
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if(nearestEnemy != null && shortestDistance <= range){
            Debug.Log("new Target transform: " + nearestEnemy.transform);
            target = nearestEnemy.transform;
        }else{
            target = null;
        }
    }

    void Update()
    {
        if(target == null){
            return;
        }
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        //shooting
        if(fireCountDown <= 0){
            Shoot();
            fireCountDown = 1f / fireRate;
        }
        fireCountDown -= Time.deltaTime;
    }

    private void Shoot()
    {
        Debug.Log("Shoot");
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Debug.Log("Bullet Name: " + bulletGO.name);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null){
            bullet.Seek(target);
        }
    }
    /* void Start(){
        enemies = new List<GameObject>();
        currentEnemie = new Vector3();
    } */

    /* void Update(){
        if(enemies.Count > 0){
            AttackFirstEnemie();
        }
        Debug.Log(enemies.Count);
    } */

    //Target locks on first Enemie in the List
    /* private void AttackFirstEnemie(){
        currentEnemie = enemies[0].transform.position;
        Vector3 dir = currentEnemie - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rot = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rot.y, 0f);
    } */

    /* void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag =="Enemy"){
            enemies.Add(collider.gameObject);
            Debug.Log("HALL");
        }
    } */

    /* void OnTriggerExit(Collider collider){
        enemies.Remove(collider.gameObject);
    } */

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
