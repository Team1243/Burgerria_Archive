using UnityEngine;
using UnityEngine.Serialization;

public class IngredientObject : PickObjectMono
{
    [SerializeField] private GameObject _burgerIngredient; 
    
    public override Transform OnPick()
    {
        Transform burger = Instantiate(_burgerIngredient).transform;
        burger.GetComponent<SpriteRenderer>().sortingOrder = ProduceManager.Instance.OrderInLayerCnt++;
        return burger;
    }
}
