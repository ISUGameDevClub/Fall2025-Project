using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] powerUps;
    [SerializeField] float minSpawnTime = 10f;
    [SerializeField] float maxSpawnTime = 25f;
    float spawnTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTimer = Time.time + Random.Range(minSpawnTime,maxSpawnTime + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer <= Time.time)
        {
            spawnPowerUp();
            spawnTimer = Time.time + Random.Range(minSpawnTime,maxSpawnTime + 1);
        }
        
    }


    public void spawnPowerUp()
    {
        int rng = Random.Range(0,powerUps.Count());
        GameObject powerUp1 = Instantiate(powerUps[rng]);
        powerUp1.transform.position = new Vector3(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2), transform.position.y, transform.position.z);
    }

}
