using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    // ���ű�������
    public float MinScale = 0.2f;
    public float MaxScale = 3.0f;
    // ��������
    private float scaleRate = 1f;
    // �³ߴ�
    private float newScale;

    // ����
    private Ray ray;
    private RaycastHit hitInfo;

    private bool isRotating = false;
    private Vector3 offset;

    // ��ת
    private float rotationSpeed = 5.0f;

    private void OnMouseDown()
    {
        isRotating = true;
        offset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseUp()
    {
        isRotating = false;
    }

    private void Update()
    {
        // ��ק
        if (isRotating)
        {
            float mousX = Input.GetAxis("Mouse X");
            float mousY = Input.GetAxis("Mouse Y");
            if(Mathf.Abs(mousX) > Mathf.Abs(mousY))
            {
                mousY = 0;
            }
            else if(Mathf.Abs(mousY) > Mathf.Abs(mousX))
            {
                mousX = 0;
            }
            transform.Rotate(mousY * rotationSpeed, -mousX * rotationSpeed, 0, Space.World);
        }

    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void CardReset()
    {
        transform.localRotation = Quaternion.identity;
    }
}
