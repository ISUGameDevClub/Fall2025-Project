using UnityEngine;

public class PlayerVariantData : MonoBehaviour
{
    [SerializeField] private PlayerCharacter character;

    public PlayerCharacter GetPlayerCharacter() { return character; }
}
