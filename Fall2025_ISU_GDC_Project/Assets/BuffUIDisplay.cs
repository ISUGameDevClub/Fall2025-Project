using UnityEngine;


public class BuffUIDisplay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject[] buffSymbols;
    [SerializeField] GameObject[] arrows;
    PlayerHealth healthRef;
    PlayerMovement moveRef;
    HitboxProperties hitboxRef;

    
    void Start()
    {
        healthRef = GetComponent<PlayerHealth>();
        moveRef = GetComponent<PlayerMovement>();
        hitboxRef = GetComponentInChildren<HitboxProperties>();
    }

    // Update is called once per frame
    void Update()
    {
        //Horrible way to code this but im in a hurry so it will do /:
        if (hitboxRef.damageBoost != 1)
        {
            DisplayBuff(0,hitboxRef.damageBoost);
        }
        else
        {
            buffSymbols[0].SetActive(false);
        }

         if (moveRef.speedBoost != 1)
        {
           DisplayBuff(1,moveRef.speedBoost);
        }
        else
        {
            buffSymbols[1].SetActive(false);
        }
         if (healthRef.defMultiplier != 1)
        {
            DisplayBuff(2,healthRef.defMultiplier,-1);
        }
        else
        {
            buffSymbols[2].SetActive(false);
        }
    }


    public void DisplayBuff(int index,float valueToCheck, int multiplierForCheck = 1)
    {
        buffSymbols[index].SetActive(true);
        if (valueToCheck * multiplierForCheck > 1 * multiplierForCheck)
        {
            arrows[index].GetComponent<SpriteRenderer>().color = Color.green;
            arrows[index].transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            arrows[index].GetComponent<SpriteRenderer>().color = Color.red;
            arrows[index].transform.localEulerAngles = new Vector3(0, 0, 180);
        }
    }
        

}
