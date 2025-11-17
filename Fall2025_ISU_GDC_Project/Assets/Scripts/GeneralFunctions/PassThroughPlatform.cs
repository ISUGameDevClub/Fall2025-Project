using System.Collections;
using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Collider2D _collider;
    private bool _onPlatform;


    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_onPlatform && Input.GetAxisRaw("Vertical") < 0)
        {
            _collider.enabled = false;
            StartCoroutine(EnableCollider());
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        _collider.enabled = true;
    }

    private void SetPlayerOnPlatform(Collision2D other, bool value)
    {
        var player = other.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            _onPlatform = value;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, value:true);
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, value: true);
    }
}
