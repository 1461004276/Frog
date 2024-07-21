using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler
{
    //使用事件的方式解决耦合
    public static event Action<int> JumpEnd;

    public static void CallJumpEnd(int score)
    {
        //if(JumpEnd != null) JumpEnd.Invoke(score);
        JumpEnd?.Invoke(score);
    }

    public static event Action Dead;

    public static void CallDead()
    {
        Dead?.Invoke();
    }
    
}
