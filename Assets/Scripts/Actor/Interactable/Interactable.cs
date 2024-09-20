using UnityEngine;

public class Interactable : Actor
{
    public virtual bool OnInteracted(Actor actor)
    {
        if (!actor) return false;
        return true;
    }
}
