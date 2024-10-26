using UnityEngine;

public class Teleporter : Actor
{
    [Header("Teleporter References")] 
    [SerializeField] private Teleporter connectedNode;

    private Vector2 Position => transform.position;

    [Header("Effects References")]
    [SerializeField] private ParticleSystem splashPrefab;
    [SerializeField] private LineRenderer connectLine;

    #region Unity Events

    protected override void Start()
    {
        base.Start();

        connectLine.positionCount = 2;
        connectLine.SetPositions(new Vector3[] { Position, connectedNode.Position });
    }

    #endregion

    public void OnTeleported(Actor actor)
    {
        if (!actor) return;

        actor.Teleport(connectedNode.Position);
    }

    public void PlayEffects()
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, Position, Quaternion.identity);
        Instantiate(splashPrefab, connectedNode.Position, Quaternion.identity);
    }
}