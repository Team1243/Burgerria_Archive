using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class IngredientMono
{
    public Ingredient IngredType { private set; get; }


    private GameObject m_obj = null;
    private GameObject m_shadowObj = null;

    public bool IsSelected =false;

    public IngredientMono(Ingredient _ingredType, Sprite _sprite, Material _whiteMaterial, string _objName = default)
    {
        this.IngredType = _ingredType;

        m_obj = new GameObject(string.IsNullOrEmpty(_objName) ? this.IngredType.GetType().Name : _objName);
        m_obj.AddComponent<SpriteRenderer>().sprite = _sprite;
        m_obj.GetComponent<SpriteRenderer>().sortingOrder = 10;
        m_obj.AddComponent<PolygonCollider2D>().isTrigger = true;
        m_obj.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        
        m_obj.gameObject.layer = LayerMask.NameToLayer("Ingredient");


        m_obj.AddComponent<ColliderBridge>().SetOwner(this);
        var col = m_obj.GetComponent<ColliderBridge>();
        col.OnTriggerEnter += OnTriggerEnter;
        //col.OnTrigerStay += OnTriggerStay;
        //col.OnTriggerExit += OnTriggerExit;
        col.ObjectIngredient = _ingredType;

        m_shadowObj = new GameObject();
        m_shadowObj.transform.SetParent(m_obj.transform);
        m_shadowObj.transform.localScale = new Vector2(1.2f, 1.2f);
        m_shadowObj.transform.position = Vector2.zero;
        m_shadowObj.AddComponent<SpriteRenderer>();

        if(m_shadowObj.TryGetComponent<SpriteRenderer>(out SpriteRenderer _s))
        {
            _s.sprite = _sprite;
            _s.material = _whiteMaterial;
            _s.color = Color.gray;
            _s.gameObject.SetActive(false);
        }
    }

    ~IngredientMono()
    {
        Destroy();
        Debug.Log("소멸자 호출됨");
    }


    private void Init()
    {
        InvisibleShadow();
        m_obj.GetComponent<ColliderBridge>().IsCollision = false;
    }


#region 오브젝트 설정

    #region Get

    public Transform GetTransform() => m_obj.transform;

    public Vector2 GetPos() => m_obj.transform.position;

    public Vector2 GetLocalPos() => m_obj.transform.localPosition;

    public Vector2 GetScale() => m_obj.transform.localScale;

    #endregion


    #region Set

    public void SetPos(Vector2 _vec) => m_obj.transform.position = _vec;

    public void SetRoate(Vector2 _vec) => m_obj.transform.eulerAngles = _vec;

    public void SetScale(Vector2 _vec) => m_obj.transform.localScale = _vec;

    public void SetEnable()
    {
        m_obj.gameObject.SetActive(true);
        Init();
    }

    public void SetDisable() => m_obj.gameObject.SetActive(false);

    public void SetParent(Transform _transform) => m_obj.transform.SetParent(_transform); 
    
    public void Destroy(float _time = 0) => m_obj.GetComponent<ColliderBridge>().DestroyThisObj(_time);

    public void VisibleShadow()
    {
        m_obj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.55f);
        //m_shadowObj.SetActive(true);
    }

    public void InvisibleShadow()
    {
        m_obj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        //m_shadowObj.SetActive(false);
    }

    #endregion


    #endregion


    #region 충돌

    private void OnTriggerEnter(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ingredient"))
        {
            SetPos(SpawnManager.Instance.GetRandomSpawnPos());
        }

    }

    private void OnTriggerStay(Collider2D collision) 
    {

    }

    private void OnTriggerExit(Collider2D collision)
    {

    }

    #endregion


    //public static IngredientMono operator +(IngredientMono _I1, IngredientMono _I2)
    //{
    //    if((int)_I1.IngredType + (int)_I2.IngredType < 200 || !Enum.IsDefined(typeof(Ingredient), MathF.Abs((int)_I1.IngredType - (int)_I2.IngredType)))
    //    {
    //        return null;
    //    }


    //    // 선택 매니저에서 따로 구분을 해야함 조합 재료들은
    //}
}
