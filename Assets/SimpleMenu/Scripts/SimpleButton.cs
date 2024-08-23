using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimpleButton : MonoBehaviour
{
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private TMP_Text subText;
    [SerializeField] private Image icon;

    public void SetMainText(string text)
    {
        mainText.SetText(text);
    }

    public void SetSubText(string text)
    {
        subText.SetText(text);
    }

    public void SetIcon(Sprite sprite)
    {
        var iconColor = icon.color;
        icon.sprite = sprite;
        iconColor.a = sprite == null ? 0f : 1f;
        icon.color = iconColor;
    }

    public void SetIconSize(float width, float height)
    {
        icon.rectTransform.sizeDelta = new Vector2(width, height);
    }
}
