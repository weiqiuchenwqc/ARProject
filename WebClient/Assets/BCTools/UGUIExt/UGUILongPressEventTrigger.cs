﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class UGUILongPressEventTrigger :Selectable,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler,IDragHandler
{
	public float interval=0.1f;

	[SerializeField] private UnityEvent m_OnLongpress = new UnityEvent();
    [SerializeField] private UnityEvent m_OnElseClick = new UnityEvent();

    /// <summary>
    /// 初始化长按触发器
    /// </summary>
    /// <param name="go">绑定长按事件的对象</param>
    /// <param name="clearListeners">是否清除触发器上的已有的监听</param>
    /// <returns>长按触发器对象</returns>
    public static UGUILongPressEventTrigger InitTrigger(GameObject go, bool clearListeners)
    {
        //防止按钮产生干扰不能挂载button组件
        Button btnCom = go.GetComponent<Button>();
        GameObject.DestroyImmediate(btnCom);
        UGUILongPressEventTrigger eventTrigger = go.GetComponent<UGUILongPressEventTrigger>();
        eventTrigger = eventTrigger ?? go.AddComponent<UGUILongPressEventTrigger>();
        if(clearListeners)
        {
            eventTrigger.m_OnLongpress.RemoveAllListeners();
            eventTrigger.m_OnElseClick.RemoveAllListeners();
        }
        return eventTrigger;
    }

    /// <summary>
    /// 获取长按事件对象
    /// </summary>
    public UnityEvent LongpressEvent
    {
        get
        {
            return m_OnLongpress;
        }
    }

    /// <summary>
    /// 获取长按点击对象
    /// </summary>
    public UnityEvent ElseClickEvent
    {
        get
        {
            return m_OnElseClick;
        }
    }

    private bool isPointDown=false;
	private float lastInvokeTime;
	
	// Update is called once per frame
	void Update ()
	{
		if(isPointDown)
		{
			if(Time.time-lastInvokeTime>interval)
			{
				//触发点击;
				m_OnLongpress.Invoke();
				isPointDown = false;
			}
		}

	}

	public new void OnPointerDown (PointerEventData eventData)
	{
        base.OnPointerDown(eventData);
		isPointDown = true;
		lastInvokeTime = Time.time;
	}

	public new void OnPointerUp (PointerEventData eventData)
	{
        base.OnPointerUp(eventData);
        if(isPointDown)
        {
            if(Time.time - lastInvokeTime < interval)
            {
                m_OnElseClick.Invoke();
            }
        }
		isPointDown = false;
	}

	public new void OnPointerExit (PointerEventData eventData)
	{
        base.OnPointerExit(eventData);
		isPointDown = false;
	}

	public void OnDrag (PointerEventData eventData)
	{
		isPointDown = false;
	}
}
