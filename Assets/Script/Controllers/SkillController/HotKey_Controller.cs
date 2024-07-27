using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class HotKey_Controller : MonoBehaviour
{
    #region 组件
    private SpriteRenderer sr;
    private TextMeshProUGUI myText;
    #endregion

    #region 基础属性
    private KeyCode myHotKey;
    private float hotKeyDuration = 0;
    private float colorLoosingSpeed = 5;
    #endregion

    #region 状态属性
    private bool isLoosing;
    private bool isKeyDown;
    #endregion

    #region 计时器
    private float hotKeyTimer;
    #endregion

    private GameObject bind;


    public void Start()
    {
        hotKeyTimer = hotKeyDuration;
        sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 设置热键属性
    /// </summary>
    /// <param name="_myHotKey"></param>
    /// <param name="_position"></param>
    public void SetupHotkey(KeyCode _myHotKey, Vector2 _position)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myHotKey = _myHotKey;
        transform.position = _position;
        myText.text = myHotKey.ToString();
    }
    /// <summary>
    /// 设置热键属性
    /// </summary>
    /// <param name="_myHotKey"></param>
    /// <param name="_position"></param>
    public void SetupHotkey(KeyCode _myHotKey, Vector2 _position, float _hotKeyDuration, float _colorLoosingSpeed)
    {
        SetupHotkey(_myHotKey, _position);
        hotKeyDuration = _hotKeyDuration;
        colorLoosingSpeed = _colorLoosingSpeed;
    }


    private void Update()
    {
        //判断热键是否设置了存活时间
        if (hotKeyDuration != 0)
        {
            hotKeyTimer -= Time.deltaTime;
            //热键存活时间已过，进入消失状态
            if (hotKeyTimer < 0)
            {
                isLoosing = true;
            }
        }
        //判断热键是否被按下
        if (Input.GetKeyDown(myHotKey) && !isKeyDown)
        {
            //设置已被按下状态
            isKeyDown = true;
        }
        //判断是否进入消失状态
        if (isLoosing)
        {
            //如果设置了消失速度时采用渐变消失模式，否则直接消失
            if (colorLoosingSpeed > 0)
                //渐变消失模式
                colorLoosing(colorLoosingSpeed);
            else
                Destroy(gameObject);

        }
    }

    /// <summary>
    /// 渐变消失
    /// </summary>
    /// <param name="_colorLoosingSpeed">消失速度</param>
    public void colorLoosing(float _colorLoosingSpeed)
    {
        //设置克隆对象渐变消失效果
        sr.color = new Color(0, 0, 0, sr.color.a - (Time.deltaTime * _colorLoosingSpeed));
        myText.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * _colorLoosingSpeed));
        //克隆对象不可见时销毁对象
        if (sr.color.a <= 0 && myText.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 热键是否已被按下
    /// </summary>
    /// <returns></returns>
    public bool IsKeyDown()
    {
        return isKeyDown;
    }
    /// <summary>
    /// 热键是否进入消失状态
    /// </summary>
    /// <returns></returns>
    public bool IsLoosing()
    {
        return isLoosing;
    }
    /// <summary>
    /// 设置热键是否进入消失状态
    /// </summary>
    /// <param name="_isLoosing"></param>
    public void SetLoosing(bool _isLoosing)
    {
        isLoosing = _isLoosing;
    }
    /// <summary>
    /// 设置热键绑定对象
    /// </summary>
    /// <param name="_obj"></param>
    public void SetBind(GameObject _obj) => bind = _obj;
    /// <summary>
    /// 获取热键绑定对象
    /// </summary>
    /// <returns></returns>
    public GameObject GetBind() => bind;




}
