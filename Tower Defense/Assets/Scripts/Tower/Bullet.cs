using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactEffekt;
    private Transform target;
    public float speed = 40f;
    public float explosionRadius = 0f;
    public void Seek(Transform _target){
        target = _target;
    }
    void Update()
    {
        if(target == null){
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        
        if(dir.magnitude <= distanceThisFrame){
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    private void HitTarget()
    {
        GameObject effectInstance = (GameObject)Instantiate(impactEffekt, transform.position, transform.rotation);
        Destroy(effectInstance, 5f);
        if (explosionRadius > 0){
            Explode();
        }else{
            Damage(target);
        }
        Destroy(gameObject);
    }

    void Damage(Transform enemy){
        enemy.gameObject.GetComponent<Enemy>().TakeDamage(10);
    }

    private void Explode(){
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in hitObjects){
            if(collider.tag == "Enemy"){
                Damage(collider.transform);
            }
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

    }
}
