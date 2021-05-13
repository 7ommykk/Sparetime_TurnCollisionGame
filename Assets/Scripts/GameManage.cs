// 继承 GameManage 管理类
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    // 拿到所有物体
    private GameObject[] players;
    private GameObject[] enemys;
    private List<GameObject> player;
    private List<GameObject> enemy;
    private List<GameObject> team; //当前是哪一队在操作
    private int index; //索引
    public GameObject go;// 攻击目标

    public List<GameObject> GetPlayer   
    {
        set { player = value; }
        get { return player; }
    }

    public List<GameObject> GetEnemy
    {
        set { enemy = value; }
        get { return enemy; }
    }

    public List<GameObject> GetTeam
    {
        set { team = value; }
        get { return team; }
    }

    public int GetIndex
    {
        set { index = value; }
        get { return index; }
    }

    // 比Start 初始化更早
    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        player = new List<GameObject>();
        enemy = new List<GameObject>();
        foreach (var o in players)
            player.Add(o);
        foreach (var o in enemys)
            enemy.Add(o);
 
        // 发起消息，攻击
        team = player; // player 先进行攻击
        index = 0;
    }

    // Start is called before the first frame update
    //初始化一下
    void Start()
    {
        go = enemy[0];
        Announce(go);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Announce(GameObject go) // 发布命令 ，换一个人再进行攻击。
    {
        if(team.Count == 0)
        {
            if(player.Count == 0)
                Debug.Log("Sphere战胜，Cube战败");
            else
                Debug.Log("Cube战胜，Sphere战败");
            return;
        }
        team[index].SendMessage("Action", go, SendMessageOptions.DontRequireReceiver);
        team[index].GetComponent<ObjectControl>().GetMove = true;
    }
}
