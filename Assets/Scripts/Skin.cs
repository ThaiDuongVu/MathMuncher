using UnityEngine;

public class Skin : MonoBehaviour
{
    [Header("Stats")]
    public new string name;
    public int cost;

    [Header("References")]
    public RuntimeAnimatorController frontAnimator;
    public RuntimeAnimatorController backAnimator;
    public RuntimeAnimatorController sideAnimator;
}
