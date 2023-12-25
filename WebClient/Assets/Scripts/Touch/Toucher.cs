using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toucher : MonoBehaviour {

    private static bool m_CanTouch = true;
    public static bool canTouch
    {
        get{ return m_CanTouch; }
        set{ m_CanTouch = value; }
    }
    private Vector3 m_PreMousePos = -Vector3.one;
	
	void Update ()
    {
        if (!m_CanTouch) return;

        Transform currentRole = RoleList.GetCurrentRole();
#if UNITY_EDITOR
		if (Input.GetMouseButton(0))
		{
			Vector3 curMousePos = Input.mousePosition;
			if (-Vector3.one != m_PreMousePos)
			{
				RoleList.GetRoleRot().Rotate(Vector3.up, -(curMousePos - m_PreMousePos).x);
			}
			m_PreMousePos = Input.mousePosition;
		}
		else
			m_PreMousePos = -Vector3.one;
		
#elif UNITY_ANDROID || UNITY_IPHONE
		Touch[] touch = Input.touches;
		switch (Input.touchCount)
		{
		case 1:
		RoleList.GetRoleRot().Rotate(Vector3.up, -touch[0].deltaPosition.x);
		break;
		case 2:
		break;
		}
#endif
    }
}
