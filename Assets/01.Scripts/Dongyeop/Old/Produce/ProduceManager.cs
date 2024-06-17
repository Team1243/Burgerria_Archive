using UnityEngine;

public class ProduceManager : MonoBehaviour
{
    public static ProduceManager Instance;

    [SerializeField] private LayerMask _whatIsPickObject;
    [SerializeField] private LayerMask _whatIsPutObject;

    [Header("Pick Object")] 
    private Transform _currentObject;
    private PickObjectMono _pickObject = null;
    
    [Header("Burger")]
    public int OrderInLayerCnt = 10;
    [SerializeField] private BurgerTray _burgerTray;
    
    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple ProduceManager is running");
        Instance = this;
    }

    private void Update()
    {
        GrabCheck();
    } 

    private void GrabCheck()
    {
        if (Input.touchCount >= 1)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                if (_currentObject)
                    Destroy(_currentObject.gameObject);
                
                Vector3 origin = Camera.main.ScreenToWorldPoint(touch.position);
                
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 100, _whatIsPickObject);
                if (hit)
                {
                    PickObjectMono pick = hit.transform.GetComponent<PickObjectMono>();
                    _currentObject = pick.OnPick();
                    _currentObject.position = Camera.main.ScreenToWorldPoint(touch.position);
                    _currentObject.position = new Vector3(_currentObject.position.x, _currentObject.position.y, 0);
                    _pickObject = pick;
                }
                BurgerTrayPosition(origin);
            }
            if (touch.phase == TouchPhase.Moved)
            {
                if (_currentObject)
                {
                    Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
                    _currentObject.position = new Vector3(pos.x, pos.y);
                    BurgerTrayPosition(pos);
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                Vector3 origin = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 100, _whatIsPutObject);
                
                if (hit && hit.transform.TryGetComponent(out PutObjectMono putObj))
                    putObj.OnPut(_pickObject, _currentObject);
                else if (_currentObject)
                    Destroy(_currentObject.gameObject);

                _pickObject = null;
                _currentObject = null;
                _burgerTray.IsShow = false;
            }
        }
    }

    private void BurgerTrayPosition(Vector3 pos)
    {
        if (_pickObject.IsBurger)
        {
            if (pos.y > 2 && Mathf.Abs(pos.x) < 3)
                _burgerTray.IsShow = true;
            else 
                _burgerTray.IsShow = false;
        }
        else
            _burgerTray.IsShow = false;
    }
}
