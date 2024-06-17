using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class BurgerSelectManager : MonoBehaviour
{
    public static BurgerSelectManager Instance;
    
    [SerializeField] private List<GameObject> _burgerImages;

    private GameObject _currentBurger;
    private int _burgerCnt = 0;
        
    const float n1 = 7.5625f;
    const float d1 = 2.75f;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple BurgerSeleteManager is running");
        Instance = this; 
        NewBurger();
    }

    public void NewBurger()
    {
        GameObject burger = new GameObject("NewBurger");
        burger.transform.parent = transform;
        burger.transform.localPosition = Vector3.down;
        _currentBurger = burger;
    }

    public void AddBurger(Ingredient ingredient)
    {
        Transform trm = Instantiate(_burgerImages[(int)ingredient], _currentBurger.transform).transform;
        trm.GetComponent<SpriteRenderer>().sortingOrder = _burgerCnt;
        trm.localPosition = new Vector3(0, 5 + (_burgerCnt * 0.5f));
        StartCoroutine(MovementCo(trm, trm.localPosition, new Vector3(0, _burgerCnt * 0.5f), .15f));
        ++_burgerCnt;
    }

    public GameObject BurgerEnd(bool isDestroy)
    {
        _burgerCnt = 0;
        GameObject deleteBurger = _currentBurger;
        if (isDestroy)
            StartCoroutine(MovementCo(deleteBurger.transform, deleteBurger.transform.localPosition, new Vector3(0, 7, 0), 1, () => Destroy(deleteBurger.gameObject)));
        NewBurger();
        return deleteBurger;
    }

    private IEnumerator MovementCo(Transform moveTrm, Vector3 start, Vector3 end, float time, Action endAction = null)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            yield return null;
            currentTime += Time.deltaTime;
            float t = currentTime / time;
            
            if (t < 1 / d1) 
                t = n1 * t * t;
            else if (t < 2 / d1) 
                t = n1 * (t -= 1.5f / d1) * t + 0.75f;
            else if (t < 2.5f / d1) 
                t = n1 * (t -= 2.25f / d1) * t + 0.9375f;
            else 
                t = n1 * (t -= 2.625f / d1) * t + 0.984375f;

            moveTrm.localPosition = Vector3.Lerp(start, end, t);
        }
        
        moveTrm.localPosition = Vector3.Lerp(start, end, 1);
        endAction?.Invoke();
    }
}
