using UnityEngine;

public class SunCannon : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private Vector2 trajectory;
    [SerializeField] private int resolution = 30; // Number of points
    [SerializeField] private float timeStep = 0.1f; // Time between points
    [Tooltip("Have this checked if the projectile uses gravity/physics. Unchecked will just draw straight line.")]
    [SerializeField] private bool usesPhysics;

    private void OnDrawGizmos()
    {
        // Safety check
        if (resolution <= 1) return;

        if (usesPhysics)
        {
            Gizmos.color = Color.yellow;

            Vector2 startPos = transform.position;
            Vector2 velocity = trajectory;

            Vector2 previousPoint = startPos;

            for (int i = 1; i <= resolution; i++)
            {
                float t = i * timeStep;

                // Physics formula: p(t) = p0 + v0 * t + 0.5 * g * t²
                Vector2 gravity = Physics2D.gravity;
                Vector2 newPoint = startPos + velocity * t + 0.5f * gravity * t * t;

                Gizmos.DrawLine(previousPoint, newPoint);
                previousPoint = newPoint;
            }
        }
        else
        {
            Gizmos.color = Color.cyan;

            Vector2 startPos = transform.position;
            Vector2 previousPoint = startPos;

            for (int i = 1; i <= resolution; i++)
            {
                float t = i * timeStep;
                Vector2 newPoint = startPos + trajectory * t;
                Gizmos.DrawLine(previousPoint, newPoint);
                previousPoint = newPoint;
            }
        }
    }

    public Vector2 GetTrajectory()
    {
        return trajectory.normalized;
    }
}
