using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{

    public Transform enemyPrefab;
    public Transform spawnPoint;

    public static int EnemiesAlive = 0;

    public float timeBetweenWaves = 3f;
    
    private float countdown = 2f;

    private int waveNumber = 0;

    public Wave[] waves;

    public Wave[] easyWaves = new Wave[9];
    public Wave[] MediumWaves = new Wave[19];
    public Wave[] HardWaves = new Wave[29];

    public static int difficulty;

    void Update(){

        if(EnemiesAlive > 0)
        {
            return;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        //Zählt die vergangene Zeit seit dem letzten Frame Update
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave(){

        PlayerStats.Rounds++;

        Wave wave = easyWaves[waveNumber];

        switch (difficulty)
        {
            case 0:
                wave = easyWaves[waveNumber];
                break;
            case 1:
                wave = MediumWaves[waveNumber];
                break;
            case 2:
                wave = HardWaves[waveNumber];
                break;
        }
        

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            //waiting a amount of time before Spawning new Enemy
            yield return new WaitForSeconds(1f / wave.rate);
        }

        waveNumber++;
    }

    //Spawnt den Enemie am WaveSpawner
    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        EnemiesAlive++;
    }

}
