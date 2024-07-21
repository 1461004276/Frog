using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //todo : 全局的AudioManger出现问题
    //component
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer Renderer;
    private PlayerInput PlayerInput;
    private Collider2D coll;

    [Header("跳跃")]
    //这是，每次跳跃距离
    public float jumpDistance = 20f;
    public float longJumpDistance;
    //记录最终跳跃距离
    private float moveDistance = 0f;
    //判断是否是长按
    private bool buttonHeld = false;
    //判断是否在跳跃
    [SerializeField]private bool isJump = false; 
    [SerializeField] private bool canJump = false; 
    //跳跃目的地
    private Vector2 destination;
    //储存触碰屏幕的坐标值
    private Vector2 touchPosition;
    //枚举类型，用于判断移动方向
    private enum MoveDirection
    {
        Left,
        Right,
        Up
    }

    [SerializeField]private MoveDirection dir;//移动方向
    //存储从青蛙位置发射射线接触的物体
    private RaycastHit2D[] result = new RaycastHit2D[2];
    [Header("得分 ")] 
    public int stepScore;
    [SerializeField]private int ScoreResult;


    //判断玩家是否存活
    [Header("死亡")]
    public bool isDead = false;
    private void Start()
    {
        //get component
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Renderer = GetComponent<SpriteRenderer>();
        PlayerInput = GetComponent<PlayerInput>();
        coll = GetComponent<Collider2D>();
        //set value
        longJumpDistance = jumpDistance * 2;
        destination = transform.position;
    }

    private void Update()
    {
        if (isDead)
        {
            DisableInput();
            return;
        }
        if (canJump)
        {
            TriggerJump();
        }
    }

    private void FixedUpdate()
    {
        if(isJump) rb.position = Vector2.Lerp(transform.position,destination,0.134f);
    }
    
    //检测角色碰撞
    private void OnTriggerStay2D(Collider2D other)
    {
        
        //检测是否掉入河里
        bool inWater = true;
        if (other.CompareTag("Water") && !isJump)
        {
            Physics2D.RaycastNonAlloc(transform.position + Vector3.up*0.3f , Vector2.zero, result);
            foreach (var hit in result)
            {
                if(hit.collider == null) continue;
                if (hit.collider.CompareTag("Wood"))
                {
                    //通过将青蛙变成木板子物体进行跟随移动
                    transform.parent = hit.transform;
                    inWater = false;
                    //保证木板上青蛙显示正常
                    Renderer.sortingLayerName = "Front";
                }
            }

            if (inWater)
            {
                Debug.Log("game over");
                isDead = true;
            }
            
        }

        //超出边界
        if (other.CompareTag("Border") || other.CompareTag("Car"))
        {
            Debug.Log("Game Over!");
            isDead = true;

        }
        //越过障碍物
        if (other.CompareTag("Obstacle") && !isJump)
        {
            Debug.Log("Game Over!");
            isDead = true;

        }

        if (isDead)
        {
            //广播通知游戏结束
            AudioManger.instance.setDeadClip();
            AudioManger.instance.playAudio();
            EventHandler.CallDead();
            coll.enabled = false;
        }

    }
    
    //离开碰撞体事件
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Wood")) Renderer.sortingLayerName = "Middle";
    }


    #region INPUT 输入回调函数

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && !isJump)
        {
            //Debug.Log("JUMP!"+moveDistance);
            moveDistance = jumpDistance;
            canJump = true;
            AudioManger.instance.setJumpClip(AudioManger.clipType.jump);
            if (dir == MoveDirection.Up)
            {
                //向上跳得分
                ScoreResult += stepScore;
            }
        }
    }

    public void LongJump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            moveDistance = longJumpDistance;
            buttonHeld = true;
        }

        if (context.canceled && buttonHeld && !isJump)
        {
            //long jump start
            //Debug.Log("LongJump!"+moveDistance);
            buttonHeld = false;
            canJump = true;
            AudioManger.instance.setJumpClip(AudioManger.clipType.longJump);
            if (dir == MoveDirection.Up)
            {
                //向上跳得分
                ScoreResult += stepScore * 2;
            }
        }
    }
    
    public void TouchPosition(InputAction.CallbackContext context)
    {
        //判断手机点击位置来实现不同方向的跳跃
        if(context.performed)
        {
            //将手机屏幕的像素坐标转换成世界坐标
            touchPosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            //Debug.Log(touchPosition);
            var offset = ((Vector3)touchPosition - transform.position).normalized;
            //Debug.Log(offset.x);
            //normalized是单位化,只返回0到1区间
            if (Math.Abs(offset.x) <= 0.7f)
            {
                dir = MoveDirection.Up;
            }
            else if (offset.x < 0)
            {
                dir = MoveDirection.Left;
            }
            else if(offset.x > 0)
            {
                dir = MoveDirection.Right;
            }
        }
        
    }

    #endregion
    
    /// <summary>
    /// 触发跳跃动画
    /// </summary>
    private void TriggerJump()
    {
        canJump = false;
        switch (dir)
        {
            case MoveDirection.Left:
                anim.SetBool("isSide",true);
                destination = new Vector2(transform.position.x - moveDistance, transform.position.y );
                transform.localScale = new Vector3(1,1,1);
                break;
            case MoveDirection.Right:
                anim.SetBool("isSide",true);
                destination = new Vector2(transform.position.x + moveDistance, transform.position.y );
                transform.localScale = new Vector3(-1,1,1);
                break;
            case MoveDirection.Up:
                anim.SetBool("isSide",false);
                destination = new Vector2(transform.position.x, transform.position.y + moveDistance);
                transform.localScale = new Vector3(1,1,1);
                break;
        }
        anim.SetTrigger("Jump");
    }

    #region 跳跃动画事件
    public void jumpAnimEvent()
    {
        //青蛙起跳，设置条件为true
        isJump = true;
        AudioManger.instance.playAudio();
        //跳跃时候设置角色图层在障碍物之上
        Renderer.sortingLayerName = "Front";
        //跳跃时将青蛙的
        transform.parent = null;
        
    }
    
    public void jumpEndAnimEvent()
    {
        isJump = false;
        //落地，将图层重新设置到中层
        Renderer.sortingLayerName = "Middle";

        if (dir == MoveDirection.Up && !isDead)
        {
            //通过事件委托的方式，把计分和加载场景完成
            EventHandler.CallJumpEnd(ScoreResult);
        }

    }

    #endregion

    private void DisableInput()
    {
        PlayerInput.enabled = false;
    }
}


