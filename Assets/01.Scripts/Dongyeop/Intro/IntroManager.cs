using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private List<OrderListSO> _orderLists = new List<OrderListSO>();
    [SerializeField] private List<Sprite> _ingredientSprite = new List<Sprite>();
    private List<IntroMovement> _burgerObj = new List<IntroMovement>();

    private void Awake()
    {
        for (int i = 0; i < 5; ++i)
            _burgerObj.Add(transform.GetChild(i).GetComponent<IntroMovement>());
    }

    private void Start()
    {
        OrderListSO orderList = _orderLists[Random.Range(0, _orderLists.Count)];
        for (int i = 0; i < 5; ++i)
            _burgerObj[i].SpriteRenderer.sprite = _ingredientSprite[(int)orderList.OrderList.Ingredients[i]];
    }
}
