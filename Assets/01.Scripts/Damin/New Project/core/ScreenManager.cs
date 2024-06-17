using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance;

    public Vector3 ScreenSize { get; private set; }

    [Header("SpawnRange")]
    [Header("Padding")]
    [SerializeField] private bool m_showPaddingGizmo = false;
    [SerializeField] private float m_leftPadding;
    [SerializeField] private float m_rightPadding;
    [SerializeField] private float m_topPadding;
    [SerializeField] private float m_bottomPadding;

    public float LeftPadding => m_leftPadding;
    public float RightPadding => m_rightPadding;
    public float TopPadding => m_topPadding;
    public float BottomPadding => m_bottomPadding;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    //public Vector2 LetTop() => Camera.main.ScreenToWorldPoint(new Vector2(-(Screen.width / 2), Screen.height / 2));

    //public Vector2 RightTop() => Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));

    //public Vector2 LeftBottom() => Camera.main.ScreenToWorldPoint(new Vector2(-(Screen.width / 2), -(Screen.height / 2)));

    //public Vector2 RightBottom() => Camera.main.ScreenToWorldPoint(new Vector2((Screen.width / 2), -(Screen.height / 2)));

    public Vector2 LeftTop() => new Vector2(-(Camera.main.orthographicSize * Camera.main.aspect), Camera.main.orthographicSize);

    public Vector2 RightTop() => new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);

    public Vector2 LeftBottom() => new Vector2(-(Camera.main.orthographicSize * Camera.main.aspect), -Camera.main.orthographicSize);

    public Vector2 RightBottom() => new Vector2(Camera.main.orthographicSize * Camera.main.aspect, -Camera.main.orthographicSize);

    public float Height() => Camera.main.orthographicSize * 2;

    public float Width() => (Camera.main.orthographicSize * 2) * Camera.main.aspect;


#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawCube(Vector3.zero, new Vector3(Width(), Height(), 0));



        Gizmos.color = Color.green;

        Vector2 _LeftTop = LeftTop();
        Vector2 _RightBottom = RightBottom();

        _LeftTop.x += m_leftPadding;
        _LeftTop.y -= m_topPadding;

        _RightBottom.x -= m_rightPadding;
        _RightBottom.y += m_bottomPadding;

        Vector2 _RightTop = new Vector2(_RightBottom.x, _LeftTop.y);
        Vector2 _LeftBottom = new Vector2(_LeftTop.x, _RightBottom.y);

        Gizmos.DrawLine(_LeftTop, _RightTop);
        Gizmos.DrawLine(_LeftBottom, _RightBottom);

        Gizmos.DrawLine(_LeftTop, _LeftBottom);
        Gizmos.DrawLine(_RightTop, _RightBottom);
    }

#endif
}
