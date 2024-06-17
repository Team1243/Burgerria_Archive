using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackManager : MonoBehaviour
{
    public static FeedBackManager Instance;

    [SerializeField] private SerializableDict<string, FeedBack> _feedBacks = new SerializableDict<string, FeedBack>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void PlayFeedBack(string _feedback)
    {
        _feedBacks.GetValue(_feedback).CreateFeedBack();
    }

    public void FinishFeedback(string _feedback)
    {
        _feedBacks.GetValue(_feedback).CompleteFeedBack();
    }
}
