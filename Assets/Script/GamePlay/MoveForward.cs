using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed;
    public float dir;
    private Vector2 startPostion;
    
    void Start()
    {
        startPostion = transform.position;
        transform.localScale = new Vector3(dir, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //判断汽车是否走出视野，若走出就销毁
        if(Mathf.Abs(transform.position.x - startPostion.x) > 24) Destroy(this.gameObject);
        Move();
    }

    void Move()
    {
        transform.position += transform.right * dir * speed * Time.deltaTime;
    } 
}
