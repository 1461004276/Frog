using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button Button;

    private void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        LoadScene.instance.Loading(1);
    }
    
}
