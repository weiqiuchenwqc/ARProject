using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleList : MonoBehaviour {
    
    private static Transform m_PlayerRot;
    [SerializeField]
    private Toggle m_LoopTog;
    private BCTweenRotation m_TweenComponent;
    private static Dictionary<string, Transform> m_PlayerDic = new Dictionary<string, Transform>();
    private static Transform m_CurrentRole;
	
	void Awake () {
        m_PlayerRot = transform;
        m_TweenComponent = GetComponent<BCTweenRotation>();
        m_TweenComponent.enabled = false;
        Toucher.canTouch = true;

        m_PlayerDic.Clear();
        for (int i = m_PlayerRot.childCount - 1; i >= 0; --i)
        {
            Transform child = m_PlayerRot.GetChild(i);
            m_PlayerDic.Add(child.name, child);
            child.gameObject.SetActive(false);
        }

        m_LoopTog.onValueChanged.RemoveAllListeners();
        m_LoopTog.onValueChanged.AddListener(delegate (bool value)
        {
            m_TweenComponent.enabled = value;
            Toucher.canTouch = !value;
        });

    }

    public static Transform GetRoleByName(string name)
    {
        if (m_PlayerDic.ContainsKey(name))
            return m_PlayerDic[name];
        return null;
    }

    public static Transform GetRoleByIndex(int index)
    {
        if (index < m_PlayerRot.childCount)
            return m_PlayerRot.GetChild(index);
        else
            return null;
    }

    public static void SetCurrentRole(int index)
    {
        m_CurrentRole = m_PlayerRot.GetChild(index);
    }

    public static void SetCurrentRole(string name)
    {
        if (m_PlayerDic.ContainsKey(name))
            m_CurrentRole = m_PlayerDic[name];
    }

    public static Transform GetCurrentRole()
    {
        return m_CurrentRole;
    }

    public static Transform GetRoleRot()
    {
        return m_PlayerRot;
    }
}
