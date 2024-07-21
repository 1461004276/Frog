using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Generate : MonoBehaviour
{
   //创建列表存储要产生的物体
   public List<GameObject> generateObject;
   //设置物体运动朝向
   public int direction;

   private void Start()
   {
      InvokeRepeating(nameof(GenerateObject),0.2f,Random.Range(7f,10f));
   }

   void GenerateObject()
   {
      var index = Random.Range(0, generateObject.Count);
      var gameObject = Instantiate(generateObject[index],transform.position,Quaternion.identity,transform);
      gameObject.GetComponent<MoveForward>().dir = direction;
   }
   
}
