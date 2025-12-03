using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlayerCharacter", menuName = "Scriptable Objects/PlayerCharacter")]
public class PlayerCharacter : ScriptableObject
{
    public string characterName;
    public Sprite characterSelectIcon;
    public string victoryText;
    public Sprite victorySprite;
}
