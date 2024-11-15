using UnityEngine;

public class Platform : Interactable
{
    private BoxCollider2D _collider;
    private SpriteRenderer _sprite;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _collider = GetComponent<BoxCollider2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        _collider.size = new Vector2(sprite.size.x, sprite.size.y);
    }

    #endregion

    public override bool OnInteracted(Actor actor)
    {
        return false;
    }

    public void SetSolid(bool isSolid)
    {
        sprite.color = isSolid ? Color.white : new Color(1f, 1f, 1f, 0.0625f);
        _sprite.sortingOrder = isSolid ? 0 : -11;
        _collider.enabled = isSolid;
    }
}