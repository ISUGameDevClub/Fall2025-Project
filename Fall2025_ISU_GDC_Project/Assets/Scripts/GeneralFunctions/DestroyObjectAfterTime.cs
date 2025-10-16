using UnityEngine;

public class DestroyObjectAfterTime : MonoBehaviour
{
    [SerializeField] private float destroyAfterTime;

    private void Start()
    {
        Invoke("DestroyObject", destroyAfterTime);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
