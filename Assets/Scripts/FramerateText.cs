using TMPro;
using UnityEngine;

public class FramerateText : MonoBehaviour
{
    private TMP_Text _text;

    #region Unity Events

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _text.SetText((1f / Time.deltaTime).ToString("N2"));
    }

    #endregion
}
