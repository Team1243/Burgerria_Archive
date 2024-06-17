using UnityEngine;

public class BurgerTray : PutObjectMono
{
    [HideInInspector] public bool IsShow = false;

    [Header("Tray")]
    [SerializeField] private float _maxTime;
    [SerializeField] private Vector3 _trayShow;
    [SerializeField] private Vector3 _trayUnShow;
    private float _currentTime = 0;

    private void Update()
    {
        TrayPositionChange();
    }

    public override void OnPut(PickObjectMono pickObject, Transform currentObject)
    {
        if (pickObject && pickObject.IsBurger)
        {
            currentObject.position = transform.position - new Vector3(0, .5f, 0);
            currentObject.parent = transform;
            Destroy(pickObject);
        }
        else if (currentObject)
            Destroy(currentObject);
    }

    private void TrayPositionChange()
    {
        _currentTime += IsShow ? Time.deltaTime : -Time.deltaTime;
        _currentTime = Mathf.Clamp(_currentTime, 0, _maxTime);
        float time = _currentTime == 0 ? 0 : _currentTime / _maxTime;
        transform.position = Vector3.Lerp(_trayShow, _trayUnShow, time);
    }
}
