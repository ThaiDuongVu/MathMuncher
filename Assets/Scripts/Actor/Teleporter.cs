using UnityEngine;

public class Teleporter : Actor
{
    [Header("Teleporter References")]
    [SerializeField] private Teleporter connectedNode;

    private Vector2 Position => transform.position;

    [Header("Effects References")]
    [SerializeField] private ParticleSystem splashPrefab;
    [SerializeField] private LineRenderer connectLine;

    private Animator _animator;
    private static readonly int TeleportAnimationTrigger = Animator.StringToHash("teleport");

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
    }

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

        // Play teleport effects
        Pop();
        connectedNode.Pop();
    }

    public void Pop()
    {
        _animator.SetTrigger(TeleportAnimationTrigger);
    }

    public void PlayEffects()
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(splashPrefab, Position, Quaternion.identity);
        Instantiate(splashPrefab, connectedNode.Position, Quaternion.identity);
    }
}