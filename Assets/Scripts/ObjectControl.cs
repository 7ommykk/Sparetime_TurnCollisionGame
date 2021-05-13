// 继承 BaseObj
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControl : BaseObj
{
    // Vector3 欧拉角-旋转
    private Vector3 primitivePos;
    private Vector3 primitiveRot;
    private Vector3 tempPos;
    private bool canMove;

    public int hp = 100;

    // 变量当形参
    private Vector3 targetPos;
    private float distance;

    private GameManage gm;

    public bool GetMove
    {
        set { canMove = value; }
        get { return canMove;}
    }


    // Start is called before the first frame update
    void Start()
    {
        distance = 1;
        canMove = false;
        primitivePos = transform.position;
        primitiveRot = transform.eulerAngles;
        // 赋值
        tempPos = Vector3.zero;
        targetPos = Vector3.zero;
        gm = GameObject.Find("GameManage").GetComponent<GameManage>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            MoveTo(targetPos, distance);

        Debug.Log(canMove);
    }


    protected override void Damage(int damage)
    {
        if(hp > 0)
        {
            hp -= damage;
            if(hp <= 0)
            {
                Debug.Log("死亡");
                hp = 0;
                if(gameObject.tag == "Player")
                    gm.GetPlayer.Remove(gameObject);
                else
                    gm.GetEnemy.Remove(gameObject);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("受伤");
            }
        }

    }

    // dis: 跑过去还是跑回来   原始位置/敌人位置  碰撞
    protected override void MoveTo(Vector3 go, float dis)
    {
        if (Vector3.Distance(transform.position, go) > dis)
        {
            // 和玩家等高
            tempPos = go;
            tempPos.y = transform.position.y;
            transform.LookAt(tempPos);
            transform.Translate(Vector3.forward * Time.deltaTime * 5);
        }
        else
        {
            canMove = false;
            if (dis > 0.1f)
            {
                // 攻击
                // Debug.Log(666);
                Attack();
                //返回阶段的参数变化
                GoBackParameter();
            }
            else
            {
                // 返回到原始位置
                GoBack();
                ChangeNext();
            }

        }

    }

    private void Attack()
    {
        if(gm.go != null)
            gm.go.GetComponent<ObjectControl>().Damage(50);
    }


    private void GoBack()
    {
        transform.position = primitivePos;
        transform.eulerAngles = primitiveRot;
        canMove = false;
    }

    private void GoBackParameter()
    {
        canMove = true;
        targetPos = primitivePos;
        distance = 0.1f;
    }

    // 如何切换下一个玩家
    private void ChangeNext() // 同队换人
    {
        // 到底位于第几个攻击的。小于当前正在进行操作队伍的总人数
        if (gm.GetIndex < gm.GetTeam.Count - 1)
        {
            // 后面还有人没有攻击的
            gm.GetIndex++;
            distance = 1;
            gm.Announce(gm.go);
        }
        else 
        {
            // 换队伍
            ChangeTeam();
        }
    } 
    
    private void Action(GameObject go)
    {
        targetPos = go.transform.position;
    }

    private void ChangeTeam()
    {
        // 参数复位，初始化
        gm.GetIndex = 0;
        distance = 1;

        if(gm.GetTeam == gm.GetPlayer)
        {
            gm.GetTeam = gm.GetEnemy;
            gm.go = gm.GetPlayer[0];
        }
        else
        {
            gm.GetTeam = gm.GetPlayer;
            gm.go = gm.GetEnemy[0];
        }
        gm.Announce(gm.go);
    }
}
