using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactEffekt;
    private Transform target;
    public float speed = 40f;

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
    }

    private void HitTarget()
    {
        GameObject effectInstance = (GameObject)Instantiate(impactEffekt, transform.position, transform.rotation);
        Destroy(effectInstance, 2f);
        Destroy(target.gameObject);
        Destroy(gameObject);
    }
}
