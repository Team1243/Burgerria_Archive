using Define;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct BugerSprite
{
    [SerializeField] private Sprite Sprite;
    public Sprite _sprite => Sprite;

    //[SerializeField] private Sprite WhiteSprite;
    //public Sprite _whiteSprite => WhiteSprite;
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;


    [Header("Status")]

    [SerializeField] private Vector2 m_ingredientSize;

    [Tooltip("첫 스폰 갯수")]
    [SerializeField] private int m_spawnCount = 4;

    [Tooltip("재료 사이의 최소 거리")][Range(0.5f, 3f)]
    [SerializeField] private float m_minDistance;
    [SerializeField] private Material m_whiteMat;
    [SerializeField] private SerializableDict<Ingredient, BugerSprite> m_sprites = new SerializableDict<Ingredient, BugerSprite>();


    [Space]


    //[Header("SpawnRange")]
    //[Header("Padding")]
    //[SerializeField] private bool m_showGizmo = false;
    //[SerializeField] private float m_leftPadding;
    //[SerializeField] private float m_rightPadding;
    //[SerializeField] private float m_topPadding;
    //[SerializeField] private float m_bottomPadding;

    [Space]

    [SerializeField] private ParticleSystem m_particleSystem;
    [SerializeField] private int m_particleCount;
    private Stack<ParticleSystem> m_particles = new Stack<ParticleSystem>();

    private Dictionary<Ingredient, Stack<IngredientMono>> m_ingredients = new Dictionary<Ingredient, Stack<IngredientMono>>();
    private List<IngredientMono> m_spawnedList = new List<IngredientMono>();

    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        
        //기본 스폰
        foreach (Ingredient type in Enum.GetValues(typeof(Ingredient)))
        {
            m_ingredients.Add(type, new Stack<IngredientMono>());
            //m_spawnedList.Add(type, new List<IngredientMono>());

            //Debug.Log(type.ToString());
            MakeNewIngredient(type, m_spawnCount);
        }

        MakeParticle(m_particleCount);
    }

    /// <summary>
    /// _transforms에는 스폰위치, 로테이션, 크기 등을 정의 함,  _count랑 크기가 같아야함, null일시 랜덤
    /// </summary>
    /// <param name="_ingredientType"></param>
    /// <param name="_amount"></param>
    /// <param name="_transforms"></param>
    public void SpawnIngredient(Ingredient _ingredientType, int _amount = 1, Vector2[] _transforms = null)
    {
        if (m_ingredients[_ingredientType].Count <= 0 || _amount > m_ingredients[_ingredientType].Count)
        {
            Debug.LogWarning($"{_amount} - {m_ingredients[_ingredientType].Count} = {_amount - m_ingredients[_ingredientType].Count}");
            MakeNewIngredient(_ingredientType, _amount - m_ingredients[_ingredientType].Count);
        }


        for (int i = 0; i < _amount; i++)
        {
            //if (m_ingredients[_ingredientType].Count <= 0)
            //    MakeNewIngredient(_ingredientType);

            var spawn_ingredient = m_ingredients[_ingredientType].Pop();
            spawn_ingredient.SetEnable();


            //m_spawnedList[_ingredientType].Add(spawn_ingredient);
            m_spawnedList.Add(spawn_ingredient);

            //spawn_ingredient.SetPos(_transforms?[i] ?? GetRandomSpawnPos());
            if (_transforms != default)
                spawn_ingredient.SetPos(_transforms[i]);
        }
        //GC.Collect();
    }

    public void SpawnIngredient(Ingredient _ingredientType, int _amount = 1)
    {
        if (m_ingredients[_ingredientType].Count <= 0 || _amount > m_ingredients[_ingredientType].Count)
        {
            Debug.LogWarning($"{_amount} - {m_ingredients[_ingredientType].Count} = {_amount - m_ingredients[_ingredientType].Count}");
            MakeNewIngredient(_ingredientType, _amount - m_ingredients[_ingredientType].Count);
        }


        for (int i = 0; i < _amount; i++)
        {
            //if (m_ingredients[_ingredientType].Count <= 0)
            //    MakeNewIngredient(_ingredientType);

            var spawn_ingredient = m_ingredients[_ingredientType].Pop();
            spawn_ingredient.SetEnable();


            //m_spawnedList[_ingredientType].Add(spawn_ingredient);
            m_spawnedList.Add(spawn_ingredient);


            //spawn_ingredient.SetPos(_transforms?[i] ?? GetRandomSpawnPos());
            spawn_ingredient.SetPos(GetRandomSpawnPos());
        }
    }

    private void MakeNewIngredient(Ingredient _ingredientType, int _count = 1)
    {
        for (int i = 0; i < _count; i++)
        {
            IngredientMono _newIngredient = new IngredientMono(_ingredientType, m_sprites.GetValue(_ingredientType)._sprite, m_whiteMat, _ingredientType.ToString());;
            _newIngredient.SetScale(m_ingredientSize);
            _newIngredient.SetParent(transform);
            _newIngredient.SetDisable();

            if (!m_ingredients.ContainsKey(_ingredientType))
                m_ingredients.Add(_ingredientType, new Stack<IngredientMono>());

            m_ingredients[_ingredientType].Push(_newIngredient);
            //var value = Ingredients.ContainsKey(_ingredientType) ? Ingredients[_ingredientType] : new List<IngredientMono<IngredientType>>();
            //value.Add(_newIngredient);
            //Ingredients[_ingredientType] = value;
        }
    }

    private void MakeParticle(int _count)
    {
        for(int i = 0; i < _count; i++)
        {
            var _newParticle = Instantiate(m_particleSystem, transform.position, Quaternion.identity);
            _newParticle.gameObject.transform.SetParent(transform);
            m_particles.Push(_newParticle);

            _newParticle.gameObject.SetActive(false);
        }
    }

    public void SpawnParitcle(Vector3 _pos, Ingredient _ingredientType)
    {
        if (m_particles.Count <= 0)
            MakeParticle(1);

        ParticleSystem _newParticle = m_particles.Pop();
        Color _color = Color.white;

        _newParticle.gameObject.SetActive(true);
        _newParticle.gameObject.transform.position = _pos;
        switch (_ingredientType)
        {
            case Ingredient.FRIED_EGG:
                _color = Color.white;
                break;
            case Ingredient.UPBREAD:
            case Ingredient.DOWNBREAD:
                _color = new Color(0.9725491f, 0.5921569f, 0.2156863f, 1f);
                break;
            case Ingredient.MEAT:
                _color = new Color(0.764706f, 0.3411765f, 0.2235294f, 1f);
                break;
            case Ingredient.SLICED_LETTUCE:
                _color = Color.green;
                break;
            case Ingredient.CHEESE:
                _color = Color.yellow;
                break;
            case Ingredient.BACON:
                _color = new Color(0.8000001f, 0.3333333f, 0.3176471f, 1f);
                break;
            case Ingredient.SLICED_TOMATO:
                _color = Color.red;
                break;
            case Ingredient.FRYPAN:
                _color = Color.black;
                break;
            case Ingredient.PICKLE:
                _color = Color.green;
                break;
            case Ingredient.ONION:
                _color = new Color(0.6078432f, 0.2745098f, 0.4784314f, 1f);
                break;
            case Ingredient.MUSHROOM:
                _color = new Color(0.6509804f, 0.5372549f, 0.4196079f, 1f);
                break;
            case Ingredient.HAM:
                _color = new Color(0.7764707f, 0.3254902f, 0.2078432f, 1f);
                break;
            case Ingredient.RAWMEAT:
                _color = new Color(0.7568628f, 0.4431373f, 0.4745098f, 1f);
                break;
            case Ingredient.PAPRIKA:
                _color = Color.red;
                break;
            case Ingredient.EGG:
                _color = new Color(0.764706f, 0.4901961f, 0.2941177f, 1f);
                break;
            case Ingredient.TRASH:
                _color = new Color(0.627451f, 0.627451f, 0.627451f, 1f);
                break;
            case Ingredient.KNIFE:
                _color = new Color(0.627451f, 0.627451f, 0.627451f, 1f);
                break;
            case Ingredient.TOMATO:
                _color = Color.red;
                break;
            case Ingredient.LETTUCE:
                _color = Color.green;
                break;
        }

        _newParticle.startColor = _color;
        _newParticle.Play();
    }

    public void DeSpawnParticle(ParticleSystem _particle)
    {
        m_particles.Push( _particle );
        _particle.gameObject.SetActive(false);
    }

    //public void DeSpawn(Transform _transform)
    //{
    //    var _despawn = m_spawnedList.Select(s => s.Value.Where(w => w.GetTransform() == _transform)) as IngredientMono;

    //    if (_despawn == null)
    //    {
    //        Debug.LogWarning($"{_transform} does not exit");
    //        return;
    //    }

    //    m_spawnedList[_despawn.IngredType].Remove(_despawn);
        

    //    if (m_ingredients[_despawn.IngredType].Count >= m_spawnCount)
    //    {
    //        _despawn.Destroy();
    //        _despawn = null;
    //        return;
    //    }

    //    _despawn.SetPos(Vector2.zero);
    //    m_ingredients[_despawn.IngredType].Push(_despawn);
    //    _despawn.SetDisable();
    //}

    public void DeSpawn(IngredientMono _ingredient)
    {
        //IngredientMono _despawn = m_spawnedList.Select(s => s.Value.Where(w => w == _ingredient)) as IngredientMono;
        //var _despawn = m_spawnedList[_ingredient.IngredType].Single(item => item == _ingredient);
        var _despawn = m_spawnedList.Single(item => item == _ingredient);

        if (_despawn == null)
        {
            Debug.LogWarning($"{_ingredient} does not exit");
            return;
        }

        m_spawnedList.Remove(_despawn);

        if (m_ingredients[_despawn.IngredType].Count >= m_spawnCount)
        {
            _despawn.Destroy();
            //_despawn.Dispose();
            return;
        }

        _despawn.SetPos(Vector2.zero);
        m_ingredients[_despawn.IngredType].Push(_despawn);
        _despawn.SetDisable();

        //GC.Collect();
    }

    [ContextMenu("DeSpawnAll")]
    public void DeSpawnAll()
    {
        //foreach (Ingredient _item in Enum.GetValues(typeof(Ingredient)))
        //{
        //    if (!m_spawnedList.ContainsKey(_item) || m_spawnedList[_item].Count <= 0)
        //        continue;

        //    while (m_spawnedList[_item].Count > 0)
        //    {
        //        var _deItem = m_spawnedList[_item][0];
        //        m_spawnedList[_item].Remove(m_spawnedList[_item][0]);

        //        if (m_ingredients[_item].Count >= m_spawnCount)
        //        {
        //            //_deItem.Dispose();
        //            _deItem.Destroy();
        //            continue;
        //        }

        //        _deItem.SetPos(Vector2.zero);
        //        m_ingredients[_deItem.IngredType].Push(_deItem);
        //        _deItem.SetDisable();
        //    }

        //    m_spawnedList[_item].Clear();

        //}
        for(int i = 0;  i < m_spawnedList.Count; i++)
        {
            var _deItem = m_spawnedList[i];
            //m_spawnedList.Remove(_deItem);

            if (m_ingredients[_deItem.IngredType].Count >= m_spawnCount)
            {
                //_deItem.Dispose();
                _deItem.Destroy();
                //continue;
            }
            else
            {
                _deItem.SetPos(Vector2.zero);
                m_ingredients[_deItem.IngredType].Push(_deItem);
                _deItem.SetDisable();
            }

        }

        m_spawnedList.Clear();
    }

    private bool CanSetPos(Vector2 _vec)
    {
        for(int i = 0; i < m_spawnedList.Count; i++)
        {
            if (Vector2.Distance(_vec, m_spawnedList[i].GetPos()) < m_minDistance)
                return false;
        }
        return true;
    }

    private bool CanSetPos(Vector2 _vec, float _distance)
    {
        for (int i = 0; i < m_spawnedList.Count; i++)
        {
            if (Vector2.Distance(_vec, m_spawnedList[i].GetPos()) < _distance)
                return false;
        }
        return true;
    }

    public Vector2 GetRandomSpawnPos(float _minDist = default)
    {
        if (_minDist == default)
            _minDist = m_minDistance;

        Vector2 LeftTop = ScreenManager.Instance.LeftTop();
        Vector2 RightBottom = ScreenManager.Instance.RightBottom();

        LeftTop.x += ScreenManager.Instance.LeftPadding;
        LeftTop.y -= ScreenManager.Instance.TopPadding;

        RightBottom.x -= ScreenManager.Instance.RightPadding;
        RightBottom.y += ScreenManager.Instance.BottomPadding;

        Vector2 _newPos = new Vector2(Random.Range(LeftTop.x, RightBottom.x), Random.Range(RightBottom.y, LeftTop.y));

        int i = 0; 
        while (i++ < 7)
        {
            if (CanSetPos(_newPos, _minDist))
                break;
            _newPos = new Vector2(Random.Range(LeftTop.x, RightBottom.x), Random.Range(RightBottom.y, LeftTop.y));
        }

        if (!CanSetPos(_newPos, _minDist -0.15f))
            return GetRandomSpawnPos();

        return _newPos;
    }
}
