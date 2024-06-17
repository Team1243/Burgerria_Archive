using UnityEngine;
using UnityEngine.Serialization;

public class Main : PutObjectMono
{
    [SerializeField] private GameObject _burger;

    public override void OnPut(PickObjectMono pickObject, Transform currentObject)
    {
        if (currentObject.TryGetComponent<Burger>(out Burger burger))
        {
            burger.transform.position = transform.position + new Vector3(0, -2, 0);
            currentBurger = burger;
        }
        else if (!currentBurger)
        {
            currentBurger = Instantiate(_burger).GetComponent<Burger>();
            currentBurger.transform.position = transform.position + new Vector3(0, -2, 0);
        }
        if (pickObject && !pickObject.IsBurger)
            currentBurger.AddBurgerDic(pickObject.Ingredient, currentObject);
    }
}
