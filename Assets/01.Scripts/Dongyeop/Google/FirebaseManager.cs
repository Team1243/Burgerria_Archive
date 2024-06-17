using System;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
                FirebaseAnalytics.LogEvent("Test Event");
        });
    }
}
