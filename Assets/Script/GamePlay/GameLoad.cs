using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameLoad : MonoBehaviour
{
    //存储游戏场景
    public List<GameObject> load;
    //存储相机与生成点的差值
    public float offsetY;
    //存储上一次随机值
    public int lastLoadIndex;
    //过度场景
    public GameObject simpleLoad;
    private Vector3 simpleLoadPostion;


    private void OnEnable()
    {
        EventHandler.JumpEnd += check;
    }

    private void OnDisable()
    {
        EventHandler.JumpEnd -= check;
    }
    
    public void check(int o)
    {
        if (transform.position.y - Camera.main.transform.position.y <= offsetY / 2f)
        {
            transform.position = new Vector3(0, transform.position.y + offsetY, 0);
            generate();
        }
    }

    public void generate()
    {
        int loadIndex = Random.Range(0, load.Count);
        while (loadIndex == lastLoadIndex)
        {
            loadIndex = Random.Range(0, load.Count);
        }

        lastLoadIndex = loadIndex;
        Instantiate(load[loadIndex], transform.position, quaternion.identity);
        simpleLoadPostion = transform.position + new Vector3(0, -6, 0);
        Instantiate(simpleLoad, simpleLoadPostion, quaternion.identity);
    }
}
