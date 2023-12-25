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
    private WebCamTexture webCamTex;
    private int index = 0;

    private float aspect = 9f / 16f;
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        Log("v1.0.11");
        Log("Screen:" + Screen.width + "x" + Screen.height);
        //Screen.SetResolution(Screen.width, Screen.height,0);

        //cameraImage.rectTransform.sizeDelta = new Vector2(Screen.height, width * Screen.width / height);
        //cameraImage.rectTransform.sizeDelta = new Vector2(height * Screen.height / width, Screen.width);

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


                    webCamTex = new WebCamTexture(devices[index].name, Screen.width, (int)(Screen.width * aspect), 60);
                    // ʵʱ��ȡ����ͷ�Ļ���
                    webCamTex.Play();

                    cameraImage.texture = webCamTex;


                    cameraImage.rectTransform.localEulerAngles = new Vector3(0, 0, webCamTex.videoRotationAngle);

                    yield return new WaitForEndOfFrame();

                    cameraImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.width);

                    break;
                }
               
                yield return new WaitForEndOfFrame();
            }

            if(WebCamTexture.devices == null || WebCamTexture.devices.Length <= 0)
            {
                Log("No Cameras");
            }
        }
    }

    private void Log(string str)
    {
        t_tips.text += str + "\n";
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

        // ���������ͼ
        webCamTex = new WebCamTexture(WebCamTexture.devices[index].name, 0, 0, 60);
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
