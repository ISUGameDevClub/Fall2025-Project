using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private GameObject creditsMenu;

    private void Start()
    {
        levelSelectMenu.SetActive(false);
    }

    //called by button OnClick
    public void OpenLevelSelectMenu()
    {
        levelSelectMenu.SetActive(true);
    }

    public void OpenCreditsMenu()
    {
        creditsMenu.SetActive(true);
    }

    public void CloseCreditsMenu()
    {
        creditsMenu.SetActive(false);
    }
}
