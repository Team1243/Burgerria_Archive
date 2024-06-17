using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class IntroMovement : MonoBehaviour
{
    [SerializeField] private float _delayTime;
    [SerializeField] private float _movementTime;
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private Vector3 _endPos;

    [HideInInspector] public SpriteRenderer SpriteRenderer;
    
    private float _currentTime = 0;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_delayTime);

        while (_currentTime <= _movementTime)
        {
            yield return null;
            _currentTime += Time.deltaTime;

            float time = _currentTime / _movementTime;
            //time = Mathf.Sqrt(1 - Mathf.Pow(time - 1, 2)); //easeOutCirc
            
            float n1 = 7.5625f;
            float d1 = 2.75f;
            if (time < 1 / d1) 
                time = n1 * time * time;
            else if (time < 2 / d1) 
                time = n1 * (time -= 1.5f / d1) * time + 0.75f;
            else if (time < 2.5f / d1) 
                time = n1 * (time -= 2.25f / d1) * time + 0.9375f;
            else 
                time = n1 * (time -= 2.625f / d1) * time + 0.984375f;
            
            transform.position = Vector3.Lerp(_startPos, _endPos, time);

            if (_endPos.y + 0.12f > transform.position.y + 0.08f)
                Taptic.Medium();
        }

        transform.position = _endPos;
    }
}
