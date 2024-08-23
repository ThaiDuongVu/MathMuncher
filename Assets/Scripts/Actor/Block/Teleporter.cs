using UnityEngine;

public class Teleporter : Block
{
    public Vector2 Position => transform.position;

    [Header("Teleporter References")]
    [SerializeField] private ParticleSystem teleportSplashPrefab;
    [SerializeField] private LineRenderer connectLine;
    [SerializeField] private Teleporter connectedTeleporter;

    private static readonly int ActivateAnimationTrigger = Animator.StringToHash("activate");

    #region Unity Events

    protected override void Update()
    {
        base.Update();

        connectLine.SetPosition(0, Position);
        connectLine.SetPosition(1, connectedTeleporter.Position);
    }

    #endregion

    public void OnActivated(Block block)
    {
        if (!connectedTeleporter) return;

        // Set new block position
        var connectedPosition = connectedTeleporter.Position;
        block.transform.position = connectedPosition;
        block.TargetPosition = connectedPosition;

        Animator.SetTrigger(ActivateAnimationTrigger);
        Instantiate(teleportSplashPrefab, transform.position, Quaternion.identity);
    }

    public void OnActivated(Player player)
    {
        if (!connectedTeleporter) return;

        // Set new player position
        var connectedPosition = connectedTeleporter.Position;
        player.transform.position = connectedPosition;
        player.TargetPosition = connectedPosition;

        Animator.SetTrigger(ActivateAnimationTrigger);
        Instantiate(teleportSplashPrefab, transform.position, Quaternion.identity);
    }

    public override bool Move(Vector2 direction)
    {
        return true;
    }
}
