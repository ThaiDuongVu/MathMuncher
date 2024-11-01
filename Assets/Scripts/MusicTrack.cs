using UnityEngine;

public class MusicTrack : MonoBehaviour
{
    private AudioSource _audioSource;
    public float Duration => _audioSource.clip.length;

    #region Unity Events

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    #endregion

    public void Play()
    {
        _audioSource.Play();
    }
}
