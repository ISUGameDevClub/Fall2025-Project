using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{

    [SerializeField] float timeInterval;
    private PowerUpSpawner[] powerUpSpawners;
    private float totalWeight;
    private float[] weightCutoffs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerUpSpawners = FindObjectsByType<PowerUpSpawner>(FindObjectsSortMode.None);
        weightCutoffs = new float[powerUpSpawners.Length];
        for(int i = 0; i < powerUpSpawners.Length; i++)
        {
            totalWeight += powerUpSpawners[i].transform.lossyScale.x;
            weightCutoffs[i] = totalWeight;
        }
        StartCoroutine(spawnPowerUps());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator spawnPowerUps()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeInterval);
            float n = Random.Range(0, totalWeight);
            int i = 0;
            while (n > weightCutoffs[i])
            {
                i++;
            }
            powerUpSpawners[i].spawnPowerUp();
            
        }
    }
}
