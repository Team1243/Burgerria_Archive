using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [SerializeField] private float _transitionTime;

    private Material _mat;
    private int _shaderValue = Shader.PropertyToID("_Value");

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple SceneTransition");
        Instance = this;

        _mat = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().material;
    }

    public void SceneTransitions(int num)
    {
        StartCoroutine(SceneTransitionsCo(num));
    }
    
    public void SceneTransitions(string str)
    {
        Scene scene = new Scene();
        scene.name = str;
        StartCoroutine(SceneTransitionsCo(scene.buildIndex));
    }

    private IEnumerator SceneTransitionsCo(int num)
    {
        StartCoroutine(SceneTransitionScreen(2.5f, 0));
        yield return new WaitForSeconds(_transitionTime);

        if (num >= 0)
            SceneManager.LoadScene(num);
        else 
            Application.Quit();
            
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SceneTransitionScreen(0, 2.5f));
        yield return new WaitForSeconds(_transitionTime);
        Taptic.Light();
    }

    private IEnumerator SceneTransitionScreen(float start, float end)
    {
        float currentTime = 0;

        while (currentTime < _transitionTime)
        {
            yield return null;
            currentTime += Time.deltaTime;
            float t = currentTime / _transitionTime;
            t = t == 0 ? 0 : t == 1 ? 1 : t < 0.5f ? Mathf.Pow(2, 20 * t - 10) / 2 : (2 - Mathf.Pow(2, -20 * t + 10)) / 2;
            _mat.SetFloat(_shaderValue, Mathf.Lerp(start, end, t));
        }
        _mat.SetFloat(_shaderValue, end);
    }
}
