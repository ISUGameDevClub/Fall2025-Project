using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UltimateTrackerManager : MonoBehaviour
{
    
    //a value between 0.0<->100.0+ will be stored. Overflow past 100.0 will happen, if player has not used their ultimate for some time
    Dictionary<PlayerInput, float> playerInputToUltimatePercentage_dict = new Dictionary<PlayerInput, float>();

    public void InitializeDictionary(List<PlayerInput> inputList)
    {
        playerInputToUltimatePercentage_dict.Clear();
        foreach (PlayerInput input in inputList)
        {
            playerInputToUltimatePercentage_dict.Add(input, 0);
        }
    }

    public void AddUltimateCharge(PlayerInput pi, float amt)
    {
        float curVal = playerInputToUltimatePercentage_dict[pi];
        playerInputToUltimatePercentage_dict[pi] = curVal + amt;
    }

    public void ResetPlayerUltimateCharge(PlayerInput pi)
    {
        playerInputToUltimatePercentage_dict[pi] = 0;
    }

    public float GetUltimatePercentageForPlayer(PlayerInput pi)
    {
        return playerInputToUltimatePercentage_dict[pi];
    }

    public bool CanPlayerUseUltimate(PlayerInput pi)
    {
        return playerInputToUltimatePercentage_dict[pi] >= 100f;
    }
}
