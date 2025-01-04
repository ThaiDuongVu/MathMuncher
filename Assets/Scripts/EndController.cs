using UnityEngine;

public class EndController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private SimpleMenu endMenu;
    private bool _endMenuEnabled;

    #region Unity Events

    private void Start()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.01666667f * Time.timeScale;
    }

    private void Update()
    {
        if (!_endMenuEnabled && player.transform.position.y > 5)
        {
            endMenu.SetActive(true);
            _endMenuEnabled = true;

            // Play effects
            CameraShaker.Instance.Shake(CameraShakeMode.Normal);
            PostProcessingController.Instance.SetDepthOfField(true);
            Destroy(player.gameObject);
        }
    }

    #endregion
}
