using UnityEngine;

public abstract class PutObjectMono : MonoBehaviour
{
    [Header("PutObject")]
    [HideInInspector] public Burger currentBurger = null;
    public PutObjectType Type; 
    
    public virtual void OnPut(PickObjectMono pickObject, Transform currentObject)
    {
    }
}
