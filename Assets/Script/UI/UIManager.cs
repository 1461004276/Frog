using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //得分文本组件
    public Text Score;
    //游戏结束面板
    public GameObject gameOverPanel;
    private void OnEnable()
    {
        EventHandler.JumpEnd += OnUpdteScore;
        EventHandler.Dead += OnGameOver;
    }
    private void OnDisable()
    {
        EventHandler.JumpEnd -= OnUpdteScore;
        EventHandler.Dead -= OnGameOver;

    }
    private void Start()
    {
        Score.text = "00";
    }
    //更改分数
    private void OnUpdteScore(int score)
    {
        Score.text = score.ToString();
    }
    //游戏结束
    private void OnGameOver()
    {
        gameOverPanel.SetActive(true);
        if(gameOverPanel.activeInHierarchy) Time.timeScale = 0f;
    }
    //restar按钮按下
    public void OnRestar()
    {
        gameOverPanel.SetActive(false);
        LoadScene.instance.Loading(1);
        Time.timeScale = 1f;
    }

    public void OnBack()
    {
        gameOverPanel.SetActive(false);
        LoadScene.instance.Loading(0);
        Time.timeScale = 1f;
    }
    
}
