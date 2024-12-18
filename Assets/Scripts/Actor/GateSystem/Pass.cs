using UnityEngine;
using UnityEngine.Events;

public class Pass : MonoBehaviour
{
    [Header("Pass References")]
    [SerializeField] private UnityEvent activateEvents;

    private bool _activationInvoked = false;
    private PassHole[] _passHoles;

    #region Unity Events

    private void Awake()
    {
        _passHoles = GetComponentsInChildren<PassHole>();
    }

    private void Update()
    {
        CheckActivation();
    }

    #endregion

    private void CheckActivation()
    {
        // Check conditions
        if (_activationInvoked) return;
        foreach (var hole in _passHoles)
            if (!hole.IsActivated) return;
        // Activate
        activateEvents.Invoke();
        _activationInvoked = true;
    }
}
