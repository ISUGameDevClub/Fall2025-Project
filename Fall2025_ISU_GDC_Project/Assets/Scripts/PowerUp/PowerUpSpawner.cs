using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] GameObject powerUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void spawnPowerUp()
    {
        GameObject powerUp1 = Instantiate(powerUp);
        powerUp1.transform.position = new Vector3(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2), transform.position.y, transform.position.z);
    }

}
