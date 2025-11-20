using UnityEngine;
using UnityEngine.UI;

public class TransitionButton : MonoBehaviour
{
	[SerializeField] private GameSequenceManager.Sequence NextSequence;
	[SerializeField] private Button Button;

	private void Start()
	{
		Button.onClick.AddListener( OnClick );
	}

	private void OnClick()
	{
		GameSequenceManager.SetSequence( NextSequence );
		Button.onClick.RemoveAllListeners();
	}
};