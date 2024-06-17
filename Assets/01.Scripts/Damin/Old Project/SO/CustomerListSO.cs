using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CustomerList", menuName = "SO/Customer/CustomerList")]
public class CustomerListSO : ScriptableObject
{
    [SerializeField] private List<CustomerInfoSO> CustomerList;
    public List<CustomerInfoSO> _customerList => CustomerList;

    public CustomerInfoSO GetRandomCustomer(CustomerInfoSO _curCustomer = default)
    {
        while (true)
        {
            int index = Random.Range(0, CustomerList.Count);

            if(_customerList[index] != _curCustomer)
                return _customerList[index];
        }
    }

    public CustomerInfoSO GetCustomer(int index)
    {
        return CustomerList[index];
    }
}
