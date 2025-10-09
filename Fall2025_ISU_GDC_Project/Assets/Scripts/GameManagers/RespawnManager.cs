using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private Transform playerRespawnPoint;

    /// <summary>
    /// Takes in a playerHealth Obj, handles the respawn logic for that playerHealth
    /// </summary>
    public void RespawnPlayer(PlayerHealth playerHealth)
    {
        var playerObj = playerHealth.gameObject;

        playerObj.transform.position = playerRespawnPoint.position;
    }
}
