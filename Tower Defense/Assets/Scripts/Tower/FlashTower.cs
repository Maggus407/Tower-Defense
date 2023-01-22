using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashTower : MonoBehaviour
{
    private List<GameObject> enemiesInRange = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void Update()
    {
        if(enemiesInRange.Count > 0)
        {

        }
    }
}
