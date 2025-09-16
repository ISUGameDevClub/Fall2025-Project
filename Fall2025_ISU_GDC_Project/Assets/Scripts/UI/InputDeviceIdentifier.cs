using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputDeviceIdentifier : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deviceText;
    [SerializeField] private Image deviceIcon;
    [SerializeField] private Image deviceColor;

    public void UpdateIdentifierData(Sprite img, string text, Color color)
    {
        deviceIcon.sprite = img;
        deviceText.text = text;
        deviceColor.color = color;
    }
}
