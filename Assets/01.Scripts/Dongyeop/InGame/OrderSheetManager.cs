using System.Collections;
using System.Collections.Generic;
using Define;
using NS.RomanLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class OrderSheetManager : MonoBehaviour
{
    public static OrderSheetManager Instance;

    [SerializeField] private List<OrderListSO> _orderListSosLevel1;
    [SerializeField] private List<OrderListSO> _orderListSosLevel2;
    [SerializeField] private List<OrderListSO> _orderListSosLevel3;
    public List<Sprite> IngredientImages;

    [SerializeField] private Transform _successParticle;

    private UIDocument _uiDocument;
    private VisualElement _root;
    private List<VisualElement> _ingredientImages = new List<VisualElement>();
    private List<VisualElement> _ingredientCheckImages = new List<VisualElement>();
    private VisualElement _burgerRender;
    private OrderSheet _orderSheet;
    private RadialFillElement _radialFillElement;

    [Header("Time UI")]
    [SerializeField] private GameObject _timeArea;
    [SerializeField] private GameObject _timeIcon;

    private OrderListSO _currentList;
    private int _cnt = 0;

    private bool _isDifference = false;

    private List<Coroutine> _objMovementCos = new List<Coroutine>();
    
    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple OrderSheetManager is running");
        Instance = this;
    }

    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        _root = _uiDocument.rootVisualElement;

        _orderSheet = new OrderSheet(_root.Q<VisualElement>("order-sheet-01"));
        _ingredientImages.Add(_orderSheet.BackImage.Q("item00")); 
        _ingredientCheckImages.Add(_ingredientImages[0].Q("icon")); 
        _ingredientImages.Add(_orderSheet.BackImage.Q("item01")); 
        _ingredientCheckImages.Add(_ingredientImages[1].Q("icon")); 
        _ingredientImages.Add(_orderSheet.BackImage.Q("item02")); 
        _ingredientCheckImages.Add(_ingredientImages[2].Q("icon")); 
        _ingredientImages.Add(_orderSheet.BackImage.Q("item03")); 
        _ingredientCheckImages.Add(_ingredientImages[3].Q("icon")); 
        _ingredientImages.Add(_orderSheet.BackImage.Q("item04"));
        _ingredientCheckImages.Add(_ingredientImages[4].Q("icon"));
        _radialFillElement = _root.Q<RadialFillElement>("radial-fill-element");
        _burgerRender = _root.Q<VisualElement>("burger-render");
    }

    public void BurgerCheck(List<Ingredient> ingredients, List<Transform> ingredientTrms)
    {
        if (ingredients.Count != _currentList.OrderList.Ingredients.Count)
        {
            foreach (var ingredient in ingredients)
            {
                if (ingredient == Ingredient.TRASH)
                {
                    InGameManager.Instance.GameOver();
                }
            }
            if (GameObject.Find("AudioPlayer").TryGetComponent<AudioPlayer>(out AudioPlayer _audio))
                _audio.SimplePlay("fail");

            BurgerSelectManager.Instance.BurgerEnd(true);
            ObjectReset(0);
            if (GameManager.Instance.CurrentScore > 1)
                TimeLabelSet(-3);
            return;
        }

        for (int i = 0; i < ingredients.Count; ++i)
        {
            if (ingredients[i] != _currentList.OrderList.Ingredients[i])
            {
                if (ingredients[i] == Ingredient.TRASH)
                {
                    InGameManager.Instance.GameOver();
                }
                if (GameObject.Find("AudioPlayer").TryGetComponent<AudioPlayer>(out AudioPlayer _audio))
                    _audio.SimplePlay("fail");

                BurgerSelectManager.Instance.BurgerEnd(true);
                ObjectReset(0);
                TimeLabelSet(-3);
                return;
            }
        }

        StartCoroutine(BurgerComplete(ingredientTrms));
    }

    private IEnumerator BurgerComplete(List<Transform> ingredientTrms)
    {
        _objMovementCos.Clear();
        for (int i = 0; i < ingredientTrms.Count; ++i)
        {
            ingredientTrms[i].GetComponent<SpriteRenderer>().sortingOrder = i+1;
            ingredientTrms[i].GetComponent<ColliderBridge>().Owner.InvisibleShadow();
            _objMovementCos.Add(StartCoroutine(ObjectMovement(ingredientTrms[i], ingredientTrms[i].position, new Vector3(0, -1 + (0.3f * i)),.25f)));
        }
        InGameManager.Instance.CycleType = GameCycleType.BURGETCOMPLETE;
        yield return new WaitForSeconds(.25f);
        _successParticle.gameObject.SetActive(true);
        _successParticle.GetComponent<ParticleSystem>().Play();

        InGameManager.Instance.BurgerMatch();
        yield return new WaitForSeconds(.1f);
        Taptic.Success();
        if(GameObject.Find("AudioPlayer").TryGetComponent<AudioPlayer>(out AudioPlayer _audio))
        {
            _audio.SimplePlay("success");
        }

        for (int i = 0; i < ingredientTrms.Count; ++i)
        {
            StopCoroutine(_objMovementCos[i]);
            StartCoroutine(ObjectMovement(ingredientTrms[i], new Vector3(0, -1 + (0.3f * i)), new Vector3(0, 12.5f),.5f));
        }
        yield return new WaitForSeconds(.25f);
        _burgerRender.AddToClassList("big");
        yield return new WaitForSeconds(.15f);
        ingredientTrms.Clear();
        yield return new WaitForSeconds(0.1f);
        BurgerSelectManager.Instance.BurgerEnd(true);
        ObjectReset();
        InGameManager.Instance.CycleType = GameCycleType.PLAY;
        yield return new WaitForSeconds(.1f);
        _burgerRender.RemoveFromClassList("big");
    }

    public void SetTimerSlider(float current, float max)
    {
        //_sliderValue.style.width = Length.Percent((current / max) * 100);
        _radialFillElement.value = current / max;
    }

    public void ObjectReset(int add = 1)
    {
        _cnt = 0;
        for (int i = 0; i < 5; ++i)
            _ingredientCheckImages[i].style.unityBackgroundImageTintColor = new StyleColor(new Color(253.0f / 255.0f, 61.0f / 255.0f, 61.0f / 255.0f, 0));
        
        _currentList = CurrentOrderListSo();
        _currentList.IngredientSetUp(_ingredientImages);
        
        SpawnManager.Instance.DeSpawnAll();
        GameManager.Instance.CurrentScore += add;

        _isDifference = false;
        foreach (var ingredient in _currentList.OrderList.Ingredients)
            SpawnIngredient(ingredient);
        SelectManager.Instance.Init();
    }

    public void AddObject(Ingredient ingredient)
    {
        if (ingredient == _currentList.OrderList.Ingredients[_cnt])
            _ingredientCheckImages[_cnt].style.unityBackgroundImageTintColor = new StyleColor(new Color(253.0f / 255.0f, 61.0f / 255.0f, 61.0f / 255.0f, 1));
        ++_cnt;
    }

    private IEnumerator ObjectMovement(Transform trm, Vector3 start, Vector3 end, float duration)
    {
        float current = 0;

        while (current < duration)
        {
            yield return null;
            current += Time.deltaTime;

            float t = current / duration;
            t = t * t * t;
            trm.position = Vector3.Lerp(start, end, t);
        }
        trm.position = end;
    }

    private void SpawnIngredient(Ingredient ingredient)
    {
        if (_isDifference || GameManager.Instance.CurrentScore <= 6)
        {
            SpawnManager.Instance.SpawnIngredient(ingredient, 1);
            return;
        }
        
        switch (ingredient)
        {
            case Ingredient.MEAT:
                SpawnManager.Instance.SpawnIngredient(Ingredient.RAWMEAT, 1);
                SpawnManager.Instance.SpawnIngredient(Ingredient.FRYPAN, 1);
                _isDifference = true;
                break;
            case Ingredient.FRIED_EGG:
                SpawnManager.Instance.SpawnIngredient(Ingredient.EGG, 1);
                SpawnManager.Instance.SpawnIngredient(Ingredient.FRYPAN, 1);
                _isDifference = true;
                break;
            case Ingredient.SLICED_TOMATO:
                SpawnManager.Instance.SpawnIngredient(Ingredient.TOMATO, 1);
                SpawnManager.Instance.SpawnIngredient(Ingredient.KNIFE, 1);
                _isDifference = true;
                break;
            case Ingredient.SLICED_LETTUCE:
                SpawnManager.Instance.SpawnIngredient(Ingredient.LETTUCE, 1);
                SpawnManager.Instance.SpawnIngredient(Ingredient.KNIFE, 1);
                _isDifference = true;
                break;
            default:
                SpawnManager.Instance.SpawnIngredient(ingredient, 1);
                break;
        }
    }

    private OrderListSO CurrentOrderListSo()
    {
        if (GameManager.Instance.CurrentScore <= 4)
            return _orderListSosLevel1[Random.Range(0, _orderListSosLevel1.Count)];
        if (GameManager.Instance.CurrentScore <= 8)
            return _orderListSosLevel2[Random.Range(0, _orderListSosLevel2.Count)];
        return _orderListSosLevel3[Random.Range(0, _orderListSosLevel3.Count)];
    }

    public void TimeLabelSet(float value)
    {
        StartCoroutine(TimeLabelSetCo(value));
    }

    private IEnumerator TimeLabelSetCo(float value)
    {
        List<GameObject> times = new List<GameObject>();
        
        for (int i = 0; i < Mathf.Abs(value) / 0.5f; ++i)
        {
            GameObject obj = Instantiate(_timeIcon, _timeArea.transform, true);
            Vector2 pos = new Vector2(Random.Range(-175.0f, 175.0f), Random.Range(-175.0f, 175.0f));
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.localPosition = new Vector3(0, 0, 0);
            rect.anchoredPosition = pos;
            obj.GetComponent<Image>().color = value < 0 ? Color.blue : Color.red;
            times.Add(obj);
            StartCoroutine(UIScaleCo(obj, Vector3.zero, Vector3.one, 0.2f));
        }
        yield return new WaitForSeconds(0.2f);
        
        foreach (var time in times)
            StartCoroutine(UIPositionCo(time, new Vector2((Screen.width / 2 * -1) + 100, (Screen.height / 2f) - 100f), 0.5f));
        
        yield return new WaitForSeconds(0.5f);
        
        foreach (var time in times)
            StartCoroutine(UIScaleCo(time, Vector3.one, Vector3.zero, 0.2f));
        InGameManager.Instance.CurrentTime += value;
    }

    private IEnumerator UIScaleCo(GameObject obj, Vector3 start, Vector3 end, float duration)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        float current = 0;

        while (current < duration)
        {
            yield return null;
            current += Time.deltaTime;
            float t = current / duration;
            t *= t * t;
            rect.localScale = Vector3.Lerp(start, end, t);
        }
    }

    private IEnumerator UIPositionCo(GameObject obj, Vector2 end, float duration)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        Vector2 start = rect.anchoredPosition;
        float current = 0;

        while (current < duration)
        {
            yield return null;
            current += Time.deltaTime;
            float t = current / duration;
            t *= t * t;
            rect.anchoredPosition = Vector3.Lerp(start, end, t);
        }
    }
}
