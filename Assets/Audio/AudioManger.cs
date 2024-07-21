using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public static AudioManger instance;

    [Header("音乐片段")] 
    public AudioClip BGMClip;

    public AudioClip jumpClip;
    public AudioClip longJumpClip;
    public AudioClip deadClip;
    [Header("音乐播放")] 
    public AudioSource BGMSource;
    public AudioSource Fx;
    
    public enum clipType
    {
        jump,
        longJump
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this.gameObject);
        DontDestroyOnLoad(this);
    }

    public void setJumpClip(clipType type)
    {
        switch (type)
        {
            case clipType.jump:
                Fx.clip = jumpClip;
                break;
            case clipType.longJump:
                Fx.clip = longJumpClip;
                break;
        }
    }
    
    public void setDeadClip()
    {
        Fx.clip = deadClip;
    }

    public void playAudio()
    {
        Fx.Play();
    }

}
