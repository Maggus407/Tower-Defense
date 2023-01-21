using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float startSpeed = 10f;

    [HideInInspector]
    public float speed;

    public float startHealth = 100;
    public float health;

    public int worth = 50;

    private bool isDead = false;

    void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }

    public void TakeDamage (float amount)
    {
        health -= amount;
       
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        WaveSpawner.EnemiesAlive--;

        Destroy(gameObject);
    }
}
