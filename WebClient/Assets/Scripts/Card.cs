using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private bool isOpera = false;
    // 旋转
    private float rotationSpeed = 5.0f;

    private Vector2 oldPos01;
    private Vector2 oldPos02;


    private void OnMouseDown()
    {
        isOpera = true;
    }

    private void OnMouseUp()
    {
        isOpera = false;
    }

    private void Update()
    {
        if (isOpera)
        {
            float mousX = Input.GetAxis("Mouse X");
            float mousY = Input.GetAxis("Mouse Y");
            if (Mathf.Abs(mousX) > Mathf.Abs(mousY))
            {
                mousY = 0;
            }
            else if (Mathf.Abs(mousY) > Mathf.Abs(mousX))
            {
                mousX = 0;
            }
            transform.Rotate(mousY * rotationSpeed, -mousX * rotationSpeed, 0, Space.World);

            if (Input.touchCount == 2)
            {
                //第一个手指 或者第二个手指
                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    Vector2 temPos1 = Input.GetTouch(0).position;
                    Vector2 temPos2 = Input.GetTouch(1).position;
                    if (IsEnlarge(oldPos01, oldPos02, temPos1, temPos2))
                    {
                        //放大
                        float oldScale = transform.localScale.x;
                        float newScale = Mathf.Clamp(oldScale * 1.025f, 0.5f, 2f);
                        transform.localScale = new Vector3(newScale, newScale, newScale);
                    }
                    else
                    {
                        //缩小
                        float oldScale = transform.localScale.x;
                        float newScale = Mathf.Clamp(oldScale / 1.025f, 0.5f, 2f);

                        transform.localScale = new Vector3(newScale, newScale, newScale);
                    }

                    oldPos01 = temPos1;
                    oldPos02 = temPos2;
                }

            }
        }

    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    //判断手势
    bool IsEnlarge(Vector2 op1, Vector2 op2, Vector2 np1, Vector2 np2)
    {
        //Mathf.Sqrt返回的平方根
        float lenth01 = Mathf.Sqrt((op1.x - op2.x) * (op1.x - op2.x) + (op1.y - op2.y) * (op1.y - op2.y));
        float lenth02 = Mathf.Sqrt((np1.x - np2.x) * (np1.x - np2.x) + (np1.y - np2.y) * (np1.y - np2.y));
        if (lenth01 < lenth02)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void CardReset()
    {
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
