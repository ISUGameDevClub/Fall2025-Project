using Unity.VisualScripting;
using UnityEngine;

public class Temp : MonoBehaviour
{
    [SerializeField] private float boxSpeed = 2.0f;
    [SerializeField] private float switchDirectionAfterTime;
    private float timer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = switchDirectionAfterTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer > 0)
        {
            transform.Translate(Vector3.left * Time.deltaTime * boxSpeed, Space.World);
        } else if (timer < 0 && timer >-switchDirectionAfterTime)
        {
            transform.Translate(Vector3.right * Time.deltaTime * boxSpeed, Space.World);
        }
        else
        {
            timer = switchDirectionAfterTime;
        }
        
        
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        //if (col.gameObject.CompareTag("Player"))
        //{
            collider.gameObject.transform.parent = transform;
        //}
    }

    private void OnCollisionExit2D(Collision2D collider)
    {
        //if (col.gameObject.CompareTag("Player"))
        //{
            collider.gameObject.transform.parent = null;
        //}
    }
}
