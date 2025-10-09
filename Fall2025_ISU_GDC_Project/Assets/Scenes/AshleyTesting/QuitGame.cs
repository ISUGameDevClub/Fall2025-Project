using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour
{
    public Button quitButton;

	void Start () {
		Button btn = quitButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
        Debug.Log("quiting the game...");
	}
}
