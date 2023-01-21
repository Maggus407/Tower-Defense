using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    /* private List<GameObject> enemies; */
    private Vector3 currentEnemie;
    private Transform target;
    private Enemy enemy;

    [HeaderAttribute("General")]
    public float range = 10f;

    [HeaderAttribute("Use Bullets (default)")]
    public float fireRate = 2f;
    private float fireCountDown = 0f;
    
    [HeaderAttribute("Use Laser")]
    public int damageOverTime = 30;
    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

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
            target = nearestEnemy.transform;
            enemy = nearestEnemy.GetComponent<Enemy>();
        }else{
            target = null;
        }
    }

    void Update()
    {
        if(target == null){
            if(useLaser){
                if(lineRenderer.enabled){
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
            return;
        }
        LockOntarget();

        if(useLaser){
            Laser();
        }else{
            //shooting
            if(fireCountDown <= 0){
                Shoot();
                fireCountDown = 1f / fireRate;
            }
            fireCountDown -= Time.deltaTime;
        }

    }

    void LockOntarget(){
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void Shoot()
    {
       
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
      
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null){
            bullet.Seek(target);
        }
    }

    void Laser(){

        enemy.TakeDamage(damageOverTime * Time.deltaTime);
        if(!lineRenderer.enabled){
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;

        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
        Vector3 dir = firePoint.position - target.position;
        impactEffect.transform.position = target.position + dir.normalized * 0.25f;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
        
      
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
