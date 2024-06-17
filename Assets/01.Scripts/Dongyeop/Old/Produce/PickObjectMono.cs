using Define;
using UnityEngine;

public abstract class PickObjectMono : MonoBehaviour
{
    [Header("PickObject")]
    public bool IsBurger = false;
    public Ingredient Ingredient;

    public virtual Transform OnPick()
    {
        return null;
    }
}