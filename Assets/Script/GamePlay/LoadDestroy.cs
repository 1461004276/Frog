using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadDestroy : MonoBehaviour
{
    private void Update()
    {
        checkPostion();
    }

    public void checkPostion()
    {
        if(Camera.main.transform.position.y - transform.position.y >=20) Destroy(gameObject);
    }
}
