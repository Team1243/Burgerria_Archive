using UnityEngine;

public class Trash : PutObjectMono
{
    public override void OnPut(PickObjectMono pickObject, Transform currentObject)
    {
        base.OnPut(pickObject, currentObject);
        
        if (!pickObject)
            return;
        
        Destroy(currentObject.gameObject);
    }
}
