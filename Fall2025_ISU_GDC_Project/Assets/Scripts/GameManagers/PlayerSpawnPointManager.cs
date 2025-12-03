using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;

public class PlayerSpawnPointManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    public void SetPlayerSpawnPoints(List<GameObject> players)
    {
        int curSpawnindex = 0;

        foreach (GameObject player in players)
        {
            player.transform.position = spawnPoints[curSpawnindex].position;
            curSpawnindex++;
        }
    }

}
