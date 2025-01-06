using System.Linq;
using TMPro;
using UnityEngine;

public class EndController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private SimpleMenu endMenu;
    [SerializeField] private TMP_Text statsText;

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

            // Update stats
            var data = SaveLoadController.Instance.LoadPerma();
            statsText.SetText(
                $"Total stars collected: {data.levelRatings.Sum()}\nLevels 3-starred: {data.levelRatings.Count(rating => rating == 3)}");

            // Play effects
            CameraShaker.Instance.Shake(CameraShakeMode.Normal);
            PostProcessingController.Instance.SetDepthOfField(true);
            Destroy(player.gameObject);
        }
    }

    #endregion
}
