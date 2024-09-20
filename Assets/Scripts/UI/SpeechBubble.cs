using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void SetText(string message)
    {
        text.SetText(message);
    }
}
