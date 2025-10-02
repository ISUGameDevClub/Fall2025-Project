using UnityEngine;
using TMPro;
using System.Collections;

public class WinDialogue : MonoBehaviour
{

    public TMP_Text winDialogue;
    public string textToShow = "Eat Justice";
    private string textShown = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("DisplayTextStaggered");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DisplayTextStaggered()
    {
        //Debug.Log("im firat");
        //yield return new WaitForSeconds(1f);
        //Debug.Log("im second");
        for (int i = 0; i < textToShow.Length; i++)
        {
            textShown += textToShow[i];
            winDialogue.text = textShown;
            yield return new WaitForSeconds(.1f);
        }
    }
}
