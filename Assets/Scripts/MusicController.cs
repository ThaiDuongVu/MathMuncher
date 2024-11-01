using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private MusicTrack[] _tracks;

    #region Unity Events

    private void Awake()
    {
        _tracks = GetComponentsInChildren<MusicTrack>();
    }

    private void Start()
    {
        StartCoroutine(PlayRandom());
    }

    #endregion

    private IEnumerator PlayRandom()
    {
        var track = _tracks[Random.Range(0, _tracks.Length)];
        track.Play();

        yield return new WaitForSeconds(track.Duration);
        StartCoroutine(PlayRandom());
    }
}
