using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    //渐变增量
    public float scaler;
    public static LoadScene instance;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this);

    }

    void Start()
    {
        StartCoroutine(Fade(0));
    }

    public void Loading(int index)
    {
        StartCoroutine(LoadToScene(index));
    }
    
    private IEnumerator LoadToScene(int index)
    {
        yield return Fade(1);
        yield return SceneManager.LoadSceneAsync(index);
        yield return Fade(0);

    }
    
    /// <summary>
    /// 渐变
    /// </summary>
    /// <param name="amount">1是变黑 2是透明</param>
    /// <returns></returns>
    private IEnumerator Fade(int amount)
    {
        _canvasGroup.blocksRaycasts = true;

        while (_canvasGroup.alpha != amount)
        {
            switch (amount)
            {
                case 1:
                    _canvasGroup.alpha += Time.deltaTime * scaler;
                    break;
                case 0:
                    _canvasGroup.alpha -= Time.deltaTime * scaler;
                    break;
                    
            }

            yield return null;
        }

        _canvasGroup.blocksRaycasts = false;
    }

   

}
