using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu (menuName = "SO/Order/OrderList", fileName = "OrderList")]
public class OrderListSO : ScriptableObject
{
    public OrderList OrderList;

    public void IngredientSetUp(List<VisualElement> _visualElements)
    {
        
        if (_visualElements.Count != 5)
        {
            Debug.LogError("Ingredient VisualElement Count not 5");
            return;
        }
        
        for (int i = 0; i < 5; ++i)
        {
            _visualElements[i].style.backgroundImage = new StyleBackground(OrderSheetManager.Instance.IngredientImages[(int)OrderList.Ingredients[i]]);
        }
    }
}
