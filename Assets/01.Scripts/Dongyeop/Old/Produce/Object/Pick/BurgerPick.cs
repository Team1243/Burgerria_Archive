using UnityEngine;

public class BurgerPick : PickObjectMono
{
    public override Transform OnPick()
    {
        return transform.root;
    }
}
