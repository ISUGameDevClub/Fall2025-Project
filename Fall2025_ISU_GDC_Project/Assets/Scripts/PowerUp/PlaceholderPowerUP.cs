using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceholderPowerUP : MonoBehaviour
{

    [SerializeField] private float lifeTime;
    [SerializeField] private float bobHeight;
    float counter;
    float startY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(powerUpActive());
        counter = 0;
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Mathf.PI * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, startY + Mathf.Sin(counter) * bobHeight, transform.position.z);

    }

    private IEnumerator powerUpActive()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    
}
