using UnityEngine;

public class Star : Interactable
{
    [Header("Effects References")]
    [SerializeField] private ParticleSystem splashPrefab;
    [SerializeField] private AudioSource collectAudio;

    private Player[] _players;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();
        _players = FindObjectsByType<Player>(FindObjectsSortMode.None);
    }

    #endregion

    public override bool OnInteracted(Actor actor)
    {
        if (!base.OnInteracted(actor)) return false;

        actor.Reactivate();

        // Play effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);

        // Play audio
        collectAudio.transform.SetParent(null);
        collectAudio.Play();
        Destroy(collectAudio.gameObject, 1f);

        // Player speech
        foreach (var player in _players) player.Talk("Yeah!");

        return false;
    }
}