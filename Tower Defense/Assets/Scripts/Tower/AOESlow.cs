using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESlow : MonoBehaviour
{
    List<GameObject> currentEnemies;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().speed = other.GetComponent<Enemy>().startSpeed / 2;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().speed = other.GetComponent<Enemy>().startSpeed;
        }
    }
}
