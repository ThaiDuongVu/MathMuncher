using UnityEngine;

public class MathController : MonoBehaviour
{
    #region Singleton

    private static MathController _mathControllerInstance;

    public static MathController Instance
    {
        get
        {
            if (_mathControllerInstance == null) _mathControllerInstance = FindFirstObjectByType<MathController>();
            return _mathControllerInstance;
        }
    }

    #endregion
}
