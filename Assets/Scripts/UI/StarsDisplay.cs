using UnityEngine;
using UnityEngine.UI;

public class StarsDisplay : MonoBehaviour
{
    [SerializeField] private Image[] icons;
    private const int MaxStars = 3;

    public void SetStars(int stars)
    {
        gameObject.SetActive(false);

        for (var i = 0; i < stars; i++) icons[i].gameObject.SetActive(true);
        for (var i = stars; i < MaxStars; i++) icons[i].gameObject.SetActive(false);

        gameObject.SetActive(true);
    }
}
