using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SelectState { None, Start, Ing, End }

public class SelectManager : MonoBehaviour
{
    public static SelectManager Instance;

    [SerializeField] private LayerMask _whatIsIngredient;

    private List<Ingredient> _ingredients = new List<Ingredient>();

    private ColliderBridge m_prevTransform = null;
    private List<Transform> _ingredientTrms = new List<Transform>();

    private Ingredient m_prevType = default;
    public SelectState State = SelectState.End;

    private Vector2 m_cursorPos;

    private TrailRenderer m_trailRenderer;
    private LineRenderer m_lineRenderer;

    private AudioPlayer _audioPlayer;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this); 
    }

    private void Start()
    {
        m_trailRenderer = GetComponentInChildren<TrailRenderer>();
        m_lineRenderer = GetComponent<LineRenderer>();
        _audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Update()
    {
        InputAction();
        DrawTrail();
    }

    public void Init()
    {
        State = SelectState.None;
    }

    private void InputAction()
    {
        if (State == SelectState.End)
            return;
        
        if (Application.platform == RuntimePlatform.Android)
        {
            if(Input.touchCount > 0)
            {
                Touch _touch = Input.GetTouch(0);

                if(_touch.phase == TouchPhase.Began && State == SelectState.None && !EventSystem.current.IsPointerOverGameObject())
                {
                    State = SelectState.Start;
                }


                if(_touch.phase == TouchPhase.Ended)
                {
                    ClearSelectList();
                }
                else if(State == SelectState.Start || State == SelectState.Ing)
                {
                    State = SelectState.Ing;
                    m_cursorPos = Camera.main.ScreenToWorldPoint(_touch.position);

                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward, 1, _whatIsIngredient);
                    if (hit)
                    {
                        if (hit.transform.TryGetComponent(out ColliderBridge collision) && !collision.IsCollision)
                        {
                            SelectObj(collision);
                        }
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && State == SelectState.None && !EventSystem.current.IsPointerOverGameObject())
            {
                State = SelectState.Start;
                m_trailRenderer.Clear();
            }

            if (Input.GetMouseButton(0) && (State == SelectState.Start || State == SelectState.Ing))
            {
                State = SelectState.Ing;
                m_cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward, 1, _whatIsIngredient);
                if (hit)
                {
                    if (hit.transform.TryGetComponent(out ColliderBridge collision) && !collision.IsCollision)
                    {
                        SelectObj(collision);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && State == SelectState.Ing) 
            {
                ClearSelectList();
            }
        }
    }

    private void SelectObj(ColliderBridge collision)
    {
        SelectEffect();
        collision.SelectThisObj();
        DrawLine(collision.transform.position);

        Ingredient _current = collision.ObjectIngredient;

        if (m_prevTransform != null && m_prevType != _current && (int)_current >= 100 && ((int)m_prevType % 10 == 0 || (int)_current % 10 == 0))
        {
            //&& Enum.IsDefined(typeof(Ingredient), MathF.Abs((int)m_prevType - (int)_current))
            
            if(m_lineRenderer.positionCount > 0) 
                m_lineRenderer.positionCount -= 2;
            else
                m_lineRenderer.positionCount = 0;   
            SelectEffect();
            collision.SelectThisObj();

            Vector2[] newList = new Vector2[1] { collision.Owner.GetLocalPos() };
            SpawnManager.Instance.SpawnIngredient((Ingredient)MathF.Abs((int)m_prevType - (int)_current), 1, newList);

            SpawnManager.Instance.DeSpawn(m_prevTransform.Owner);
            SpawnManager.Instance.DeSpawn(collision.Owner);


            m_prevTransform = null;
            m_prevType = default;
            //OrderSheetManager.Instance.AddObject(collision.ObjectIngredient);
            //BurgerSelectManager.Instance.AddBurger(collision.ObjectIngredient);

            return;
        }
        else if ((int)_current >= 100)
        {
            m_prevTransform = collision;
            m_prevType = collision.ObjectIngredient;

            return;
        }
        else
        {
            m_prevTransform = null;
            m_prevType = default;
        }

        
        _ingredients.Add(collision.ObjectIngredient);
        _ingredientTrms.Add(collision.transform);


        OrderSheetManager.Instance.AddObject(collision.ObjectIngredient);
        BurgerSelectManager.Instance.AddBurger(collision.ObjectIngredient);

        DrawLine(collision.transform.position);
    }

    private void SelectEffect()
    {
        _audioPlayer.PlayerClipWithVariablePitch("pop", 1 + (_ingredients.Count * 0.4f));

        if (Application.platform == RuntimePlatform.Android)
            Taptic.Selection();
    }

    public void ClearSelectList()
    {
        OrderSheetManager.Instance.BurgerCheck(_ingredients, _ingredientTrms);

        State = SelectState.None;
        m_lineRenderer.positionCount = 0;

        _ingredients.Clear();
    }

    private void DrawLine(Vector2 _pos)
    {
        m_lineRenderer.positionCount++;
        m_lineRenderer.SetPosition(m_lineRenderer.positionCount - 1, _pos);
    }

    private void DrawTrail()
    {
        //if (m_state != SelectState.Ing)
        //    return;

        m_trailRenderer.gameObject.transform.position = m_cursorPos;
    }
}
