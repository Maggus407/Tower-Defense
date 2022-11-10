using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    //Timer before it spawns the first Wave
    private float countdown = 2f;

    private int waveNumber = 1;

    void Update(){
        if(countdown <= 0){
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        //Zählt die vergangene Zeit seit dem letzten Frame Update
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave(){
        for (int i = 0; i < waveNumber; i++)
        {
            SpawnEnemy();
            //waiting a amount of time before Spawning new Enemy
            yield return new WaitForSeconds(0.3f);
        }

        waveNumber++;
    }

    //Spawnt den Enemie am WaveSpawner
    void SpawnEnemy(){
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

}
