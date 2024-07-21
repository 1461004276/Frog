using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public float offsetY;
    private float ratio;
    public float zoomBase;
    private void Start()
    {
        //计算出当前屏幕的高宽比例
        ratio = Screen.height / (float)Screen.width;
        //根据屏幕的高宽比例计算出摄像机的正交大小
        Camera.main.orthographicSize = zoomBase*ratio*0.5f;
    }

    private void LateUpdate()
    {
        //相机跟随
        transform.position = new Vector3(transform.position.x, player.position.y+offsetY*ratio, transform.position.z);
        
    }
}
