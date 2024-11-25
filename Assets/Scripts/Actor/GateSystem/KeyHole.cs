using UnityEngine;
using UnityEngine.Events;

public class KeyHole : Actor
{
    [Header("Key Hole References")]
    [SerializeField] private Sprite activatedSprite;
    [SerializeField] private UnityEvent activateEvents;
    [SerializeField] private AudioSource activateAudio;

    public void OnActivated()
    {
        sprite.sprite = activatedSprite;
        Reactivate();

        activateEvents.Invoke();
        activateAudio.Play();
    }
}
