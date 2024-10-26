public class Interactable : Actor
{
    public virtual bool OnInteracted(Actor actor)
    {
        return actor;
    }
}