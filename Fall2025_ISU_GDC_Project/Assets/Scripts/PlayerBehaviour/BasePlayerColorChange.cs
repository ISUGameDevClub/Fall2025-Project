using UnityEngine;

public class BasePlayerColorChange : MonoBehaviour
{
    //this script just assigns a random color to the sprite of a newly added player. 
    //Will be removed once actual assets are in
    private void Start()
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            //GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 0.8f, 1f,0.8f, 1f);
        }
        this.gameObject.name = "Player | ID: " + Random.Range(1000, 9999); //i just give the player a random id here for clarity's sake
    }
}
