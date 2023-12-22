using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Launch : MonoBehaviour
{
    [SerializeField]
    private RawImage cameraImage;

    [SerializeField]
    private Text t_tips;
    private RectTransform rectTransform;
    private WebCamTexture webCamTex;
    private int lastRotationAngle;
    private int index = 0;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        t_tips.text = "v1.0.3\n";
        rectTransform = GetComponent<RectTransform>();

        // ��������ͷȨ��
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        // �����ȡ������ͷȨ��
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            // ��ȡ���е�����ͷ�豸
            for (int i = 0; i < 100; i++)
            {
                WebCamDevice[] devices = WebCamTexture.devices;

                
                if (devices != null && devices.Length > 0)
                {
                    //t_tips.text += "No Rear Camera\n";
                    index = devices.Length > 1 ? 1 : 0;

                    // ����Ϊ1������ͷһ��Ϊ��������ͷ�������ֱ�Ϊ�豸���ơ�ͼ���ȡ��߶ȡ�ˢ����
                    webCamTex = new WebCamTexture(devices[index].name, 720, 1280, 60);
                    // ʵʱ��ȡ����ͷ�Ļ���
                    webCamTex.Play();

                    cameraImage.texture = webCamTex;

                    cameraImage.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTex.videoRotationAngle);

                    break;
                }
               
                yield return new WaitForEndOfFrame();
            }

            if(WebCamTexture.devices == null || WebCamTexture.devices.Length <= 0)
            {
                t_tips.text += "No Cameras\n";
            }
        }
    }

    //�л�ǰ������ͷ
    public void SwitchCamera()
    {
        if (WebCamTexture.devices.Length < 1)
            return;

        if (webCamTex != null)
            webCamTex.Stop();

        index++;
        index = index % WebCamTexture.devices.Length;
        t_tips.text += "index:" + index + "," + WebCamTexture.devices[index].depthCameraName + "," + WebCamTexture.devices[index].kind;
        t_tips.text += "\n";

        // ���������ͼ
        webCamTex = new WebCamTexture(WebCamTexture.devices[index].name, 720, 1280, 60);
        cameraImage.texture = webCamTex;
        webCamTex.Play();

        //ǰ�ú�������ͷ��Ҫ��תһ���Ƕȣ��������ǲ���ȷ��,��������Play()������
        cameraImage.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTex.videoRotationAngle);
    }



    private void OnApplicationQuit()
    {
        webCamTex?.Stop();
    }
}
