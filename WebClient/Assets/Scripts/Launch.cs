using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Launch : MonoBehaviour
{
    [SerializeField]
    protected RawImage cameraImage;

    [SerializeField]
    protected Text t_tips;
    private WebCamTexture webCamTex;
    private int index = 0;

    private float aspect = 9f / 16f;

    protected GameObject lastCardGo;
    void Start()
    {
        this.OnStart();
    }

    protected virtual void OnStart()
    {
        try
        {
            StartCoroutine(Cor_LaunchMobileCamera());
        }
        catch (System.Exception e)
        {
            Log("err message:" + e.Message + "\n" + e.StackTrace);
        }
    }

    private IEnumerator Cor_LaunchMobileCamera()
    {
        yield return new WaitForEndOfFrame();
        Log("v1.0.17");
        Log("Screen:" + Screen.width + "x" + Screen.height);

        //Screen.SetResolution(Screen.width, Screen.height,0);
        //float ratio = (float)Screen.width / (float)Screen.height;

     
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


                    webCamTex = new WebCamTexture(devices[index].name, Screen.width, Screen.height, 60);
                    // ʵʱ��ȡ����ͷ�Ļ���
                    webCamTex.Play();

                    cameraImage.texture = webCamTex;


                    cameraImage.rectTransform.localEulerAngles = new Vector3(0, 0, webCamTex.videoRotationAngle);

                    yield return new WaitForEndOfFrame();

                    //cameraImage.rectTransform.sizeDelta = new Vector2(Screen.width , Screen.height);

                    //Log("wh1:" + webCamTex.width + "," + webCamTex.height);
                    //Log("wh2:" + webCamTex.requestedWidth + "," + webCamTex.requestedHeight);
                    float ratio = (float)Screen.width / (float)Screen.height;
                    cameraImage.rectTransform.sizeDelta = new Vector2(Screen.width + Mathf.Clamp(Screen.width * (ratio - aspect) * 10, 0, (float)Screen.width / 2), Screen.height);

                    Log("rawImage:" + cameraImage.rectTransform.sizeDelta.x + "," + cameraImage.rectTransform.sizeDelta.y);


                    break;
                }

                yield return new WaitForEndOfFrame();
            }

            if (WebCamTexture.devices == null || WebCamTexture.devices.Length <= 0)
            {
                Log("No Cameras");
#if UNITY_EDITOR
                float ratio = (float)Screen.width / (float)Screen.height;
                cameraImage.rectTransform.sizeDelta = new Vector2(Screen.width + Mathf.Clamp(Screen.width * (ratio - aspect) * 10,0,(float)Screen.width / 2), Screen.height);
                Log("rawImage:" + cameraImage.rectTransform.sizeDelta.x + "," + cameraImage.rectTransform.sizeDelta.y);
#endif
            }
        }
    }

    protected virtual void Log(string str)
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


    public void ChangeCardGo(GameObject cardGo)
    {
        if(this.lastCardGo != null)
        {
            this.lastCardGo.SetActive(false);
        }

        if (cardGo != null)
        {
            cardGo.SetActive(true);
            this.lastCardGo = cardGo;
        }

    }
    

    private void OnApplicationQuit()
    {
        webCamTex?.Stop();
    }
}
