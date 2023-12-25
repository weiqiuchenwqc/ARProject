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

        // 请求摄像头权限
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        // 如果获取到摄像头权限
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            // 获取所有的摄像头设备
            for (int i = 0; i < 100; i++)
            {
                WebCamDevice[] devices = WebCamTexture.devices;

                
                if (devices != null && devices.Length > 0)
                {
                    //t_tips.text += "No Rear Camera\n";
                    index = devices.Length > 1 ? 1 : 0;

                    // 索引为1的摄像头一般为后置摄像头，参数分别为设备名称、图像宽度、高度、刷新率


                    webCamTex = new WebCamTexture(devices[index].name, Screen.width, (int)(Screen.width * aspect), 60);
                    // 实时获取摄像头的画面
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

    //切换前后摄像头
    public void SwitchCamera()
    {
        if (WebCamTexture.devices.Length < 1)
            return;

        if (webCamTex != null)
            webCamTex.Stop();

        index++;
        index = index % WebCamTexture.devices.Length;

        // 创建相机贴图
        webCamTex = new WebCamTexture(WebCamTexture.devices[index].name, 0, 0, 60);
        cameraImage.texture = webCamTex;
        webCamTex.Play();

        //前置后置摄像头需要旋转一定角度，否则画面是不正确的,必须置于Play()函数后
        cameraImage.rectTransform.localEulerAngles = new Vector3(0, 0, -webCamTex.videoRotationAngle);
    }



    private void OnApplicationQuit()
    {
        webCamTex?.Stop();
    }
}
