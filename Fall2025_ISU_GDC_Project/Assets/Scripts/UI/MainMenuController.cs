using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectMenu;

    private void Start()
    {
        levelSelectMenu.SetActive(false);
    }

    //called by button OnClick
    public void OpenLevelSelectMenu()
    {
        levelSelectMenu.SetActive(true);
    }
}
