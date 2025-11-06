using UnityEngine;

public class CoR_Ultimate_Arrow : MonoBehaviour
{
    private Transform travelTo;
    private float speed;
    private CoR_Ultimate_Activator activator;

    public void SetPath(Transform t, float s, CoR_Ultimate_Activator a)
    {
        travelTo = t;
        speed = s;
        activator = a;

        Vector3 direction = travelTo.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void Update()
    {
        // Move our position a step closer to the target.
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, travelTo.position, step);
        

        //check if we are at target
        if (Vector3.Distance(transform.position, travelTo.position) < 0.001f)
        {
            activator.StartShootingFireballs();
            Destroy(this.gameObject);
        }
    }
}
