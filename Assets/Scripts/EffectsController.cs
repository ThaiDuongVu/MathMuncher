using UnityEngine;

public class EffectsController : MonoBehaviour
{
    #region Singleton

    private static EffectsController _effectsControllerInstance;

    public static EffectsController Instance
    {
        get
        {
            if (_effectsControllerInstance == null) _effectsControllerInstance = FindFirstObjectByType<EffectsController>();
            return _effectsControllerInstance;
        }
    }

    #endregion

    [SerializeField] private SpeechBubble speechBubblePrefab;

    public void SpawnSpeechBubble(Vector2 position, string message)
    {
        var speechBubble = Instantiate(speechBubblePrefab, position, Quaternion.identity);
        speechBubble.SetText(message);
    }
}
