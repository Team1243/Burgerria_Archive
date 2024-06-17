using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderBridge : MonoBehaviour
{
    public IngredientMono Owner { private set; get; }

    public Ingredient ObjectIngredient;
    public bool IsCollision = false;
    
    public Action OnEnalbeAction;
    public Action OnDisableAction;
    public Action<Collider2D> OnTriggerEnter;
    public Action<Collider2D> OnTrigerStay;
    public Action<Collider2D> OnTriggerExit;

    public void SetOwner(IngredientMono _owner)
    {
        Owner = _owner;
    }

    public void SelectThisObj()
    {
        IsCollision = true;
        Owner.VisibleShadow();
        SpawnManager.Instance.SpawnParitcle(transform.position, ObjectIngredient);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsCollision)
            OnTriggerEnter?.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if(!IsCollision)
        //    OnTrigerStay?.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if(!IsCollision)
        //    OnTriggerExit?.Invoke(collision);
    }

    private void OnEnable()
    {
        OnEnalbeAction?.Invoke();
        IsCollision = false;
    }

    private void OnDisable()
    {
        OnDisableAction?.Invoke();
    }

    public void DestroyThisObj(float _time = default)
    {
        Destroy(this, _time);
    }
}
