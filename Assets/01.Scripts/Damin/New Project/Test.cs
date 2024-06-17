using Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Spawn
{
    [SerializeField] private Ingredient Type;
    public Ingredient _type => Type;

    [SerializeField] private int Amount;
    public int _amount => Amount;
}

public class Test : MonoBehaviour
{
    [SerializeField] private List<Spawn> _spawns = new List<Spawn>();

    [ContextMenu("SpawnTest")]
    public void SpawnTest()
    {
        for(int i = 0; i < _spawns.Count; i++) 
        {
            for(int j = 0; j < _spawns[i]._amount; j++)
            {
                SpawnManager.Instance.SpawnIngredient(_spawns[i]._type, 1);
            }
        }
    }

}
