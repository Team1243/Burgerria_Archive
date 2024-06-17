using System;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class Burger : MonoBehaviour
{
    [SerializeField] private int _burgerLimit;
    
    private PutObjectMono _currentPutObject;
    public Dictionary<Define.Ingredient, int> BurgerDic = new Dictionary<Define.Ingredient, int>();
    public Deque<Define.Ingredient> BurgerDeque = new Deque<Define.Ingredient>();

    private Vector3 _ingredientPos = new Vector3(0, .2f, 0);

    private void OnEnable()
    {
        BurgerDic.Add(Define.Ingredient.UPBREAD, 0);
        BurgerDic.Add(Define.Ingredient.DOWNBREAD, 0);
        BurgerDic.Add(Define.Ingredient.MEAT, 0);
        BurgerDic.Add(Define.Ingredient.CHEESE, 0);
        BurgerDic.Add(Define.Ingredient.FRIED_EGG, 0);
        BurgerDic.Add(Define.Ingredient.BACON, 0);
        BurgerDic.Add(Define.Ingredient.SLICED_LETTUCE, 0);
        BurgerDic.Add(Define.Ingredient.SLICED_TOMATO, 0);
    }

    public void AddBurgerDic(Define.Ingredient ingredient, Transform obj)
    {
        if (BurgerDeque.Count() > _burgerLimit)
        {
            Destroy(obj.gameObject);
            return;
        }
        
        BurgerDic[ingredient] = BurgerDic[ingredient] + 1;
        BurgerDeque.Push(ingredient);
        obj.parent = transform;
        obj.localPosition = _ingredientPos;
        _ingredientPos += new Vector3(0, 0.2f, 0);
    }
}
