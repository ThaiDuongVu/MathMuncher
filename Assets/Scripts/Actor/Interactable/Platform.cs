using UnityEngine;

public class Platform : Interactable
{
    #region Unity Events

    protected override void Start()
    {
        _boxCollider.size = new Vector2(sprite.size.x, sprite.size.y) - new Vector2(0.125f, 0.125f);
        base.Start();
    }

    #endregion

    public override bool OnInteracted(Actor actor)
    {
        return false;
    }

    public void SetSolid(bool isSolid)
    {
        sprite.color = isSolid ? Color.white : new Color(1f, 1f, 1f, 0.0625f);
        sprite.sortingOrder = isSolid ? 0 : -11;
        _boxCollider.enabled = isSolid;
        Level.Instance.SendUIMessage(isSolid ? "Wall locked" : "Wall unlocked");
    }
}