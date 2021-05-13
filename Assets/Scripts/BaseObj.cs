// 继承 MonoBehaviour
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObj : MonoBehaviour
{
    // 通过速度来分析，速度快的先出手

    // 移动   1个引用
    protected abstract void MoveTo(Vector3 go, float dis);

    protected abstract void Damage(int damage);
        

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
